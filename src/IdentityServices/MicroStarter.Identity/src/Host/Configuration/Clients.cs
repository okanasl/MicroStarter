// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Host.Configuration
{
    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "MicroStarter.AngularSsrClient",
                    ClientName = "MicroStarter.AngularSsrClient",
                    ClientUri = "http://microstarter.angularssrclient.localhost",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireClientSecret = false,
                    AccessTokenType = AccessTokenType.Reference,                    
                    RedirectUris = 
                    {
                       "http://microstarter.angularssrclient.localhost",
						"http://microstarter.angularssrclient.localhost/silent-renew.html",
						"http://microstarter.angularssrclient.localhost/login-callback.html"
                    },
                    PostLogoutRedirectUris =
                    { 
                        "http://microstarter.angularssrclient.localhost",
						"http://microstarter.angularssrclient.localhost/loggedout"
                    },
                    AllowedCorsOrigins =
                    { 
                        "http://microstarter.angularssrclient.localhost",

                    },
                    AllowedScopes =
                    {
                        "openid",
						"email",
						"profile",
						"bar_api_fullaccess"

                    }
                },
                new Client
                {
                    ClientId = "MicroStarter.AngularSsrClientdev",
                    ClientName = "MicroStarter.AngularSsrClientdev",
                    ClientUri = "http://localhost:4200",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireClientSecret = false,
                    AccessTokenType = AccessTokenType.Reference,                    
                    RedirectUris = 
                    {
                       "http://localhost:4200",
						"http://localhost:4200/silent-renew.html",
						"http://localhost:4200/login-callback.html"
                    },
                    PostLogoutRedirectUris =
                    { 
                        "http://localhost:4200",
						"http://localhost:4200/loggedout"
                    },
                    AllowedCorsOrigins =
                    { 
                        "http://localhost:4200",

                    },
                    AllowedScopes =
                    {
                        "openid",
						"email",
						"profile",
						"bar_api_fullaccess"

                    }
                }
            };
        }
    }
}