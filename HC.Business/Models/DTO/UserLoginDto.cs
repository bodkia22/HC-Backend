using HC.Business.Mapping;
using HC.Data.Entities;

namespace HC.Business.Models.DTO
{
    public class UserLoginDto : IMapFrom<User>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
