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
    public class MenuController : ControllerBase
    {
        private readonly MenuService _menuService;
        private readonly IMapper _mapper;

        public MenuController(MenuService menuService, IMapper mapper)
        {
            _menuService = menuService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MenuResponseDTO>> GetMenu([FromQuery] int? venueId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var items = _menuService.GetAll(venueId, page, pageSize);
                var result = _mapper.Map<IEnumerable<MenuResponseDTO>>(items);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<MenuResponseDTO> GetMenuItem(int id, [FromQuery] int? venueId = null)
        {
            try
            {
                var item = _menuService.GetById(id);
                if (item == null)
                    return NotFound();

                if (venueId.HasValue && !item.VenueMenuItems.Any(vm => vm.VenueId == venueId))
                    return BadRequest("Menu item is not available in this venue.");

                return Ok(_mapper.Map<MenuResponseDTO>(item));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<MenuResponseDTO> CreateMenu([FromBody] MenuCreateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var item = _mapper.Map<MenuItem>(dto);
                var created = _menuService.Create(item, dto.HospitalityVenueID);

                if (created == null) return BadRequest("Failed to create item.");

                return CreatedAtAction(nameof(GetMenuItem), new { id = created.IdmenuItem }, _mapper.Map<MenuResponseDTO>(created));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateMenu(int id, [FromBody] MenuUpdateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updated = _mapper.Map<MenuItem>(dto);
                var success = _menuService.Update(id, updated, dto.HospitalityVenueID);
                if (!success) return NotFound("Menu item is not linked to specified venue.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteMenu(int id, [FromQuery] int venueId)
        {
            try
            {
                var success = _menuService.Delete(id, venueId);
                if (!success) return NotFound("Menu item not linked to this venue.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

