using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HC.Business.Models.VM;
using HC.Data.Entities;
using HC.WebUI.ViewModels.LoginViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HC.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UsersController(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("get-authorized")]
        public async Task<ActionResult<UserViewModel>> GetAuthorized()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);

            return Ok(_mapper.Map<UserViewModel>(user));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserWithFullInfoViewModel>> GetAllUsersWithFullInfo()
        {
            var users = await _userManager.Users
                .ProjectTo<UserWithFullInfoViewModel>(_mapper.ConfigurationProvider)
                .OrderBy(x => x.RegisteredDate).ToListAsync();

            return Ok(users);
        }
    }

}
