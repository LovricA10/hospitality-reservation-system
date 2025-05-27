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
    public class HospitalityVenueController : ControllerBase
    {
        private readonly HospitalityVenueService _venueService;
        private readonly IMapper _mapper;

        public HospitalityVenueController(HospitalityVenueService venueService, IMapper mapper)
        {
            _venueService = venueService;
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
                    return NotFound();

                var response = _mapper.Map<HospitalityVenueResponseDTO>(venue);
                return Ok(response);
            }
            catch (Exception ex)
            {
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

                var response = _mapper.Map<HospitalityVenueResponseDTO>(created);
                response.TypeName = type.TypeName;

                return CreatedAtAction(nameof(GetVenueById), new { id = created.Idvenue }, response);
            }
            catch (Exception ex)
            {
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
                    return NotFound();

                var typeId = dto.TypeId ?? venue.TypeId;
                if (!typeId.HasValue)
                    return BadRequest("TypeId is required.");


                var type = _venueService.GetHospitalityTypeById(typeId.Value);

                if (type == null)
                    return BadRequest("Invalid HospitalityType.");

                // Map only non-null properties
                if (dto.VenueName != null) venue.VenueName = dto.VenueName;
                if (dto.Address != null) venue.Address = dto.Address;
                if (dto.TypeId.HasValue) venue.TypeId = dto.TypeId.Value;

                _venueService.Update(venue);
                return NoContent();
            }
            catch (Exception ex)
            {
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
                    return NotFound("Venue not found.");

                _venueService.Delete(venue);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}