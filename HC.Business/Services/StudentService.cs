﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HC.Business.Interfaces;
using HC.Business.Models;
using HC.Data;
using HC.Data.Entities;

namespace HC.Business.Services
{
    public class StudentService : IStudentService
    {
        private readonly IHCDbContext _context;
        private readonly IMapper _mapper;

        public StudentService(HCDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SubscribeToCourseViewModel> SubscribeToCourse(SubscribeToCourseDto subscribeToCourse)
        {
            var courseToStudent = _mapper.Map<CourseToStudent>(subscribeToCourse);
            //var courseToStudent = new CourseToStudent
            //{
            //    CourseId = subscribeToCourse.CourseId,
            //    StudentId = subscribeToCourse.StudentId
            //};

            _context.CoursesToStudents.Add(courseToStudent);

            await _context.SaveChangesAsync();
            
            return _mapper.Map<SubscribeToCourseViewModel>(courseToStudent);
        }
    }
}