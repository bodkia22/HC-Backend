using AutoMapper;
using HC.Business.Mapping;
using HC.Data.Entities;

namespace HC.Business.Models.VM
{
    public class UserViewModel : IMapFrom<User>
    {
        public string NickName { get; set; }
        public string FullName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserViewModel>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(x => $"{x.FirstName} {x.LastName}"))
                .ForMember(x => x.NickName, opt => opt.MapFrom(x => x.UserName));
        }
    }
}