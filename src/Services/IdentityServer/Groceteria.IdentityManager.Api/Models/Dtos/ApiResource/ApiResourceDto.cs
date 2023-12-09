using Groceteria.IdentityManager.Api.Models.Dtos.ApiScope;

namespace Groceteria.IdentityManager.Api.Models.Dtos.ApiResource
{
    public class ApiResourceDto
    {
        public int Id { get; set; }
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string AllowedAccessTokenSigningAlgorithms { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public List<ApiResourceSecretDto> Secrets { get; set; }
        public List<ApiResourceScopeDto> Scopes { get; set; }
        public List<ClaimsDto> UserClaims { get; set; }
        public MetaDataDto Metadata { get; set; }
    }
}