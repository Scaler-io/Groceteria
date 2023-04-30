using Groceteria.Shared.Core;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Groceteria.Basket.Api.Models.Requests
{
    public class ShoppingCartFetchRequest
    {
        [Required(ErrorMessage = "Username is required")]
        [JsonProperty("Username")]
        public string Username { get; set; }
    }
}
