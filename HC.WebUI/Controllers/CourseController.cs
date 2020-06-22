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
using Microsoft.AspNetCore.Authorization;
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
        public CourseController(HCDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
        public async Task<ActionResult<CourseToStudentViewModel>> GetCoursesByStudentId(int userId)
        {
            var res = await _context.CoursesToStudents
                .Where(x => x.StudentId == userId).ProjectTo<CourseToStudentViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            
            return Ok(res);
        }
    }
}
