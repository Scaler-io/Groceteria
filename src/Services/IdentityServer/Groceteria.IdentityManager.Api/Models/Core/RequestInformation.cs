namespace Groceteria.IdentityManager.Api.Models.Core
{
    public class RequestInformation
    {
        public string CorrelationId { get; set; }
        public UserDto CurrentUser { get; set; }
    }
}
