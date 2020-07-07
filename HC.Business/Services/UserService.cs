using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HC.Business.Interfaces;
using HC.Business.Models;
using HC.Business.Models.VM;
using HC.Data;
using HC.Data.Entities;
using Microsoft.AspNetCore.Identity;
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
            var res =  users
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
    }
}
