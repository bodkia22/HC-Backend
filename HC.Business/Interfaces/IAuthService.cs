using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HC.Business.Models;
using Microsoft.AspNetCore.Identity;

namespace HC.Business.Interfaces
{
    public interface IAuthService
    {
        public Task<IdentityResult> Register(UserForRegisterDto userForRegister);
    }
}
