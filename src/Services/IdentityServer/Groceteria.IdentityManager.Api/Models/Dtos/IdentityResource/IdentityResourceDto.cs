namespace Groceteria.IdentityManager.Api.Models.Dtos.IdentityResource;

public class IdentityResourceDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public bool Required { get; set; }
    public bool Enabled { get; set; }
    public List<ClaimsDto> UserClaims { get; set; }
    public MetaDataDto MetaData { get; set; }
}
