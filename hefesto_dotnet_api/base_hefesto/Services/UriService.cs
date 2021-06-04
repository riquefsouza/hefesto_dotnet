using hefesto.base_hefesto.Pagination;
using Microsoft.AspNetCore.WebUtilities;
using System;

namespace hefesto.base_hefesto.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetPageUri(PaginationFilter filter, string route)
        {
            var _enpointUri = new Uri(string.Concat(_baseUri, route));
            var modifiedUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "pageNumber", filter.pageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "size", filter.size.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "sort", filter.sort.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "columnOrder", filter.columnOrder.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "columnTitle", filter.columnTitle.ToString());

            return new Uri(modifiedUri);
        }
    }
}
