using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PM_Domain.BaseEntities;

namespace PM_Domain.Entities
{
    public class Language : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(500)]
        public string Key { get; set; }
        [Required, MaxLength(200)]
        public string Culture { get; set; }
        [Required, MaxLength(500)]
        public string Value { get; set; } 
    }

}
