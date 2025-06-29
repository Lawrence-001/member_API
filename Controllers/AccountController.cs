using MemberTestAPI.Dtos;
using MemberTestAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemberTestAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AccountController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            //if (ModelState.IsValid)
            //{
            //    //return Ok(await _authService.Register(registerDto));
            //    await _authService.Register(registerDto);
            //    return Ok("Registration successful, Please log in.");

            //}
            //return BadRequest(ModelState);
            try
            {
                await _authService.Register(registerDto);
                return Ok(new { message = "Registration successful. Please check your email to confirm your account." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _authService.Login(loginDto));
            }
            return BadRequest(ModelState);
        }

        //[HttpPost("confirm-email")]
        //[AllowAnonymous]
        //public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto dto)
        //{
        //    try
        //    {
        //        await _authService.ConfirmEmail(dto.UserId, dto.Token);
        //        return Ok(new { message = "Email confirmed successfully." });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { error = ex.Message });
        //    }
        //}

        // swagger testing
        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailGet([FromQuery] string userId, [FromQuery] string token)
        {
            try
            {
                await _authService.ConfirmEmail(userId, token);
                return Ok(new { message = "Email confirmed successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
