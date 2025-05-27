using AutoMapper;
using Dao.Models;
using Dao.Services;
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
        private readonly ReservationService _reservationService;
        private readonly IMapper _mapper;

        public ReservationController(ReservationService reservationService, IMapper mapper)
        {
            _reservationService = reservationService;
            _mapper = mapper;
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
                    return NotFound($"Reservation with ID {id} not found.");

                var response = _mapper.Map<ReservationResponseDTO>(reservation);
                return Ok(response);
            }
            catch (Exception ex)
            {
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
                    return BadRequest("Failed to create reservation.");

                var response = _mapper.Map<ReservationResponseDTO>(created);
                return CreatedAtAction(nameof(GetReservationById), new { id = created.Idreservation }, response);
            }
            catch (Exception ex)
            {
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
                    return NotFound($"Reservation with ID {id} not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
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
                    return NotFound($"Reservation with ID {id} not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the reservation. {ex.Message}");
            }
        }
    }
}
