﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_Application.DTOs.Product
{
    public class FilterProductDTO
    {
        public string? Name { get; set; }
        public string? Status { get; set; }
        public int? CategoryId { get; set; }
    }
}
