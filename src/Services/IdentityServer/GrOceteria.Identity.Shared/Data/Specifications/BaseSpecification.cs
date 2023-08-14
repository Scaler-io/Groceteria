using Groceteria.Identity.Shared.Data.Interfaces;
using System.Linq.Expressions;

namespace Groceteria.Identity.Shared.Data.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : class
    {
        public BaseSpecification() { }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }

        public List<string> IncludeStrings { get; } = new List<string>();

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool isPageingEnabled { get; private set; }

        protected void AddIncludes(string includeString)
        {
            this.IncludeStrings.Add(includeString);
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderBy)
        {
            this.OrderBy = orderBy;
        }

        protected void AddOrderbyDescending(Expression<Func<T, object>> orderByDesc)
        {
            this.OrderByDescending = orderByDesc;
        }

        protected void ApplyPaging(int skip, int take)
        {
            Take = take;
            Skip = skip;
            isPageingEnabled = true;
        }
    }
}
