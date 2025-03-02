using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_Domain.BaseEntities
{
    public abstract class BaseEntity
    {
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [Required, MaxLength(100)]
        public string CreatedBy { get; set; }
        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
        public bool? isDeleted { get; set; }

    }
}
