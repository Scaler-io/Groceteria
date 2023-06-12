using FluentValidation;
using Groceteria.Shared.Enums;
using System.Linq.Expressions;
using System.Reflection;

namespace Groceteria.SalesOrder.Application.Extensions
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> Required<T, TProperty>(this IRuleBuilder<T, TProperty>  rule, string message = "")
        {
            if (string.IsNullOrEmpty(message)) 
            {
                message = $"The field is required";
            }
            return rule.NotEmpty()
                .WithErrorCode(ErrorCode.NotFound.ToString())
                .WithMessage(message) 
                .NotNull()
                .WithErrorCode(ErrorCode.NotFound.ToString())
                .WithMessage(message);
        }
    }
}
