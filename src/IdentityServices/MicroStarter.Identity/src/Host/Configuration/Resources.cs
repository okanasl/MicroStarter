// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Host.Configuration
{
    public class Resources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new[]
            {
                // some standard scopes from the OIDC spec
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                // custom identity resource with some consolidated claims
                new IdentityResource("roles", new[] { JwtClaimTypes.Role })
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {                
                new ApiResource
                {
                    Name = "MicroStarter.Api",
                    DisplayName = "MicroStarter.Api",
                    ApiSecrets =
                    {
                        new Secret("MicroStarter.Identity_bar_secret".Sha256())
                        
                    },
                    UserClaims =
                    {
                        "sub",
						"updated_at",
						"iat",
						"amr",
						"sid",
						"jti",
						"client_id",
						"scope",
						"role",
						"id",
						"exp",
						"aud",
						"name",
						"given_name",
						"family_name",
						"middle_name",
						"nickname",
						"preferred_username",
						"profile",
						"picture",
						"email_verified",
						"email"

                    },
                    Scopes =
                    {
                        new Scope() { Name="bar_api_read"},
						new Scope() { Name="bar_api_write"},
						new Scope() { Name="bar_api_fullaccess"}

                    }
                }
            };
        }
    }
}