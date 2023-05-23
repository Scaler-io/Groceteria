namespace Groceteria.SalesOrder.Application.Models
{
    public class EmailField
    {
        public EmailField(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}
