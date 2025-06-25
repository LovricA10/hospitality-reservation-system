using AutoMapper;
using Dao.Models;
using Dao.Services;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservationSystemWebApi.Controllers.DTOs;
using RestaurantReservationSystemWebApi.Security;

namespace RestaurantReservationSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        private readonly IMapper _mapper;
        private readonly LogService _logService;

        public UserController(IConfiguration configuration, UserService userService, IMapper mapper, LogService logService)
        {
            _configuration = configuration;
            _userService = userService;
            _mapper = mapper;
            _logService = logService;
        }

        [HttpPost("[action]")]
        public ActionResult<UserDTO> Register([FromBody] UserDTO userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var trimmedEmail = userDto.Email.Trim().ToLowerInvariant();

                if (string.IsNullOrEmpty(trimmedEmail))
                    return BadRequest(new { error = "Email is required." });

                if (_userService.GetByEmail(trimmedEmail) != null)
                    return BadRequest(new { error = $"Email {trimmedEmail} already exists" });

                if (string.IsNullOrWhiteSpace(userDto.Password))
                    return BadRequest(new { error = "Password is required." });

                var b64salt = PasswordHashProvider.GetSalt();
                var b64hash = PasswordHashProvider.GetHash(userDto.Password, b64salt);

                var user = _mapper.Map<User>(userDto);
                user.Email = trimmedEmail;
                user.PwdSalt = b64salt;
                user.PwdHash = b64hash;
                user.Role = string.IsNullOrEmpty(userDto.Role) ? "User" : userDto.Role;

                _userService.Create(user);
                _logService.Log($"New user registered with email: {user.Email}", 1);

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logService.Log($"Error during registration: {ex.Message}", 3); 
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("[action]")]
        public ActionResult Login([FromBody] UserLoginDTO userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var email = userDto.Email?.Trim().ToLowerInvariant();
                var genericLoginFail = "Incorrect email or password";

                var existingUser = _userService.GetByEmail(email);
                if (existingUser == null)
                {
                    _logService.Log($"Failed login attempt for email: {email}", 2);
                    return BadRequest(new { error = genericLoginFail });
                }

                var hash = PasswordHashProvider.GetHash(userDto.Password, existingUser.PwdSalt);
                if (hash != existingUser.PwdHash)
                {
                    _logService.Log($"Invalid password attempt for email: {email}", 2);
                    return BadRequest(new { error = genericLoginFail });
                }

                var secureKey = _configuration["Jwt:SecureKey"];
                var token = JwtTokenProvider.CreateToken(secureKey, 120, existingUser.Email,existingUser.Role);

                _logService.Log($"User logged in: {existingUser.Email}", 1);
                return Ok(token);
            }
            catch (Exception ex)
            {
                _logService.Log($"Error during login: {ex.Message}", 3);
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
