using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HC.Business.Models.VM;
using HC.Data;
using HC.Data.Entities;
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
        private readonly IHCDbContext _context;

        public UsersController(UserManager<User> userManager, IMapper mapper, HCDbContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
        }

        [Authorize]
        [HttpGet("get-authorized")]
        public async Task<ActionResult<UserViewModel>> GetAuthorized()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string role = User.FindFirstValue(ClaimTypes.Role);

            var user = await _userManager.FindByIdAsync(userId);

            var res = _mapper.Map<UserViewModel>(user);

            res.Role = role;

            return Ok(res);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("[action]")]
        public async Task<ActionResult<UserWithFullInfoViewModel>> GetAllUsersWithFullInfo()
        {
            var users = await _userManager.Users.OrderBy(x => x.RegisteredDate)
                .ProjectTo<UserWithFullInfoViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            
            return Ok(users);
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<UserWithFullInfoViewModel>> GetAuthorizedUserWithFullInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);


            if (user == null)
            {
                return BadRequest("User not found!");
            }

            var res = _mapper.Map<UserWithFullInfoViewModel>(user);

            return Ok(res);
        }
    }

}
