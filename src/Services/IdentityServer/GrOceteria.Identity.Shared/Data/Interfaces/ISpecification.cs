using System.Linq.Expressions;

namespace Groceteria.Identity.Shared.Data.Interfaces
{
    public interface ISpecification<T> where T: class
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<string> IncludeStrings { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        int Take { get; }
        int Skip { get; }
        bool isPageingEnabled { get; }
    }
}
