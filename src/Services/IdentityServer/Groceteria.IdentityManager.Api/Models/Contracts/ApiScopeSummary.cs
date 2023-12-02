namespace Groceteria.IdentityManager.Api.Models.Contracts
{
    public class ApiScopeSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
