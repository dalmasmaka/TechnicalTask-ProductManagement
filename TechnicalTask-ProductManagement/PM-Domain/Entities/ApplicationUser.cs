using Microsoft.AspNetCore.Identity;
using PM_Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public DateTime LoginTimestamp { get; set; }
        public DateTime LogOutTimestamp { get; set; } 
    }
}
