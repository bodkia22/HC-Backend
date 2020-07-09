using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using HC.Business.Interfaces;
using HC.Business.Models.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HC.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _service;
        public CourseController(ICourseService service)
        {
            _service = service;
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "student")]
        public async Task<ActionResult<List<CourseViewModel>>> GetAllCourses()
        {
            var res = await _service.GetAllCourses();

            return Ok(res);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "student")]
        public async Task<ActionResult<CourseViewModel>> GetById(int courseId)
        {
            var res = await _service.GetCourseById(courseId);

            return Ok(res);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> GetIsUserSubscribedToTheCourse(int courseId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var res = await _service.GetIsUserSubscribedToTheCourse(courseId, userId);

            if(!res)
            {
                return BadRequest();
            }

            return Ok("User is subscribed to this course");
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<ActionResult<List<CourseToStudentViewModel>>> GetCoursesByStudentId(int userId)
        {
            var res = await _service.GetCoursesByStudentId(userId);
            
            return Ok(res);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<ActionResult<List<CourseToStudentViewModel>>> GetCoursesByStudentEmail(string email)
        {
            var courses = await _service.GetCoursesByStudentEmail(email);

            if (courses != null)
            {
                return Ok(courses);
            }

            return BadRequest();
        }
    }
}
