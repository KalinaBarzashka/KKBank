using System;

namespace KKBank.Web.ViewModels.ViewModels
{
    public class PagingViewModel
    {
        public int PageNumber { get; set; }

        public int TotalItemsCount { get; set; } //AcountsCount

        public int ItemsPerPage { get; set; }

        public int PagesCount => (int)Math.Ceiling((double)TotalItemsCount / ItemsPerPage);

        //computing props

        public bool HasPreviousPage => this.PageNumber > 1;

        public int PreviousPageNumber => this.PageNumber - 1;

        public bool HasNextPage => this.PageNumber < PagesCount;

        public int NextPageNumber => this.PageNumber + 1;
    }
}