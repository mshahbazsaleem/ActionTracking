using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.WebHost.Infrastructures
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("at_api", "Action Tracking API")
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[]
            {
                new ApiScope(name: "at_api",   displayName: "Action Tracking API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "ActionTracking",
                    ClientName = "MAA Action Tracking Application",
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    RequireConsent = false,
                    RequirePkce = false,

                    ClientSecrets =
                    {
                        new Secret("R#L0cked!!".Sha256())
                    },

                    RedirectUris = { "https://localhost:44394/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:44394/signout" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "at_api"
                    },
                    AllowOfflineAccess = true,
                    UpdateAccessTokenClaimsOnRefresh = true
                }
        };

        }
    }
}
