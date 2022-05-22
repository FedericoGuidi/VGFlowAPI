using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI.Application.Models
{
    public class LoginPayload
    {
        public string AuthorizationCode { get; set; }
        public string AppleID { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
    }
}
