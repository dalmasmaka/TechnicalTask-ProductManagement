using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_Application.DTOs.Product
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Status { get; set; }
        public string CategoryName { get; set; }

        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
