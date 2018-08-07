// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.Quickstart.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Host.Models;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.AspNetIdentity;
using Host.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using System.Collections.Generic;
using Host.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using IdentityServer4.EntityFramework;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Entities;
using IdentityServer4.EntityFramework.UserContext;
using IdentityServer4;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Polly;

using Microsoft.AspNetCore.HttpOverrides;
using Polly.Retry;
using System.Net.Sockets;
using RabbitMQ.Client.Exceptions;
using MassTransit.RabbitMqTransport;

namespace Host
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;

        public Startup(IConfiguration config, IHostingEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string configsConnectionString;  
            string usersConnectionString; 
            string rabbitHostString;
            if (env == "Development")
            {
                usersConnectionString = "Server=localhost;Database=microstarter.identity_users;Username=doom;Password=machine";
                configsConnectionString= "Server=localhost;Database=microstarter.identity_config;Username=doom;Password=machine";  
                rabbitHostString = "rabbitmq://localhost";
            }
            else // if (env == "Docker_Production")
            {
                usersConnectionString = "Server=postgre_name;Database=microstarter.identity_users;Username=doom;Password=machine";
                configsConnectionString= "Server=postgre_name;Database=microstarter.identity_config;Username=doom;Password=machine";  
                rabbitHostString = "rabbitmq://MicroStarterEventBus";
            }
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            
            services.AddDbContext<UserDbContext>(options =>
                options.UseNpgsql(usersConnectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly))
                );
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<UserDbContext>();            
            
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseSuccessEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;                    
                })
                .AddDeveloperSigningCredential()
                // .AddTestUsers(TestUsers.Users)
                .AddAspNetIdentity<ApplicationUser>()
                // You can Configure Profile Service for your needs
                .AddProfileService<AuthProfileService>()
                // this adds the config data from DB (clients, resources, CORS)
                .AddConfigurationStore(options =>
                {
                    options.ResolveDbContextOptions = (provider, builder) =>
                    {
                        builder.UseNpgsql(configsConnectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));                    
                    };
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = (builder) =>
                    {
                        builder.UseNpgsql(configsConnectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                    };
                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    // options.TokenCleanupInterval = 10; // interval in seconds, short for testing
                })
                .AddConfigurationStoreCache();



            services.AddMassTransit(p=>{
                // p.AddConsumer<SomeEventHappenedConsumer>();
            });            
            var _retryCount = 8;
            var policy = RetryPolicy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .Or<RabbitMqConnectionException>()
                .OrInner<BrokerUnreachableException>()
                .OrInner<RabbitMqConnectionException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    Console.WriteLine("Could not connect Broker Trying Again");
                    Console.WriteLine(ex);
                    Console.WriteLine("Retrying RabbitMq Connection");
                }
            );
            IServiceProvider prov = services.BuildServiceProvider();
            IBusControl busControl;
            policy.Execute(() =>
            {
                busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri(rabbitHostString), "/", h => {
                        h.Username("doom");
                        h.Password("machine");
                    });

                    cfg.ReceiveEndpoint(host, e =>
                    {
                        e.PrefetchCount = 8;
                        // Add Your Event Consumers Here
                        // If you want Inject services to consumer, pass provider param
                        // e.Consumer<SomeEventHappenedConsumer>(provider)
                    });
                });
                services.AddSingleton(provider => busControl);
            });
            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
            // Register with IHostedService To Start bus in Application Start
            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, BusService>();
            services.AddSingleton<LocService>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddScoped<ClientIdFilter>();
            services.AddScoped<ClientSelector>();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ClientViewLocationExpander());
            });

            services.Configure<RequestLocalizationOptions>(
            options =>
            {
                var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en-US"),
                        new CultureInfo("de-CH"),
                        new CultureInfo("fr-CH")
                    };

                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                var providerQuery = new LocalizationQueryProvider
                {
                    QureyParamterName = "ui_locales"
                };

                // Cookie is required for the logout, query parameters at not supported with the endsession endpoint
                // Only works in the same domain
                var providerCookie = new LocalizationCookieProvider
                {
                    CookieName = "defaultLocale"
                };
                // options.RequestCultureProviders.Insert(0, providerCookie);
                options.RequestCultureProviders.Insert(0, providerQuery);
            });
            
            services.AddMvc()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
                        return factory.Create("SharedResource", assemblyName.Name);
                    };
                });

            return services.BuildServiceProvider(validateScopes: false);
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env
            ,ILoggerFactory loggerFactory
        )
        {
            loggerFactory.AddConsole(_config.GetSection("Logging"));
            loggerFactory.AddDebug();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {                
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);


            app.UseStaticFiles();

            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
 
        }
    }
}
