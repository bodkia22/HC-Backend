using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace HC.Data.Entities
{
    public class User : IdentityUser<int>
    {
        private string Name { get; set; }

        public Admin Admin { get; set; }
        public Student Student { get; set; }
    }
}
