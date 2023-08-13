using System.Runtime.Serialization;

namespace Groceteria.IdentityProvider.Models.Enums
{
    public enum DefaultApiScopes
    {
        [EnumMember(Value = "groceteria.identitymanager.api")]
        IdentityManagerApi,
    }
}
