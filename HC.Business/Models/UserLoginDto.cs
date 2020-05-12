using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using AutoMapper;
using HC.Business.Mapping;
using HC.Data.Entities;

namespace HC.Business.Models
{
    public class UserLoginDto : IMapFrom<User>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserLoginDto, User>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.UserName));
        }
    }
}
