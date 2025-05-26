using Dao.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers.DTOs;
using WebApp.Security;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HospitalityReservationDbContext _context;


        public UserController(IConfiguration configuration, HospitalityReservationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }


        [HttpPost("[action]")]
        public ActionResult<UserDTO> Register([FromBody] UserDTO userDto)
        {

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            try
            {
               
                var trimmedEmail = userDto.Email.Trim().ToLowerInvariant();

                if (string.IsNullOrEmpty(trimmedEmail))
                    return BadRequest(new { error = "Email is required." });

                if (_context.Users.Any(x => x.Email.Equals(trimmedEmail)))
                    return BadRequest(new { error = $"Email {trimmedEmail} already exists" });

                if (string.IsNullOrWhiteSpace(userDto.Password))
                    return BadRequest(new { error = "Password is required." });

                var b64salt = PasswordHashProvider.GetSalt();
                var b64hash = PasswordHashProvider.GetHash(userDto.Password, b64salt);

                var user = new User
                {
                    Name = userDto.Name,
                    LastName = userDto.LastName,
                    Email = trimmedEmail,
                    Phone = userDto.Phone,
                    Role = string.IsNullOrEmpty(userDto.Role) ? "User" : userDto.Role,
                    PwdSalt = b64salt,
                    PwdHash = b64hash
                };

               
                _context.Users.Add(user);
                _context.SaveChanges();

              
 

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult Login([FromBody] UserLoginDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var email = userDto.Email?.Trim().ToLowerInvariant();
                var genericLoginFail = "Incorrect email or password";

                var existingUser = _context.Users.FirstOrDefault(x => x.Email == email);
                if (existingUser == null)
                    return BadRequest(new { error = genericLoginFail });


                var b64hash = PasswordHashProvider.GetHash(userDto.Password, existingUser.PwdSalt);
                if (b64hash != existingUser.PwdHash)
                    return BadRequest(new { error = genericLoginFail });


                var secureKey = _configuration["JWT:SecureKey"];


                var serializedToken = JwtTokenProvider.CreateToken(secureKey, 120, existingUser.Email);

                return Ok(serializedToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}
