using Dao.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Controllers.DTOs;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly HospitalityReservationDbContext? _context;

        public ReservationController(HospitalityReservationDbContext? context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: api/Reservation
        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<ReservationResponseDTO>> GetAllReservations([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var reservations = _context?.Reservations
                    .Include(r => r.User) // Include related User data
                    .Include(r => r.Venue) // Include related Venue data
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(r => new ReservationResponseDTO
                    {
                        Idreservation = r.Idreservation,
                        NumberOfGuests = r.NumberOfGuests,
                        Status = r.Status,
                        ReservationDate = r.ReservationDate,
                        UserName = r.User.Name ?? "Unknown User", // Check for null User and provide default
                        VenueName = r.Venue.VenueName ?? "Unknown Venue" // Check for null Venue and provide default
                    })
                    .ToList();

                if (reservations == null || !reservations.Any())
                {
                    return NotFound("No reservations found.");
                }

                return Ok(reservations);
            }
            catch (Exception ex)
            {
                // Log the exception (in real application, use a logger)
                return StatusCode(500, "An error occurred while fetching reservations. " + ex.Message);
            }
        }

        // GET: api/Reservation/{id}
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<ReservationResponseDTO> GetReservationById(int id)
        {
            try
            {
                var reservation = _context?.Reservations
                    .Include(r => r.User) // Include related User data
                    .Include(r => r.Venue) // Include related Venue data
                    .FirstOrDefault(r => r.Idreservation == id);

                if (reservation == null)
                {
                    return NotFound($"Reservation with ID {id} not found.");
                }

                var responseDto = new ReservationResponseDTO
                {
                    Idreservation = reservation.Idreservation,
                    NumberOfGuests = reservation.NumberOfGuests,
                    Status = reservation.Status,
                    ReservationDate = reservation.ReservationDate,
                    UserName = reservation.User?.Name ?? "Unknown User", // Check for null User and provide default
                    VenueName = reservation.Venue?.VenueName ?? "Unknown Venue" // Check for null Venue and provide default
                };

                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                // Log the exception (in real application, use a logger)
                return StatusCode(500, "An error occurred while fetching the reservation. " + ex.Message);
            }
        }

        // POST: api/Reservation
        [Authorize]
        [HttpPost]
        public ActionResult<ReservationResponseDTO> CreateReservation([FromBody] ReservationCreateDTO reservationDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                if (reservationDto == null)
                {
                    return BadRequest("Reservation data is required.");
                }

                // Check if User and Venue IDs are valid
                if (reservationDto.UserId == null || reservationDto.VenueId == null)
                {
                    return BadRequest("UserId and VenueId are required.");
                }

                var user = _context?.Users?.FirstOrDefault(u => u.Iduser == reservationDto.UserId);
                var venue = _context?.HospitalityVenues?.FirstOrDefault(v => v.Idvenue == reservationDto.VenueId);

                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                if (venue == null)
                {
                    return BadRequest("Venue not found.");
                }

                var reservation = new Reservation
                {
                    NumberOfGuests = reservationDto.NumberOfGuests,
                    Status = reservationDto.Status,
                    ReservationDate = reservationDto.ReservationDate,
                    UserId = reservationDto.UserId.Value,
                    VenueId = reservationDto.VenueId.Value
                };

                _context?.Reservations.Add(reservation);
                _context?.SaveChanges();

                var responseDto = new ReservationResponseDTO
                {
                    Idreservation = reservation.Idreservation,
                    NumberOfGuests = reservation.NumberOfGuests,
                    Status = reservation.Status,
                    ReservationDate = reservation.ReservationDate,
                    UserName = user.Name,
                    VenueName = venue.VenueName
                };

                return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Idreservation }, responseDto);
            }
            catch (Exception ex)
            {
                // Log the exception (in real application, use a logger)
                return StatusCode(500, "An error occurred while creating the reservation. " + ex.Message);
            }
        }

        // PUT: api/Reservation/{id}
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateReservation(int id, [FromBody] ReservationUpdateDTO reservationDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var reservation = _context?.Reservations.FirstOrDefault(r => r.Idreservation == id);
                if (reservation == null)
                {
                    return NotFound($"Reservation with ID {id} not found.");
                }

                reservation.NumberOfGuests = reservationDto.NumberOfGuests;
                reservation.Status = reservationDto.Status;
                reservation.ReservationDate = reservationDto.ReservationDate;

                _context?.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception (in real application, use a logger)
                return StatusCode(500, "An error occurred while updating the reservation. " + ex.Message);
            }
        }

        // DELETE: api/Reservation/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(int id)
        {
            try
            {
                var reservation = _context?.Reservations.Find(id);
                if (reservation == null)
                {
                    return NotFound($"Reservation with ID {id} not found.");
                }

                _context?.Reservations.Remove(reservation);
                _context?.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception (in real application, use a logger)
                return StatusCode(500, "An error occurred while deleting the reservation. " + ex.Message);
            }
        }
    }
}
