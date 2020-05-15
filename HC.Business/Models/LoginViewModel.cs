using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Business.Models
{
    public class LoginViewModel
    {
        public string JwtToken { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}
