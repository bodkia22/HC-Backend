using System;
using System.Collections.Generic;
using System.Linq;
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

        //[HttpGet("[action]")]
        //[Authorize(Roles = "student")]
        //public async Task<ActionResult<CourseViewModel>> GetCourseByDate([FromBody] CourseByDateDto courseByDateDto)
        //{
        //    var res = await _context.Courses.Where(x => x.StartDate >= courseByDateDto.DateOfCourse)
        //        .OrderBy(x => Convert.ToDateTime(x.StartDate))
        //        .ProjectTo<CourseViewModel>(_mapper.ConfigurationProvider)
        //        .ToListAsync();

        //    return Ok(res);
        //}
    }
}
