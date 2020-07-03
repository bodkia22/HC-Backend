using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
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

                await _mailSenderService.SendEmailAsync(userToCreate.Email, "Welcome to Honey Course! Confirm Your Email",
                    $"<p>Welcome to Honey Course! Confirm Your Email <a href='{confirmationLink}'> Click here to confirm.</a> </p>");
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
                user = await  _userManager.FindByNameAsync(userForLogin.UserName);
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
                    UserName = userInfo.FirstName+userInfo.LastName, 
                    DateOfBirth = default(DateTime)
                };

                var createdResult = await _userManager.CreateAsync(identityUser);
                if(!createdResult.Succeeded)
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
    }
}
