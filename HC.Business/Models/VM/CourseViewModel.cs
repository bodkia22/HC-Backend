using AutoMapper;
using HC.Business.Mapping;
using HC.Data.Entities;

namespace HC.Business.Models.VM
{
    public class CourseViewModel : IMapFrom<Course>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string ImgUrl { get; set; }
    }
}
