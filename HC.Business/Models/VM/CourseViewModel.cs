using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HC.Business.Mapping;
using HC.Data.Entities;

namespace HC.WebUI.ViewModels.LoginViewModels
{
    public class CourseViewModel : IMapFrom<Course>
    {
        public string Name { get; set; }
        public string Info { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Course, CourseViewModel>()
                .ForMember(x => x.StartDate, opt => opt.MapFrom(x => x.StartDate.ToString("d")))
                .ForMember(x => x.EndDate, opt => opt.MapFrom(x => x.EndDate.ToString("")));
        }
    }
}
