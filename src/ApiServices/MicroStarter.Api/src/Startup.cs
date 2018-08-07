
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using IdentityServer4.AccessTokenValidation;
    // In Future..
using Microsoft.Extensions.Logging;
using MicroStarter.Api.Services;
using MicroStarter.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Swashbuckle.AspNetCore.Swagger;
namespace MicroStarter.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string connectionString;   
            string authConnectionIssuerUrl;            
            string rabbitHostString;
            if (env == "Development")
            {
                connectionString= "Server=localhost;Database=MicroStarter.Api;Username=doom;Password=machine";  
                authConnectionIssuerUrl= "http://localhost:5000";
                rabbitHostString = "localhost";
            }
            else // if (env == "Docker_Production")
            {
                connectionString= @"Server=postgre_name;Database=MicroStarter.Api;Username=doom;Password=machine";  
                authConnectionIssuerUrl= "microstarter.identity.localhost";
                rabbitHostString = "rabbitmq://MicroStarterEventBus";
            }
            services.AddDbContext<MicroStarterApiContext>(options =>
                options.UseNpgsql(connectionString)
                );
            services.AddMemoryCache(options => {
                // Your options
            });            
            services.AddAuthorization(options =>
            {
                // options.AddPolicy("Your_Authorization", policyUser =>
                // {
                //      // You may want change below for your requirements
                //      policyUser.RequireRole("Admin");
                // });
            });
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
               .AddIdentityServerAuthentication(options =>
               {
                   options.Authority = authConnectionIssuerUrl;
                   options.ApiName = "MicroStarter.Api";
                   options.ApiSecret = "secret";
                   options.RequireHttpsMetadata = false;
                   options.SupportedTokens = SupportedTokens.Both;
               });
            services.AddMassTransit(p=>{
                // p.AddConsumer<AnEventHappenedConsumer>();
            });
            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(rabbitHostString), "/", h => {
                    h.Username("doom");
                    h.Password("machine");
                });

                cfg.ReceiveEndpoint(host, e =>
                {
                    e.PrefetchCount = 8;
                    // Add Event Consumers Here Like:
                    // e.Consumer<AnEventHappenedConsumer>();
                    // If you want Inject Services in Consumer add provider parameter like below.
                    // e.Consumer<AnEventHappenedConsumer>(provider);
                });
            }));
            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
            // Register with IHostedService To Start bus when Application Starts and Stop when Application Stops
            // Then you can Inject IBus to publish your messages
            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, BusService>();        
            // You may want to change allowed origins for security.
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()                   
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .Build());
            });
            services
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env
            ,ILoggerFactory loggerFactory
        )
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseHsts(); After you configure SSL with nginx
            }
            app.UseAuthentication();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseCors("CorsPolicy");
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            // app.UseHttpsRedirection(); After you configure SSL with nginx
            app.UseMvc();
        }
    }
}
