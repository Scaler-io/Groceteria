using Groceteria.Shared.Enums;

namespace Groceteria.Infrastructure.EventBus.Message.Models
{
    public class EventResponse
    {
        public EventResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public int StatusCode { get; set; }
        public ErrorCode? Category { get; set; }
        public string Message { get; set; }
    }
}
