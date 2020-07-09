using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HC.Business.Models;
using HC.Business.Models.DTO;
using HC.Business.Models.VM;

namespace HC.Business.Interfaces
{
    public interface IStudentService
    {
        public Task<SubscribeToCourseViewModel> SubscribeToCourse(SubscribeToCourseDto subscribeToCourse);
    }
}
