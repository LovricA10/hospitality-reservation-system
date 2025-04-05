using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Controllers.DTOs;
using WebApp.Models;
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
        public ActionResult<UserDTO> Register(UserDTO userDto)
        {

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Trim email
                var trimmedEmail = userDto.Email.Trim();

                // Provjeri postoji li korisnik s tim emailom
                if (_context.Users.Any(x => x.Email.Equals(trimmedEmail)))
                    return BadRequest($"Email {trimmedEmail} already exists");

                // Hashiranje lozinke
                var b64salt = PasswordHashProvider.GetSalt();
                var b64hash = PasswordHashProvider.GetHash(userDto.Password, b64salt);

                // Kreiranje korisnika iz DTO + hashiranih vrijednosti
                var user = new User
                {
                    Name = userDto.Name,
                    LastName = userDto.LastName,
                    Email = trimmedEmail,
                    Phone = userDto.Phone,
                    Role = userDto.Role,
                    PwdSalt = b64salt,
                    PwdHash = b64hash
                };

                // Dodavanje u bazu i spremanje
                _context.Users.Add(user);
                _context.SaveChanges();

                // Obavezno obriši lozinku prije vraćanja DTO-a
                userDto.Password = null;

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult Login(UserLoginDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var genericLoginFail = "Incorrect email or password";
                var existingUser = _context.Users.FirstOrDefault(x => x.Email == userDto.Email);
                if (existingUser == null)
                    return BadRequest(genericLoginFail);

            
                var b64hash = PasswordHashProvider.GetHash(userDto.Password, existingUser.PwdSalt);
                if (b64hash != existingUser.PwdHash)
                    return BadRequest(genericLoginFail);

                
                var secureKey = _configuration["JWT:SecureKey"];
                var serializedToken = JwtTokenProvider.CreateToken(secureKey, 120, existingUser.Email);

                return Ok(serializedToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("[action]")]
        public ActionResult GetToken()
        {
            try
            {
                
                var secureKey = _configuration["Jwt:SecureKey"];
                var token = JwtTokenProvider.CreateToken(secureKey, 10);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
