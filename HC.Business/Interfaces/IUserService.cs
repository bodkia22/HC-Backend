using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HC.Business.Models;
using HC.Business.Models.DTO;
using HC.Business.Models.VM;
using HC.Data.Entities;

namespace HC.Business.Interfaces
{
    public interface IUserService
    {
        public IQueryable<User> GetUsersByData(string data, IQueryable<User> users, PageInfo pageInfo);
        public IQueryable<User> GetUsersByData(string searchString, PageInfo pageInfo);
        public Task<PaginationUsersViewModel> GetSortedUsers(DataForUsersSortDto data);
        public Task<PaginationUsersViewModel> GetSortedUsers(PageInfo pageInfo);
        public Task<UserWithFullInfoViewModel> GetAuthorizedUserWithFullInfo(string userId);
        public Task<UserViewModel> GetAuthorized(string userId, string role);
    }
}
