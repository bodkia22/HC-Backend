using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HC.Business.Models;
using HC.Business.Models.DTO;
using HC.Business.Models.VM;
using Microsoft.AspNetCore.Identity;

namespace HC.Business.Interfaces
{
    public interface IAuthService
    {
        public Task<IdentityResult> Register(UserForRegisterDto userForRegister);
        public Task<LoginViewModel> Login(UserLoginDto userForLogin);
    }
}
