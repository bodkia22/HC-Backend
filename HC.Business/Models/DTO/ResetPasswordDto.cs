using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Business.Models.DTO
{
    public class ResetPasswordDto
    {
        public string UserId { get; set; }
        public string ResetToken { get; set; }
        public string NewPassword { get; set; }
    }
}
