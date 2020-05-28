using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        public  async Task<ActionResult<List<CourseViewModel>>> GetAllCourses()
        {
            //var res = await _context.Courses.ToListAsync();
            var res = await _context.Courses.ProjectTo<CourseViewModel>(_mapper.ConfigurationProvider).ToListAsync();

            return Ok(res);
        }
    }
}
