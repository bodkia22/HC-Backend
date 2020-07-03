using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Business.Models.VM
{
    public class PaginationUsersViewModel
    {
        public List<UserWithFullInfoViewModel> Users { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
