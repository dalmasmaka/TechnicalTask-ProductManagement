using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_Application.DTOs.User
{
    public class UpdateUserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
