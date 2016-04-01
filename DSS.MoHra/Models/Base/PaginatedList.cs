using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSS.MoHra
{
    public class PaginatedList<T> : List<T>
    {
        public int TotalPages { get; }
        public int CurrentPage { get; }

        public PaginatedList(IQueryable<T> source, int? page)
        {
            // initialize
            int itemsOnPage = 20;
            CurrentPage = !page.HasValue ? 1 : page.Value;
            TotalPages = (int)Math.Ceiling((double)source.Count() / itemsOnPage);

            // normalize
            if (source == null)
                source = new List<T>().AsQueryable();
            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;
            else if (CurrentPage < 1)
                CurrentPage = 1;

            // special cases at first!
            if (TotalPages == 1)
            {
                AddRange(source.ToList());
            }
            else if (CurrentPage == 1)
            {
                AddRange(source.Take(itemsOnPage).ToList());
            }
            else
            {
                AddRange(source.Skip(itemsOnPage * (CurrentPage - 1)).Take(itemsOnPage).ToList());
            }
        }

        public System.Web.Mvc.MvcHtmlString GetPagination(Uri currentUrl)
        {
            return new System.Web.Mvc.MvcHtmlString("wat?");
        }
    }
}