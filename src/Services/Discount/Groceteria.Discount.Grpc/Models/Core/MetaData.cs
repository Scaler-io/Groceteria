using Newtonsoft.Json;

namespace Groceteria.Discount.Grpc.Models.Core
{
    public class MetaData
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
