using System;
using System.Collections.Generic;
using System.Text;
using HC.Business.Interfaces;

namespace HC.Business.Services
{
    public class UserService : IUserService
    {
        public int GetAge(DateTime dateOfBirth)
        {
            int age = dateOfBirth.Year - DateTime.Now.Year;

            if (dateOfBirth.DayOfYear < DateTime.Now.DayOfYear)
                return age - 1;

            return age;
        }
    }
}
