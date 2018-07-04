using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyUI.Models
{
    public class PageingModel
    {
        public static int PageSize = 18;
    }

    public class PageingModel<T>
    {
        public PageingModel()
        {
            this.PageIndex = 1;
            this.PageSize = PageingModel.PageSize;
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalPage { get; set; }

        public int Total { get; set; }

        public List<T> Data { get; set; }
    }
}