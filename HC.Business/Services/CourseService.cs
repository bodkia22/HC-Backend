using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HC.Business.Interfaces;
using HC.Business.Models.VM;
using HC.Data;
using HC.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HC.Business.Services
{
    public class CourseService : ICourseService
    {
        private readonly IHCDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public CourseService(HCDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<List<CourseToStudentViewModel>> GetCoursesByStudentId(int userId)
        {
            var res = await _context.CoursesToStudents
                .Where(x => x.StudentId == userId).OrderBy(x => x.StartDate)
                .ProjectTo<CourseToStudentViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return res;
        }

        public async Task<List<CourseToStudentViewModel>> GetCoursesByStudentEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                List<CourseToStudentViewModel> res = await _context.CoursesToStudents
                    .Where(x => x.StudentId == user.Id).ProjectTo<CourseToStudentViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                if (res.Count != 0)
                {
                    return res;
                }
            }
            return null;
        }

        public async Task<bool> GetIsUserSubscribedToTheCourse(int courseId, int userId)
        {
            return await _context.CoursesToStudents.FirstOrDefaultAsync(x => x.CourseId == courseId && x.StudentId == userId) !=
                   null;
        }

        public async Task<List<CourseViewModel>> GetAllCourses()
        {
            return await _context.Courses.OrderByDescending(x => x.Id)
                .ProjectTo<CourseViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<CourseViewModel> GetCourseById(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);

            return _mapper.Map<CourseViewModel>(course);
        }
    }
}
