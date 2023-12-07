namespace Groceteria.IdentityManager.Api.Models.Dtos
{
    public class ApiClientDto
    {
        public int Id { get; set; }
        public bool Enabled { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientDescription { get; set; }
        public bool IsDefault { get; set; }

        public List<string> AllowedGrantTypes { get; set; }
        public List<ClientSecretDto> ClientSecrets { get; set; }
        public List<string> RedirectUris { get; set; }
        public List<string> PostLogoutRedirectUris { get; set; }
        public List<string> AllowedScopes { get; set; }
        public string ProtocolType { get; set; }

        // TOKEN OPTIONS
        public int AccessTokenLifetime { get; set; }
        public int AuthorizationCodeLifetime { get; set; }
        public int IdentityTokenLifetime { get; set; }

        // FLAGS
        public bool RequireClientSecret { get; set; }
        public bool AlwaysSendClientClaims { get; set; }
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public bool RequirePkce { get; set; }
        public MetaDataDto MetaData { get; set; }
    }
}
