using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hefesto.base_hefesto.Pagination
{
    public class BasePaged<T>
	{
		public List<T> Page { get; set; }

		public BasePaging Paging { get; set; }

		public BasePaged()
		{
		}

		public BasePaged(List<T> page, BasePaging paging)
		{
            try
            {
                string[] paramSort = paging.PageSort.Split(",", 2);
                string sortFieldName = "";

                if (paramSort.Length > 0)
                {
                    TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                    sortFieldName = myTI.ToTitleCase(paramSort[1].ToLower());

                    if (paramSort[0].ToUpper().Equals("ASC"))
                    {
                        page = page.OrderBy(s => s.GetType().GetProperty(sortFieldName).GetValue(s)).ToList();
                    }
                    else
                    {
                        page = page.OrderByDescending(s => s.GetType().GetProperty(sortFieldName).GetValue(s)).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Sort BasePaged: " + e.Message);
            }

            this.Page = page;
            this.Paging = paging;
        }
    }
}
