using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_Application.DTOs.User
{
    public class UserDTO
    {
        public string FullName { get; set; }
        public DateTime LoginTimestamp { get; set; }
        public DateTime LogOutTimestamp { get; set; }
    }   
}
