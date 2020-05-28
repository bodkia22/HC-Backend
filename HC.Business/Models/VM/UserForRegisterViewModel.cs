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
        public DateTime DateOfBirth;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserForRegisterDto, UserForRegisterViewModel>();
        }
    }

}
