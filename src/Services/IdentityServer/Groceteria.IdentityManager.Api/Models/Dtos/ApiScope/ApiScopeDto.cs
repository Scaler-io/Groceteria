namespace Groceteria.IdentityManager.Api.Models.Dtos.ApiScope
{
    public class ApiScopeDto
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public bool ShouldShowInDiscovery { get; set; }
        public IEnumerable<ApiScopeClaimsDto> UserClaims { get; set; }
    }
}
