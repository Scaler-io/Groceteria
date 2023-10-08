using Groceteria.IdentityManager.Api.Models.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Groceteria.IdentityManager.Api.Swagger.Examples
{
    public class ApiClientResultExample : IExamplesProvider<ApiClientDto>
    {
        public ApiClientDto GetExamples()
        {
            return new ApiClientDto
            {
                Id = 1,
                Enabled = true,
                ClientId = "my-client-id",
                ClientName = "Sample Client",
                ClientDescription = "A sample client for testing purposes",
                AllowedGrantTypes = new List<string> { "authorization_code", "client_credentials" },
                ClientSecrets = new List<ClientSecretDto>
                    {
                        new ClientSecretDto
                        {
                            Description = "Primary secret",
                            Value = "my-secret-value",
                            Expiration = "2024-08-23T00:00:00Z"
                        }
                    },
                RedirectUris = new List<string> { "https://example.com/callback" },
                PostLogoutRedirectUris = new List<string> { "https://example.com/logout-callback" },
                AllowedScopes = new List<string> { "openid", "profile", "api" },
                ProtocolType = "oidc",
                AccessTokenLifetime = 3600, // seconds
                AuthorizationCodeLifetime = 300, // seconds
                IdentityTokenLifetime = 900, // seconds
                RequireClientSecret = true,
                AlwaysSendClientClaims = false,
                UpdateAccessTokenClaimsOnRefresh = true,
                AllowOfflineAccess = true,
                RequirePkce = true,
                MetaData = new MetaDataDto
                {
                    CreatedOn = "2023-08-23T10:00:00Z",
                    UpdatedOn = "2023-08-23T15:30:00Z",
                    LastAccesseOn = "2023-08-23T18:45:00Z"
                }
            };
        }
    }
}
