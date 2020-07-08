using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using HC.Business.Interfaces;
using HC.Business.Models;
using HC.Business.Models.DTO;
using HC.Business.Models.VM;
using HC.Data;
using HC.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace HC.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _mailSenderService;
        private readonly IJwtFactory _jwtFactory;
        private readonly IFacebookAuthService _facebookAuthService;

        public AuthService(UserManager<User> userManager, IMapper mapper, IEmailSenderService mailSenderService, IJwtFactory jwtFactory, IFacebookAuthService facebookAuthService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _mailSenderService = mailSenderService;
            _jwtFactory = jwtFactory;
            _facebookAuthService = facebookAuthService;
        }

        public async Task<IdentityResult> Register(UserForRegisterDto userForRegister)
        {
            var userToCreate = _mapper.Map<User>(userForRegister);

            var userCreated = await _userManager.CreateAsync(userToCreate, userForRegister.Password);

            if (userCreated.Succeeded)
            {
                await _userManager.AddToRoleAsync(userToCreate, "student"); //added all send email

                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(userToCreate);

                confirmationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationToken));

                string confirmationLink = $"http://localhost:3000/confirmation/{userToCreate.Id}/{confirmationToken}";

                await _mailSenderService.SendEmailAsync(userToCreate.Email,
                    "Welcome to Honey Course! Confirm Your Email",
                    $"<h1>Hello, {userToCreate.UserName}</h1>"+
                    $"<h2>Welcome to Honey Course!<br>Confirm Your Email <a href='{confirmationLink}'>link</a> and start learning!</h2>"+
                    "<h2>Have a nice day.</h2>");
            }
            return userCreated;
        }

        public async Task<LoginViewModel> Login(UserLoginDto userForLogin)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            User user;

            if (regex.IsMatch(userForLogin.UserName))
            {
                user = await _userManager.FindByEmailAsync(userForLogin.UserName);
            }
            else
            {
                user = await _userManager.FindByNameAsync(userForLogin.UserName);
            }

            if (!await _userManager.CheckPasswordAsync(user, userForLogin.Password) || user == null)
            {
                return null;
            }

            var emailConfirmationStatus = await _userManager.IsEmailConfirmedAsync(user);
            if (!emailConfirmationStatus)
            {
                return new LoginViewModel
                {
                    IsEmailConfirmed = false
                };
            }

            var role = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, role.First()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var token = _jwtFactory.GenerateEncodedToken(claims);

            return new LoginViewModel
            {
                JwtToken = token,
                UserName = user.UserName,
                Role = role.First(),
                IsEmailConfirmed = true,
            };
        }

        public async Task<LoginViewModel> LoginWithFacebookAsync(string accessToken)
        {
            var validatedTokenResult = await _facebookAuthService.ValidateAccessTokenAsync(accessToken);

            if (!validatedTokenResult.Data.IsValid)
            {
                return null;
            }

            var userInfo = await _facebookAuthService.GetUserInfoAsync(accessToken);

            var identityUser = await _userManager.FindByEmailAsync(userInfo.Email);

            if (identityUser == null)
            {
                identityUser = new User
                {
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    Email = userInfo.Email,
                    UserName = userInfo.FirstName + userInfo.LastName,
                    DateOfBirth = default(DateTime)
                };

                var createdResult = await _userManager.CreateAsync(identityUser);
                if (!createdResult.Succeeded)
                {
                    return null;
                }
                await _userManager.AddToRoleAsync(identityUser, "student");
            }

            var role = await _userManager.GetRolesAsync(identityUser);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, identityUser.Id.ToString()),
                new Claim(ClaimTypes.Role, role.First()),
                new Claim(ClaimTypes.Name, identityUser.UserName),
            };

            var token = _jwtFactory.GenerateEncodedToken(claims);

            return new LoginViewModel
            {
                IsEmailConfirmed = true,
                JwtToken = token,
                Role = role.First(),
                UserName = identityUser.UserName
            };
        }

        public async Task<bool> SendPasswordRecoveryMessage(string data)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.UserName.Equals(data) || x.Email.Equals(data));

            if (user != null)
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                resetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));

                string confirmationLink = $"http://localhost:3000/password/recover/{user.Id}/{resetToken}";

                await _mailSenderService.SendEmailAsync(user.Email,
                        "Changing password Honey Course !",
                        $"<h1>Hello, {user.UserName}!</h1>" +
                    $"<h2>We've just received your password reset request.<br>So you can click <a href='{confirmationLink}'>here</a> and come up with new password.</h2>" +
                    "<h3>If you don't know where this message from just ignore it.</h3>" +
                    "<h3>Have a nice day.</h3>");

                return true;
            }

            return false;
        }

        public async Task<IdentityResult> ChangePasswordByUserId(string userId, string token, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task<IdentityResult> ConfirmEmail(string userId, string token)
        {
            var userToConfirmEmail = await _userManager.FindByIdAsync(userId);
            if (userToConfirmEmail == null)
            {
                return IdentityResult.Failed();
            }

            var regex = new Regex("^[A-Za-z0-9_-]+$");

            if (regex.IsMatch(token))
            {
                token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            }
            else
            {
                return IdentityResult.Failed();
            }

            return await _userManager.ConfirmEmailAsync(userToConfirmEmail, token);
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var regex = new Regex("^[A-Za-z0-9_-]+$");

            if (regex.IsMatch(resetPasswordDto.ResetToken))
            {
                resetPasswordDto.ResetToken =
                    Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordDto.ResetToken));
            }
            else
            {
                return IdentityResult.Failed();
            }

            var res = await ChangePasswordByUserId(resetPasswordDto.UserId, resetPasswordDto.ResetToken, resetPasswordDto.NewPassword);

            return res;
        }
    }
}
