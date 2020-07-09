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
        public Task<LoginViewModel> LoginWithFacebookAsync(string accessToken);
        public Task<bool> SendPasswordRecoveryMessage(string data);
        public Task<IdentityResult> ChangePasswordByUserId(string userId, string token, string newPassword);
        public Task<IdentityResult> ConfirmEmail(string userId, string token);
        public Task<IdentityResult> ResetPassword(ResetPasswordDto resetPasswordDto);
    }
}
