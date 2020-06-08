using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using HC.Business.Mapping;
using HC.Data;
using HC.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace HC.Business.Models.VM
{
    public class UserWithFullInfoViewModel : IMapFrom<User>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        public string RegisteredDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserWithFullInfoViewModel>()
                .ForMember(x => x.RegisteredDate, opt =>
                    opt.MapFrom(x => x.RegisteredDate.ToString("g")))
                .ForMember(x => x.DateOfBirth, opt =>
                    opt.MapFrom(x => x.DateOfBirth.ToString("d")))
                .ForMember(x => x.NickName, opt => opt.MapFrom(x => x.UserName));
        }
    }
}
