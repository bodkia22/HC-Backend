using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HC.Business.Interfaces;
using HC.Business.Models;
using HC.Business.Models.DTO;
using HC.Business.Models.VM;
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

            _context.CoursesToStudents.Add(courseToStudent);

            await _context.SaveChangesAsync();
            
            return _mapper.Map<SubscribeToCourseViewModel>(courseToStudent);
        }
    }
}
