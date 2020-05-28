using System;
using AutoMapper;
using HC.Business.Mapping;
using HC.Data.Entities;

namespace HC.Business.Models.DTO
{
    public class UserForRegisterDto : IMapFrom<User>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserForRegisterDto, User>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.NickName));
        }
    }
}
