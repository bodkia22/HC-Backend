using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HC.Business.Interfaces;
using HC.Business.Models;
using HC.Business.Models.DTO;
using HC.Business.Models.VM;
using HC.Data;
using HC.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HC.Business.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(HCDbContext context, UserManager<User> userManager, IMapper mapper, IEmailSenderService mailService)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public IQueryable<User> GetUsersByData(string searchString, IQueryable<User> users, PageInfo pageInfo)
        {
            var res = users
                .Where(x =>
                    x.UserName.ToLower().Contains(searchString) || x.FirstName.ToLower().Contains(searchString) ||
                    x.LastName.ToLower().Contains(searchString) || x.Email.ToLower().Contains(searchString) ||
                    x.PhoneNumber.Contains(searchString));

            return res;
        }

        public IQueryable<User> GetUsersByData(string searchString, PageInfo pageInfo)
        {
            var res = _userManager.Users
                    .Where(x =>
                        x.UserName.ToLower().Contains(searchString) || x.FirstName.ToLower().Contains(searchString) ||
                        x.LastName.ToLower().Contains(searchString) || x.Email.ToLower().Contains(searchString) ||
                        x.PhoneNumber.Contains(searchString));

            pageInfo.Total = res.Count();

            return res;
        }

        public async Task<PaginationUsersViewModel> GetSortedUsers(DataForUsersSortDto data)
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

                return (new PaginationUsersViewModel
                {
                    Users = await res.ProjectTo<UserWithFullInfoViewModel>(_mapper.ConfigurationProvider).ToListAsync(),
                    PageInfo = pageInfo
                });
            }


            if (!string.IsNullOrEmpty(data.SearchString))
            {
                res = GetUsersByData(data.SearchString.ToLower(), pageInfo);
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

            return new PaginationUsersViewModel
            {
                Users = await res.ProjectTo<UserWithFullInfoViewModel>(_mapper.ConfigurationProvider).ToListAsync(),
                PageInfo = pageInfo
            };
        }

        public async Task<PaginationUsersViewModel> GetSortedUsers(PageInfo pageInfo)
        {
            var users = await _userManager.Users.Skip((pageInfo.Current - 1) * pageInfo.PageSize).Take(pageInfo.PageSize).OrderBy(x => x.RegisteredDate)
                .ProjectTo<UserWithFullInfoViewModel>(_mapper.ConfigurationProvider).ToListAsync(); ;

            pageInfo.Total = _userManager.Users.Count();

            return new PaginationUsersViewModel { PageInfo = pageInfo, Users = users };
        }

        public async Task<UserWithFullInfoViewModel> GetAuthorizedUserWithFullInfo(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return _mapper.Map<UserWithFullInfoViewModel>(user);
        }

        public async Task<UserViewModel> GetAuthorized(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var res = _mapper.Map<UserViewModel>(user);

            res.Role = role;

            return res;
        }
    }
}

