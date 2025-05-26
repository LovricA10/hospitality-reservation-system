using Dao.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Controllers.DTOs;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalityVenueController : ControllerBase
    {
        private readonly HospitalityReservationDbContext _context;

        public HospitalityVenueController(HospitalityReservationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: api/HospitalityVenue
        [HttpGet]
        public ActionResult<IEnumerable<HospitalityVenueResponseDTO>> GetAllVenues([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var venues = _context.HospitalityVenues
                    .Include(v => v.Type) 
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(v => new HospitalityVenueResponseDTO
                    {
                        Idvenue = v.Idvenue,
                        VenueName = v.VenueName,
                        Address = v.Address,
                        TypeId = v.TypeId,
                        TypeName = v.Type.TypeName
                    })
                    .ToList();

                return Ok(venues);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/HospitalityVenue/{id}
        [HttpGet("{id}")]
        public ActionResult<HospitalityVenueResponseDTO> GetVenueById(int id)
        {
            try
            {
                var venue = _context.HospitalityVenues
                    .Include(v => v.Type) 
                    .FirstOrDefault(v => v.Idvenue == id);

                if (venue == null)
                {
                    return NotFound();
                }

                var responseDto = new HospitalityVenueResponseDTO
                {
                    Idvenue = venue.Idvenue,
                    VenueName = venue.VenueName,
                    Address = venue.Address,
                    TypeId = venue.TypeId,
                    TypeName = venue.Type?.TypeName
                };

                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/HospitalityVenue
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<HospitalityVenueResponseDTO> CreateVenue([FromBody] HospitalityVenueCreateDTO venueDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                if (venueDto == null)
                {
                    return BadRequest("Invalid data.");
                }

                // Provjera postoji li tip u HospitalityTypes
                var hospitalityType = _context.HospitalityTypes
                    .FirstOrDefault(h => h.Idtype == venueDto.TypeId);

                if (hospitalityType == null)
                {
                    return BadRequest("Invalid HospitalityType.");
                }

                var venue = new HospitalityVenue
                {
                    VenueName = venueDto.VenueName,
                    Address = venueDto.Address,
                    TypeId = venueDto.TypeId // Spremamo povezani tip
                };

                _context.HospitalityVenues.Add(venue);
                _context.SaveChanges();

                var responseDto = new HospitalityVenueResponseDTO
                {
                    Idvenue = venue.Idvenue,
                    VenueName = venue.VenueName,
                    Address = venue.Address,
                    TypeId = venue.TypeId,
                    TypeName = hospitalityType.TypeName
                };

                return CreatedAtAction(nameof(GetVenueById), new { id = venue.Idvenue }, responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/HospitalityVenue/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateVenue(int id, [FromBody] HospitalityVenueUpdateDTO venueDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var venue = _context.HospitalityVenues.FirstOrDefault(v => v.Idvenue == id);
                if (venue == null)
                {
                    return NotFound();
                }

                
                var hospitalityType = _context.HospitalityTypes
                    .FirstOrDefault(h => h.Idtype == venueDto.TypeId);

                if (hospitalityType == null)
                {
                    return BadRequest("Invalid HospitalityType.");
                }

                venue.VenueName = venueDto.VenueName ?? venue.VenueName;
                venue.Address = venueDto.Address ?? venue.Address;
                venue.TypeId = venueDto.TypeId ?? venue.TypeId;

                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/HospitalityVenue/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteVenue(int id)
        {
            try
            {
                var venue = _context.HospitalityVenues.Find(id);
                if (venue == null)
                {
                    return NotFound("Venue not found.");
                }

                _context.HospitalityVenues.Remove(venue);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
