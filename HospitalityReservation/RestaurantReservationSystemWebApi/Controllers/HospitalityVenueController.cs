using AutoMapper;
using Dao.Models;
using Dao.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservationSystemWebApi.Controllers.DTOs;

namespace RestaurantReservationSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalityVenueController : ControllerBase
    {
        private readonly HospitalityVenueService _venueService;
        private readonly IMapper _mapper;
        private readonly LogService _logService;

        public HospitalityVenueController(HospitalityVenueService venueService, LogService logService, IMapper mapper)
        {
            _venueService = venueService;
            _logService = logService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<HospitalityVenueResponseDTO>> GetAllVenues([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var venues = _venueService.GetAll(page, pageSize);
                var response = _mapper.Map<IEnumerable<HospitalityVenueResponseDTO>>(venues);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logService.Log($"Failed to retrieve venues: {ex.Message}", 3);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<HospitalityVenueResponseDTO> GetVenueById(int id)
        {
            try
            {
                var venue = _venueService.GetById(id);
                if (venue == null)
                {
                    _logService.Log($"Venue with ID={id} not found.", 2);
                    return NotFound();
                }


                var response = _mapper.Map<HospitalityVenueResponseDTO>(venue);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logService.Log($"Error retrieving venue ID={id}: {ex.Message}", 3);
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<HospitalityVenueResponseDTO> CreateVenue([FromBody] HospitalityVenueCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var type = _venueService.GetHospitalityTypeById(dto.TypeId);
                if (type == null)
                    return BadRequest("Invalid HospitalityType.");

                var venue = _mapper.Map<HospitalityVenue>(dto);
                var created = _venueService.Create(venue);

                _logService.Log($"Venue '{venue.VenueName}' created with ID={created.Idvenue}.", 1);

                var response = _mapper.Map<HospitalityVenueResponseDTO>(created);
                response.TypeName = type.TypeName;

                return CreatedAtAction(nameof(GetVenueById), new { id = created.Idvenue }, response);
            }
            catch (Exception ex)
            {
                _logService.Log($"Error creating venue: {ex.Message}", 3);
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateVenue(int id, [FromBody] HospitalityVenueUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var venue = _venueService.GetById(id);
                if (venue == null)
                {
                    _logService.Log($"Attempted update on non-existing venue ID={id}.", 2);
                    return NotFound();

                }

                var typeId = dto.TypeId ?? venue.TypeId;
                if (!typeId.HasValue)
                    return BadRequest("TypeId is required.");


                var type = _venueService.GetHospitalityTypeById(typeId.Value);

                if (type == null)
                    return BadRequest("Invalid HospitalityType.");

               
                if (dto.VenueName != null) venue.VenueName = dto.VenueName;
                if (dto.Address != null) venue.Address = dto.Address;
                if (dto.TypeId.HasValue) venue.TypeId = dto.TypeId.Value;

                _venueService.Update(venue);
                _logService.Log($"Venue ID={id} was updated.", 1); 
                return NoContent();
            }
            catch (Exception ex)
            {
                _logService.Log($"Error updating venue ID={id}: {ex.Message}", 3); 
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteVenue(int id)
        {
            try
            {
                var venue = _venueService.GetById(id);
                if (venue == null)
                {
                    _logService.Log($"Attempted to delete non-existing venue ID={id}.", 2);
                    return NotFound("Venue not found.");
                }

                _venueService.Delete(venue);
                _logService.Log($"Venue ID={id} was deleted.", 1);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logService.Log($"Error deleting venue ID={id}: {ex.Message}", 3);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("search")]
        public ActionResult<IEnumerable<HospitalityVenueResponseDTO>> Search([FromQuery] string query)
        {
            try
            {
                var venues = _venueService.GetAllQueryable()
                    .Where(h =>
                        EF.Functions.Like(h.VenueName, $"%{query}%") ||
                        EF.Functions.Like(h.Address, $"%{query}%"))
                    .ToList();

                var response = _mapper.Map<IEnumerable<HospitalityVenueResponseDTO>>(venues);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logService.Log($"Venue search failed: {ex.Message}", 3);
                return StatusCode(500, ex.Message);
            }
        }


    }
}