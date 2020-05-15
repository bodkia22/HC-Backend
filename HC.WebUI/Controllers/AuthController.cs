using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HC.Business.Interfaces;
using HC.Business.Models;
using HC.Data.Entities;
using HC.WebUI.ViewModels.LoginViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
                return BadRequest("User was not created");
            }

            return Ok($"User {userForRegister.NickName} created ");
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginDto userToLogin)
        {
            try
            {
                var response = await authService.Login(userToLogin);
                return Ok(response);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return BadRequest("Invalid password ");
            }
        }
    }
}
