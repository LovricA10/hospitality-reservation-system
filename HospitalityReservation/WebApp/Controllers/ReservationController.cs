using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Controllers.DTOs;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {


        private readonly HospitalityReservationDbContext? _context;

        public ReservationController(HospitalityReservationDbContext? context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReservationResponseDTO>> GetAllReservations()
        {
            try
            {
                return Ok(_context?.Reservations.Select(r => new ReservationResponseDTO
                {
                    Idreservation = r.Idreservation,
                    NumberOfGuests = r.NumberOfGuests,
                    Status = r.Status,
                    ReservationDate = r.ReservationDate
                })
                    );
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{Id}")]

        public ActionResult<ReservationResponseDTO> GetReservationById(int id)
        {
            try
            {
                var reservation = _context?.Reservations.FirstOrDefault(r => r.Idreservation == id);

                if (reservation == null)
                {
                    return NotFound();
                }

                var responseDto = new ReservationResponseDTO
                {
                    Idreservation = reservation.Idreservation,
                    NumberOfGuests = reservation.NumberOfGuests,
                    Status = reservation.Status,
                    ReservationDate = reservation.ReservationDate,

                };

                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<ReservationResponseDTO> CreateReservation([FromBody] ReservationCreateDTO reservationDto)
        {
            try
            {
                if (reservationDto == null)
                {
                    return BadRequest("Error Data.");
                }

                var reservation = new Reservation
                {
                    NumberOfGuests = reservationDto.NumberOfGuests,
                    Status = reservationDto.Status,
                    ReservationDate = reservationDto.ReservationDate,
                    UserId = reservationDto.UserId,
                    VenueId = reservationDto.VenueId
                };

                _context?.Reservations.Add(reservation);
                _context?.SaveChanges();

                var responseDto = new ReservationResponseDTO
                {
                    Idreservation = reservation.Idreservation,
                    NumberOfGuests = reservation.NumberOfGuests,
                    Status = reservation.Status,
                    ReservationDate = reservation.ReservationDate
                };

                return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Idreservation }, responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReservation(int id, [FromBody] ReservationUpdateDTO reservationDto)
        {
            try
            {
                var reservation = _context?.Reservations.FirstOrDefault(r => r.Idreservation == id);
                if (reservation == null)
                {
                    return NotFound();
                }

                reservation.NumberOfGuests = reservationDto.NumberOfGuests;
                reservation.Status = reservationDto.Status;
                reservation.ReservationDate = reservationDto.ReservationDate;

                _context?.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(int id)
        {
            try
            {
                var reservation = _context?.Reservations.Find(id);
                if (reservation == null)
                {
                    return NotFound("Reservation not found.");
                }

                _context?.Reservations.Remove(reservation);
                _context?.SaveChanges();

                return NoContent(); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
