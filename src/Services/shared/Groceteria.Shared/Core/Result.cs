using Groceteria.Shared.Enums;

namespace Groceteria.Shared.Core
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public ErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public static Result<T> success(T value)
        {
            return new Result<T> { IsSuccess = true, Value = value };
        }
        public static Result<T> Failure(ErrorCode errorCode, string errorMessage = "")
        {
            return new Result<T> { IsSuccess = false, ErrorCode = errorCode, ErrorMessage = errorMessage };
        }
    }
}
