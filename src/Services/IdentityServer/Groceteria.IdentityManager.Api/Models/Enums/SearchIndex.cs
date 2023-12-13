using System.Runtime.Serialization;

namespace Groceteria.IdentityManager.Api.Models.Enums
{
    public enum SearchIndex
    {
        [EnumMember(Value = "ApiClient")]
        ApiClient,
        [EnumMember(Value = "ApiScope")]
        ApiScope,
        [EnumMember(Value = "ApiResource")]
        ApiResource,
        [EnumMember(Value = "IdentityResource")]
        IdentityResource
    }
}
