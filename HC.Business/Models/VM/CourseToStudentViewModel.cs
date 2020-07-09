using System.Globalization;
using AutoMapper;
using HC.Business.Mapping;
using HC.Data.Entities;

namespace HC.Business.Models.VM
{

    public class CourseToStudentViewModel : IMapFrom<CourseToStudent>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CourseToStudent, CourseToStudentViewModel>()
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Course.Name))
                .ForMember(x => x.StartDate, opt => opt.MapFrom(x => x.StartDate.ToString("d")))
                .ForMember(x => x.EndDate, opt => opt.MapFrom(x => x.EndDate.ToString("d")))
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Course.Id));
        }
    }
}
