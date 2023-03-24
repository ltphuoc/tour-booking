using DataAccess.DTO.Request;
using System.Linq.Dynamic.Core;

namespace DataAccess.Helpers
{

    public enum SortOrder
    {
        Ascending = 0,
        Descending = 1,
        None = 2
    }

    public static class DynamicQueryHelper
    {
        public static IQueryable<T> ApplySearchSortAndPaging<T>(IQueryable<T> query, PagingRequest request)
        {
            query = ApplySearch(query, request.SearchBy, request.KeySearch);
            query = ApplySort(query, request.ColName, (SortOrder)request.SortType);
            return ApplyPaging(query, request.Page, request.PageSize);
        }
        public static IQueryable<T> ApplyPaging<T>(IQueryable<T> query, int page, int pageSize)
        {
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public static IQueryable<T> ApplySearch<T>(IQueryable<T> query, string searchBy, string keySearch)
        {
            if (string.IsNullOrEmpty(keySearch))
            {
                return query;
            }

            var searchExpression = BuildSearchExpression<T>(searchBy);
            return query.Where(searchExpression, keySearch);
        }

        public static IQueryable<T> ApplySort<T>(IQueryable<T> query, string colName, SortOrder sortType)
        {
            var sortExpression = BuildSortExpression<T>(colName, sortType);
            return query.OrderBy(sortExpression);
        }

        private static string BuildSearchExpression<T>(string searchBy)
        {
            return $"{searchBy}.Contains(@0)";
        }

        private static string BuildSortExpression<T>(string colName, SortOrder sortType)
        {
            return $"{colName} {(sortType == SortOrder.Ascending ? "ASC" : "DESC")}";
        }
    }

}
