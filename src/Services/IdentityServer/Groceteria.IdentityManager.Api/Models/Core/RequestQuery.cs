namespace Groceteria.IdentityManager.Api.Models.Core
{
    public class RequestQuery
    {
        private int MaxPageSize { get; set; } = 50;
        public int PageIndex { get; set; } = 1;

        public string SortField { get; set; }
        public string SortOrder { get; set; } = "Asc";

        private int _pageSize { get; set; } = 5;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
