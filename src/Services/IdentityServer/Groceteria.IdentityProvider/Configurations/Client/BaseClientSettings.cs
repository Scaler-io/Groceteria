namespace Groceteria.IdentityProvider.Configurations.Client
{
    public class BaseClientSettings
    {
        public string ClientName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUris { get; set; }
    }
}
