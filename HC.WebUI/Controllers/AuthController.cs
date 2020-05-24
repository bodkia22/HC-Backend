using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HC.Business.Interfaces;
using HC.Business.Models;
using HC.Data.Entities;
using HC.WebUI.ViewModels.LoginViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using NLog;

namespace HC.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly UserManager<User> UserManager;
        private readonly IMapper _mapper;
        private static Logger logger = LogManager.GetLogger("HCLoggerRule");

        public AuthController(IAuthService service, UserManager<User> manager, IMapper mapper)
        {
            authService = service;
            UserManager = manager;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] UserForRegisterDto userForRegister)
        {
            var createdUser = await authService.Register(userForRegister);

            if (!createdUser.Succeeded)
            {
                return BadRequest(createdUser);
            }

            return Ok($"User {userForRegister.NickName} created ");
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginViewModel>> Login([FromBody] UserLoginDto userToLogin)
        {
            var response = await authService.Login(userToLogin);

            if (response == null)
            {
                return BadRequest("Login failure. Invalid password or username");
            }

            return Ok(response);
        }

        [HttpGet("confirmEmail")]
        public async Task<IdentityResult> ConfirmEmail(string userId, string token)
        {
            var userToConfirmEmail = await UserManager.FindByIdAsync(userId);

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await UserManager.ConfirmEmailAsync(userToConfirmEmail, token);
            return result;
        }
    }
}
