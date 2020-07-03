using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Business.Models
{
    public class PageInfo
    {
        public int Current { get; set; } 
        public int PageSize { get; set; } 
        public int Total { get; set; } 
        public int TotalPages  // всего страниц
        {
            get { return (int)Math.Ceiling((decimal)Total/ PageSize); }
        }

    }
}
