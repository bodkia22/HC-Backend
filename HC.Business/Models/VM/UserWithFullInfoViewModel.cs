using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using HC.Business.Mapping;
using HC.Data;
using HC.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace HC.Business.Models.VM
{
    public class UserWithFullInfoViewModel : IMapFrom<User>
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        public string RegisteredDate { get; set; }
        public string PhoneNumber { get; set; }
        public List<CourseViewModel> Courses { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserWithFullInfoViewModel>()
                .ForMember(x => x.RegisteredDate, opt =>
                    opt.MapFrom(x => x.RegisteredDate.ToString("d")))
                .ForMember(x => x.DateOfBirth, opt =>
                    opt.MapFrom(x => x.DateOfBirth.ToString("d")))
                .ForMember(x => x.NickName, opt => opt.MapFrom(x => x.UserName))
                .ForMember(x => x.FullName, opt => opt.MapFrom(x => $"{x.FirstName} {x.LastName}"))
                .ForMember(x => x.Courses, opt => opt.MapFrom(x => x.CoursesToStudents.Select(y => y.Course)));
        }
    }
}
