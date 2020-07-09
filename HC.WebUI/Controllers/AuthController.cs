using System.Threading.Tasks;
using HC.Business.Interfaces;
using HC.Business.Models.DTO;
using HC.Business.Models.VM;
using HC.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HC.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService service)
        {
            _authService = service;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] UserForRegisterDto userForRegister)
        {
            var createdUser = await _authService.Register(userForRegister);

            if (!createdUser.Succeeded)
            {
                return BadRequest(createdUser);
            }

            return Ok($"User '{userForRegister.NickName}' created ");
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginViewModel>> Login([FromBody] UserLoginDto userToLogin)
        {
            var response = await _authService.Login(userToLogin);

            if (response == null)
            {
                return BadRequest("Login failure. Invalid password or username");
            }

            return Ok(response);
        }

        [HttpGet("confirmEmail")]
        public async Task<ActionResult<IdentityResult>> ConfirmEmail(string userId, string token)
        {
            var res = await _authService.ConfirmEmail(userId, token);

            if (!res.Succeeded)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpPost("facebookAuth")]
        public async Task<ActionResult> Login([FromBody] FacebookLoginDto userFacebookLogin)
        {
            var authResponse = await _authService.LoginWithFacebookAsync(userFacebookLogin.AccessToken);


            if (authResponse == null)
            {
                return BadRequest("Login with Facebook was failed.");
            }

            return Ok(authResponse);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> PasswordRecover([FromBody] RecoveryPasswordDataDto data)
        {
            var res = await _authService.SendPasswordRecoveryMessage(data.Data);

            if (!res)
            {
                return BadRequest("User with this Email or User Name does not exist.");
            }

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<IdentityResult>> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var res = await _authService.ResetPassword(resetPasswordDto);

            if (!res.Succeeded)
            {
                return BadRequest(res);
            }

            return Ok(res);
        }
    }
}
