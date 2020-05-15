using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HC.Business.Models;

namespace HC.Business.Interfaces
{
    public interface IStudentService
    {
        public Task<SubscribeToCourseViewModel> SubscribeToCourse(SubscribeToCourseDto subscribeToCourse);
    }
}
