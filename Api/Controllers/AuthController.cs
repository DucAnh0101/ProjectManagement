using DataAccessLayer.ReqDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Implements;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReqDTO req, [FromServices] JwtTokenGeneratorServices tokenGen)
        {
            try
            {
                var result = await _authService.Login(req);
                var token = tokenGen.GenerateToken(result);
                return Ok(new
                {
                    token = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterReqDTO req)
        {
            try
            {
                var result = await _authService.Register(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] AdminUpdateUserReqDTO req)
        {
            try
            {
                var result = await _authService.AdminUpdateUser(id, req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _authService.GetListUser();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            Console.WriteLine($"User Claims: {System.Text.Json.JsonSerializer.Serialize(claims)}");
            Console.WriteLine($"Is Authenticated: {User.Identity.IsAuthenticated}");
            try
            {
                var result = await _authService.GetUserById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("RsetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassReqDTO req)
        {
            try
            {
                await _authService.ResetPassword(req);
                return Ok();
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(new { Message = argEx.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("user-update/{id}")]
        public async Task<IActionResult> UserUpdate(int id, [FromBody] UserUpdateReqDTO req)
        {
            try
            {
                var result = await _authService.UserUpdate(id, req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}