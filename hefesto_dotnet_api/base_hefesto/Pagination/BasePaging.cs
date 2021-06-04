using System;
using System.Globalization;
using System.Collections.Generic;
using hefesto.base_hefesto.Services;

namespace hefesto.base_hefesto.Pagination
{
    public class BasePaging
    {
        private const int PAGINATION_STEP = 3;

        public bool NextEnabled { get; set; }
        public bool PrevEnabled { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public string PageSort { get; set; }
        public int ColumnOrder { get; set; }
        public string ColumnTitle { get; set; }

        private List<BasePageItem> items = new List<BasePageItem>();

        public int TotalRecords { get; set; }
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }

        public BasePaging(): base()
        {
            this.TotalRecords = 0;
        }

        public BasePaging(bool nextEnabled, bool prevEnabled, int pageSize, int pageNumber,
                String pageSort, int columnOrder, String columnTitle,
                List<BasePageItem> items)
        {
            this.NextEnabled = nextEnabled;
            this.PrevEnabled = prevEnabled;
            this.PageSize = pageSize;
            this.PageNumber = pageNumber;

            this.PageSort = pageSort;
            this.ColumnOrder = columnOrder;
            this.ColumnTitle = columnTitle;

            this.items = items;
        }

        public void addPageItems(int from, int to, int pageNumber)
        {
            for (int i = from; i < to; i++)
            {
                items.Add(BasePageItem.builder()
                                  .active(pageNumber != i)
                                  .index(i)
                                  .pageItemType(BasePageItemType.PAGE)
                                  .build());
            }
        }

        public void last(int pageSize)
        {
            items.Add(BasePageItem.builder()
                              .active(false)
                              .pageItemType(BasePageItemType.DOTS)
                              .build());

            items.Add(BasePageItem.builder()
                              .active(true)
                              .index(pageSize)
                              .pageItemType(BasePageItemType.PAGE)
                              .build());
        }

        public void first(int pageNumber)
        {
            items.Add(BasePageItem.builder()
                              .active(pageNumber != 1)
                              .index(1)
                              .pageItemType(BasePageItemType.PAGE)
                              .build());

            items.Add(BasePageItem.builder()
                              .active(false)
                              .pageItemType(BasePageItemType.DOTS)
                              .build());
        }

        public static BasePaging of(PaginationFilter validFilter, int totalRecords, IUriService uriService, string route)
        {
            BasePaging paging = new BasePaging();

            var totalPages = ((double)totalRecords / (double)validFilter.size);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            paging.NextPage =
                validFilter.pageNumber >= 1 && validFilter.pageNumber < roundedTotalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.pageNumber + 1, validFilter.size, 
                    validFilter.sort, validFilter.columnOrder, validFilter.columnTitle), route)
                : null;
            paging.PreviousPage =
                validFilter.pageNumber - 1 >= 1 && validFilter.pageNumber <= roundedTotalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.pageNumber - 1, validFilter.size, 
                    validFilter.sort, validFilter.columnOrder, validFilter.columnTitle), route)
                : null;
            paging.FirstPage = uriService.GetPageUri(new PaginationFilter(1, validFilter.size, 
                validFilter.sort, validFilter.columnOrder, validFilter.columnTitle), route);
            paging.LastPage = uriService.GetPageUri(new PaginationFilter(roundedTotalPages, validFilter.size, 
                validFilter.sort, validFilter.columnOrder, validFilter.columnTitle), route);
            
            paging.TotalRecords = totalRecords;

            paging.PageSize = roundedTotalPages;
            paging.NextEnabled = validFilter.pageNumber != roundedTotalPages;
            paging.PrevEnabled = validFilter.pageNumber != 1;
            paging.PageNumber = validFilter.pageNumber;

            paging.PageSort = validFilter.sort;
            paging.ColumnOrder = validFilter.columnOrder;
            paging.ColumnTitle = validFilter.columnTitle;


            if (totalPages < PAGINATION_STEP * 2 + 6)
            {
                paging.addPageItems(1, roundedTotalPages + 1, validFilter.pageNumber);

            }
            else if (validFilter.pageNumber < PAGINATION_STEP * 2 + 1)
            {
                paging.addPageItems(1, PAGINATION_STEP * 2 + 4, validFilter.pageNumber);
                paging.last(roundedTotalPages);

            }
            else if (validFilter.pageNumber > roundedTotalPages - PAGINATION_STEP * 2)
            {
                paging.first(validFilter.pageNumber);
                paging.addPageItems(roundedTotalPages - PAGINATION_STEP * 2 - 2, roundedTotalPages + 1, validFilter.pageNumber);

            }
            else
            {
                paging.first(validFilter.pageNumber);
                paging.addPageItems(validFilter.pageNumber - PAGINATION_STEP, validFilter.pageNumber + PAGINATION_STEP + 1, validFilter.pageNumber);
                paging.last(roundedTotalPages);
            }

            return paging;
        }


    }
}
