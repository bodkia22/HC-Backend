using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HC.Data;
using Microsoft.AspNetCore.Mvc;

namespace HC.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        readonly HCDbContext _context;
        public TestController(HCDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetName()
        {
            return Ok(new {Name = "Bodkia"});
        }

        [HttpGet("[action]")]
        public IActionResult GetAllCourses()
        {
            var res = _context.Courses.ToList();
            return Ok(res);
        }
    }
}
