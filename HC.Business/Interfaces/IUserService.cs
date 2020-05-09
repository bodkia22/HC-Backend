using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Business.Interfaces
{
    public interface IUserService
    {
        public int GetAge(DateTime dateOfBirth);
    }
}
