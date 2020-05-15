using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HC.Business.Interfaces;
using HC.Business.Models;
using HC.Data;
using HC.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HC.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStudentService _service;
        public StudentController(IHttpContextAccessor httpContextAccessor, IStudentService service)
        {
            _httpContextAccessor = httpContextAccessor;
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "student")]
        public async Task<ActionResult<SubscribeToCourseViewModel>> SubscribeToCourse([FromBody] SubscribeToCourseDto subscribeToCourse)
        {
            int studentId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            subscribeToCourse.StudentId = studentId;

            var res = await _service.SubscribeToCourse(subscribeToCourse);

            return Ok(res);
        }
    }
}
