using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HC.Business.Interfaces;
using HC.Business.Models;
using HC.Business.Models.DTO;
using HC.Business.Models.VM;
using HC.Data;
using HC.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HC.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IHCDbContext _context;
        private readonly IUserService _service;

        public UsersController(UserManager<User> userManager, IMapper mapper, HCDbContext context, IUserService service)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
            _service = service;
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

        [Authorize(Roles = "admin")]
        [HttpPost("[action]")]
        public async Task<ActionResult<PaginationUsersViewModel>> GetAllUsersWithFullInfo([FromBody]PageInfo pageInfo)
        {
            var users = await _userManager.Users.Skip((pageInfo.Current - 1) * pageInfo.PageSize).Take(pageInfo.PageSize).OrderBy(x => x.RegisteredDate)
                .ProjectTo<UserWithFullInfoViewModel>(_mapper.ConfigurationProvider).ToListAsync(); ;

            pageInfo.Total = _userManager.Users.Count();

            PaginationUsersViewModel result = new PaginationUsersViewModel {PageInfo = pageInfo, Users = users};

            return Ok(result);
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<ActionResult<PaginationUsersViewModel>> GetSortedUsers(DataForUsersSortsDto data)
        {
            IQueryable<User> res = _userManager.Users;


            var pageInfo = new PageInfo
            {
                Current = data.Current,
                PageSize = data.PageSize
            };

            if (pageInfo.Current == 0) //clear
            {
                pageInfo.Total = res.Count();
                pageInfo.Current = 1;

                res = res.Skip((pageInfo.Current - 1) * pageInfo.PageSize).Take(pageInfo.PageSize);

                return Ok(new PaginationUsersViewModel
                {
                    Users = await res.ProjectTo<UserWithFullInfoViewModel>(_mapper.ConfigurationProvider).ToListAsync(),
                    PageInfo = pageInfo
                });
            }

            if (!string.IsNullOrEmpty(data.SearchString))
            {
                res = _service.GetUsersByData(data.SearchString.ToLower(), pageInfo);
            }
            else
            {
                pageInfo.Total = _userManager.Users.Count();
            }

            switch ($"{data.Field} {data.Order}")
            {
                case "id ascend":
                    res = res.OrderBy(x => x.Id).Skip((pageInfo.Current - 1) * pageInfo.PageSize).Take(pageInfo.PageSize);
                    break;
                case "id descend":
                    res = res.OrderByDescending(x => x.Id).Skip((pageInfo.Current - 1) * pageInfo.PageSize).Take(pageInfo.PageSize);
                    break;
                case "nickName ascend":
                    res = res.OrderBy(x => x.UserName).Skip((pageInfo.Current - 1) * pageInfo.PageSize).Take(pageInfo.PageSize);
                    break;
                case "nickName descend":
                    res = res.OrderByDescending(x => x.UserName).Skip((pageInfo.Current - 1) * pageInfo.PageSize).Take(pageInfo.PageSize);
                    break;
                case "email ascend":
                    res = res.OrderBy(x => x.Email).Skip((pageInfo.Current - 1) * pageInfo.PageSize).Take(pageInfo.PageSize);
                    break;
                case "email descend":
                    res = res.OrderByDescending(x => x.Email).Skip((pageInfo.Current - 1) * pageInfo.PageSize).Take(pageInfo.PageSize);
                    break;
                case "fullName ascend":
                    res = res.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).Skip((pageInfo.Current - 1) * pageInfo.PageSize).Take(pageInfo.PageSize);
                    break;
                case "fullName descend":
                    res = res.OrderByDescending(x => x.FirstName).ThenBy(x => x.LastName).Skip((pageInfo.Current - 1) * pageInfo.PageSize).Take(pageInfo.PageSize);
                    break;
                case "dateOfBirth ascend":
                    res = res.OrderBy(x => x.DateOfBirth).Skip((pageInfo.Current - 1) * pageInfo.PageSize).Take(pageInfo.PageSize);
                    break;
                case "dateOfBirth descend":
                    res = res.OrderByDescending(x => x.DateOfBirth).Skip((pageInfo.Current - 1) * pageInfo.PageSize).Take(pageInfo.PageSize);
                    break;
                case "registeredDate ascend":
                    res = res.OrderBy(x => x.RegisteredDate).Skip((pageInfo.Current - 1) * pageInfo.PageSize).Take(pageInfo.PageSize);
                    break;
                case "registeredDate descend":
                    res = res.OrderByDescending(x => x.RegisteredDate).Skip((pageInfo.Current - 1) * pageInfo.PageSize).Take(pageInfo.PageSize);
                    break;
                default:
                    res = res.Skip((pageInfo.Current - 1) * pageInfo.PageSize).Take(pageInfo.PageSize);
                    break;
            }

            return Ok(new PaginationUsersViewModel
            {
                Users = await res.ProjectTo<UserWithFullInfoViewModel>(_mapper.ConfigurationProvider).ToListAsync(),
                PageInfo = pageInfo
            });
        }
    }
}
