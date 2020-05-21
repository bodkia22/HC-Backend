using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using HC.Business.Interfaces;
using HC.Business.Models;
using HC.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HC.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _mailSenderService;
        private readonly IJwtFactory _jwtFactory;

        public AuthService(UserManager<User> userManager, IMapper mapper, IEmailSenderService mailSenderService, IJwtFactory jwtFactory)
        {
            _userManager = userManager;
            _mapper = mapper;
            _mailSenderService = mailSenderService;
            _jwtFactory = jwtFactory;
        }

        public async Task<IdentityResult> Register(UserForRegisterDto userForRegister)
        {
            var userToCreate = _mapper.Map<User>(userForRegister);

            var userCreated = await _userManager.CreateAsync(userToCreate, userForRegister.Password);

            if (userCreated.Succeeded)
            {
                await _userManager.AddToRoleAsync(userToCreate, "student"); //added all send email

                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(userToCreate);

                string confirmationLink = $"http://localhost:5001/api/auth/confirmemail?userid={userToCreate.Id}&token={confirmationToken}";

                await _mailSenderService.SendEmailAsync(userToCreate.Email, "Welcome to Honey Course! Confirm Your Email",
                    $"<h1>Welcome to Honey Course! Confirm Your Email {confirmationLink} </h1>");
            }
            return userCreated;
        }

        public async Task<LoginViewModel> Login(UserLoginDto userForLogin)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            User user;

            if (regex.IsMatch(userForLogin.UserName))
            {
                user = await _userManager.FindByEmailAsync(userForLogin.UserName);
            }
            else
            {
                user = await  _userManager.FindByNameAsync(userForLogin.UserName);
            }

            if (!await _userManager.CheckPasswordAsync(user, userForLogin.Password) && user == null)
            {
                return null;
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
                Role = role.First()
            };
        }
    }
}
