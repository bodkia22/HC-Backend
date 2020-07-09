using System;
using AutoMapper;
using HC.Business.Mapping;
using HC.Business.Models.DTO;

namespace HC.Business.Models.VM
{
    public class UserForRegisterViewModel : IMapFrom<UserForRegisterDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string DateOfBirth { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserForRegisterDto, UserForRegisterViewModel>()
                .ForMember(x => x.DateOfBirth, opt => opt.MapFrom(x => x.DateOfBirth.ToString("d")));
        }
    }

}
