using AutoMapper;
using Dao.Models;
using Dao.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservationSystemWebApi.Controllers.DTOs;
using System.Linq;

namespace RestaurantReservationSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _reservationService;
        private readonly IMapper _mapper;
        private readonly LogService _logService;

        public ReservationController(ReservationService reservationService, IMapper mapper, LogService logService)
        {
            _reservationService = reservationService;
            _mapper = mapper;
            _logService = logService;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<ReservationResponseDTO>> GetAllReservations([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var reservations = _reservationService.GetAll(page, pageSize);
                var response = _mapper.Map<IEnumerable<ReservationResponseDTO>>(reservations);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logService.Log($"Error fetching reservations: {ex.Message}", 3);
                return StatusCode(500, $"An error occurred while fetching reservations. {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<ReservationResponseDTO> GetReservationById(int id)
        {
            try
            {
                var reservation = _reservationService.GetById(id);
                if (reservation == null)
                {
                    _logService.Log($"Reservation with ID={id} not found.", 2);
                    return NotFound($"Reservation with ID {id} not found.");
                }

                var response = _mapper.Map<ReservationResponseDTO>(reservation);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logService.Log($"Error fetching reservation ID={id}: {ex.Message}", 3);
                return StatusCode(500, $"An error occurred while fetching the reservation. {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult<ReservationResponseDTO> CreateReservation([FromBody] ReservationCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var reservation = _mapper.Map<Reservation>(dto);
                var created = _reservationService.Create(reservation);
                if (created == null)
                {
                    _logService.Log("Failed to create reservation.", 2);
                    return BadRequest("Failed to create reservation.");
                }

                _logService.Log($"Reservation created with ID={created.Idreservation}.", 1);
                var response = _mapper.Map<ReservationResponseDTO>(created);
                return CreatedAtAction(nameof(GetReservationById), new { id = created.Idreservation }, response);
            }
            catch (Exception ex)
            {
                _logService.Log($"Error creating reservation: {ex.Message}", 3);
                return StatusCode(500, $"An error occurred while creating the reservation. {ex.Message}");
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateReservation(int id, [FromBody] ReservationUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = _mapper.Map<Reservation>(dto);
                var success = _reservationService.Update(id, updated);
                if (!success)
                {
                    _logService.Log($"Attempted update on non-existing reservation ID={id}.", 2);
                    return NotFound($"Reservation with ID {id} not found.");
                }

                _logService.Log($"Reservation ID={id} was updated.", 1); 
                return NoContent();
            }
            catch (Exception ex)
            {
                _logService.Log($"Error updating reservation ID={id}: {ex.Message}", 3); 
                return StatusCode(500, $"An error occurred while updating the reservation. {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(int id)
        {
            try
            {
                var success = _reservationService.Delete(id);
                if (!success)
                {
                    _logService.Log($"Attempted delete on non-existing reservation ID={id}.", 2);
                    return NotFound($"Reservation with ID {id} not found.");
                }

                _logService.Log($"Reservation ID={id} was deleted.", 1);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logService.Log($"Error deleting reservation ID={id}: {ex.Message}", 3);
                return StatusCode(500, $"An error occurred while deleting the reservation. {ex.Message}");
            }
        }
        [HttpGet("search")]
        public ActionResult<IEnumerable<ReservationResponseDTO>> Search([FromQuery] string query)
        {
            try
            {
                var queryable = _reservationService.GetAllQueryable()
                    .Include(r => r.User)
                    .Include(r => r.Venue)
                    .AsNoTracking()
                    .Where(r =>
                       EF.Functions.Like(r.Status, $"%{query}%")
                    );
                var reservations = queryable.ToList();

                var response = reservations.Select(r => _mapper.Map<ReservationResponseDTO>(r)).ToList();
                //var response = _mapper.Map<IEnumerable<ReservationResponseDTO>>(reservations);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logService.Log($"Reservation search failed: {ex.Message}", 3);
                return StatusCode(500, ex.Message);
            }
        }

    }
}
