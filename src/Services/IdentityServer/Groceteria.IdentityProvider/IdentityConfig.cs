using Groceteria.IdentityProvider.Configurations.Client;
using Groceteria.IdentityProvider.Extensions;
using Groceteria.IdentityProvider.Models.Enums;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace Groceteria.IdentityProvider
{
    public class IdentityConfig
    {
        public static IEnumerable<Client> Clients(DefaultApiClients apiClients) =>
            new List<Client>
            {
                new Client
                {
                    ClientName = apiClients.IdentityManagerClient.ClientName,
                    ClientId = apiClients.IdentityManagerClient.ClientId,
                    ClientSecrets =
                    {
                        new Secret(apiClients.IdentityManagerClient.ClientSecret.Sha512())
                    },
                    AllowedGrantTypes = { 
                        GrantType.ClientCredentials, 
                        GrantType.AuthorizationCode, 
                        GrantType.ResourceOwnerPassword 
                    },
                    AllowedScopes =
                    {
                        DefaultApiScopes.IdentityManagerApi.GetEnumMemberAttributeValue(),
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        DefaultScopes.Roles.GetEnumMemberAttributeValue(),
                    },
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RedirectUris = { apiClients.IdentityManagerClient.RedirectUris },
                    PostLogoutRedirectUris = { apiClients.IdentityManagerClient.RedirectUris },
                    AccessTokenType = AccessTokenType.Jwt
                }
            };
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope(DefaultApiScopes.IdentityManagerApi.GetEnumMemberAttributeValue(), "Identity Manager API")
            };
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("groceteria.identitymanager.api", "Identity Manager API")
                {
                    Scopes = { 
                        DefaultApiScopes.IdentityManagerApi.GetEnumMemberAttributeValue()
                    },
                    UserClaims = { "roles", "firstName", "lastName", "email", "username" }
                }
            };
        public static IEnumerable<IdentityResource> IdentityResources =>
           new IdentityResource[]
           {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
                {
                    Required = true,
                    UserClaims = { "firstName", "lastName", "username" }
                },
                new IdentityResources.Email()
                {
                    Required = true
                },
                new IdentityResource("roles", "User role(s)", new List<string>{ "roles" })
                {
                    Required = true
                }
           };
        public static List<TestUser> TestUsers =>
           new List<TestUser>
           {
                new TestUser
                {
                    SubjectId = "abc1",
                    Username = "Frank",
                    Password = "FrankPass",
                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Frank"),
                        new Claim("family_name", "Ozz"),
                        new Claim("address", "132 sample road, India"),
                        new Claim("roles", "Admin")
                    },
                },
                new TestUser
                {
                    SubjectId = "abc2",
                    Username = "Seros",
                    Password = "SerosPass",
                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Seros"),
                        new Claim("family_name", "Ozz"),
                        new Claim("roles", "Visitor")
                    }
                }
           };
    }
}
