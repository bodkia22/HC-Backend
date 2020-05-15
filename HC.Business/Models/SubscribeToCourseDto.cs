using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using AutoMapper;
using HC.Business.Mapping;
using HC.Data.Entities;

namespace HC.Business.Models
{
    public class SubscribeToCourseDto : IMapFrom<CourseToStudent>
    {
        public int CourseId { get; set; }
        public int StudentId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SubscribeToCourseDto, CourseToStudent>();
        }
    }
}
