namespace Groceteria.IdentityManager.Api.Models.Contracts
{
    public class ApiClientSummary
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientDescription { get; set; }
        public int AccessTokenLifetime { get; set; }
        public bool Enabled { get; set; }
        public bool IsDefault { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? LastAccessed { get; set; }
    }
}
