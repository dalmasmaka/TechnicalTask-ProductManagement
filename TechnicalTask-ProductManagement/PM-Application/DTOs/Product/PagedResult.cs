﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_Application.DTOs.Product
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }  
        public int TotalCount { get; set; }        
        public int PageNumber { get; set; }        
        public int PageSize { get; set; }         
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize); 
    }
}
