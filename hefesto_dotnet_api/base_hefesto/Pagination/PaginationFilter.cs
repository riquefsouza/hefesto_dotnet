using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hefesto.base_hefesto.Pagination
{
    public class PaginationFilter
    {
        public int pageNumber { get; set; }
        public int size { get; set; }
        public string sort { get; set; }
        public int columnOrder { get; set; }
        public string columnTitle { get; set; }

        public PaginationFilter()
        {
            this.pageNumber = 1;
            this.size = 10;
            this.sort = "ASC,id";
            this.columnOrder = 0;
            this.columnTitle = "id";
        }

        public PaginationFilter(int pageNumber, int size, string sort, int columnOrder, string columnTitle)
        {
            this.pageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.size = size > 10 ? 10 : size;            
            this.sort = sort.Trim().Length == 0 ? "ASC, id" : sort;
            this.columnOrder = columnOrder < 0 ? 0 : columnOrder;
            this.columnTitle = columnTitle.Trim().Length == 0 ? "id" : columnTitle;
        }
    }
}
