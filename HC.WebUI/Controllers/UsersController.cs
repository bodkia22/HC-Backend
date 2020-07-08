using System.Security.Claims;
using System.Threading.Tasks;
using HC.Business.Interfaces;
using HC.Business.Models;
using HC.Business.Models.DTO;
using HC.Business.Models.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HC.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("get-authorized")]
        public async Task<ActionResult<UserViewModel>> GetAuthorized()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string role = User.FindFirstValue(ClaimTypes.Role);

            var res = await _service.GetAuthorized(userId, role);

            return Ok(res);
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<UserWithFullInfoViewModel>> GetAuthorizedUserWithFullInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var res = await _service.GetAuthorizedUserWithFullInfo(userId);

            if (res == null)
            {
                return BadRequest("User not found!");
            }

            return Ok(res);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("[action]")]
        public async Task<ActionResult<PaginationUsersViewModel>> GetAllUsersWithFullInfo([FromBody] PageInfo pageInfo)
        {
            var result = await _service.GetSortedUsers(pageInfo);

            return Ok(result);
        }

        [Authorize(Roles="admin")]
        [HttpPost("[action]")]
        public async Task<ActionResult<PaginationUsersViewModel>> GetSortedUsers(DataForUsersSortDto data)
        {
            var res = await _service.GetSortedUsers(data);

            return Ok(res);
        }
    }
}