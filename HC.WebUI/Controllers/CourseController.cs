using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HC.Business.Models.DTO;
using HC.Data;
using HC.WebUI.ViewModels.LoginViewModels;
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

        [HttpGet]
        [Authorize(Roles = "student")]
        public async Task<ActionResult<List<CourseViewModel>>> GetAllCourses()
        {
            var res = await _context.Courses.ProjectTo<CourseViewModel>(_mapper.ConfigurationProvider)
                .OrderBy(x => x.StartDate).ToListAsync();

            return Ok(res);
        }

        [HttpGet]
        [Authorize(Roles = "student")]
        public async Task<ActionResult<CourseViewModel>> GetCourseByDate([FromBody] CourseByDateDto courseByDateDto)
        {
            var res = await _context.Courses.Where(x => x.StartDate >= courseByDateDto.DateOfCourse)
                .ProjectTo<CourseViewModel>(_mapper.ConfigurationProvider).OrderBy(x => x.StartDate).ToListAsync();

            return Ok(res);
        }
    }
}
