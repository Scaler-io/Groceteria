namespace Groceteria.IdentityManager.Api.Models.Contracts;

public class IdentityResourceSummary
{
    public int ResourceId { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public bool Enabled { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}
