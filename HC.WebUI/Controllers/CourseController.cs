using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HC.Business.Models.DTO;
using HC.Business.Models.VM;
using HC.Data;
using HC.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HC.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        readonly IHCDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public CourseController(HCDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "student")]
        public async Task<ActionResult<List<CourseViewModel>>> GetAllCourses()
        {
            var res = await _context.Courses.OrderByDescending(x => x.Id)
                .ProjectTo<CourseViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(res);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "student")]
        public async Task<ActionResult<CourseViewModel>> GetById(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);

            return _mapper.Map<CourseViewModel>(course);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> GetIsUserSubscribedToTheCourse(int courseId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (await _context.CoursesToStudents.FirstOrDefaultAsync(x => x.CourseId == courseId && x.StudentId == userId) !=
                null)
            {
                return Ok("User is subscribed to this course"); 
            }

            return BadRequest();
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<ActionResult<List<CourseToStudentViewModel>>> GetCoursesByStudentId(int userId)
        {
            var res = await _context.CoursesToStudents
                .Where(x => x.StudentId == userId).OrderBy(x => x.StartDate).ProjectTo<CourseToStudentViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            
            return Ok(res);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<ActionResult<CourseToStudentViewModel>> GetCoursesByStudentEmail(string email)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                List<CourseToStudentViewModel> res = await _context.CoursesToStudents
                    .Where(x => x.StudentId == user.Id).ProjectTo<CourseToStudentViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                if (res.Count != 0)
                {
                    return Ok(res);
                }
            }
            return BadRequest();
        }
    }
}
