using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HC.Business.Mapping;
using HC.Data.Entities;

namespace HC.WebUI.ViewModels.LoginViewModels
{
    public class GetAllCoursesViewModel : IMapFrom<Course>
    {
        public string Name { get; set; }
        public string Info { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
