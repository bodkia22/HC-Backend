using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HC.Business.Interfaces;
using HC.Business.Models;
using HC.Data.Entities;
using HC.WebUI.ViewModels.LoginViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HC.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IAuthService _service;
        public readonly UserManager<User> _manager;
        public readonly IMapper _mapper;

        public AuthController(IAuthService service, UserManager<User> manager, IMapper mapper)
        {
            _service = service;
            _manager = manager;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] UserForRegisterDto userForRegister)
        {
            var createdUser = await _service.Register(userForRegister);
            
            if(!createdUser.Succeeded)
                return BadRequest("User was not created");

            //return Ok($"User {userForRegister.NickName} created ");
            return Ok(await _manager.FindByNameAsync(userForRegister.NickName)); //change in future
        }
    }
}
