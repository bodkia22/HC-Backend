using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HC.Business.Mapping;
using HC.Business.Models;
using HC.Data.Entities;

namespace HC.WebUI.ViewModels.LoginViewModels
{
    public class UserForRegisterViewModel : IMapFrom<UserForRegisterDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public DateTime DateOfBirth;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserForRegisterViewModel, UserForRegisterDto>();
        }
    }

}
