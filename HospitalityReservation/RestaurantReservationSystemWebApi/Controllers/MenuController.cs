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
    public class MenuController : ControllerBase
    {
        private readonly MenuService _menuService;
        private readonly IMapper _mapper;
        private readonly LogService _logService;

        public MenuController(MenuService menuService, IMapper mapper, LogService logService)
        {
            _menuService = menuService;
            _mapper = mapper;
            _logService = logService;
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
                _logService.Log($"Failed to retrieve menu items: {ex.Message}", 3);
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
                {
                    _logService.Log($"Menu item with ID={id} not found.", 2);
                    return NotFound();
                }

                if (venueId.HasValue && !item.VenueMenuItems.Any(vm => vm.VenueId == venueId))
                {
                    _logService.Log($"Menu item ID={id} is not available in venue ID={venueId}.", 2);
                    return BadRequest("Menu item is not available in this venue.");
                }

                return Ok(_mapper.Map<MenuResponseDTO>(item));
            }
            catch (Exception ex)
            {
                _logService.Log($"Error retrieving menu item ID={id}: {ex.Message}", 3);
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

                if (created == null)
                {
                    _logService.Log("Failed to create menu item.", 2);
                    return BadRequest("Failed to create item.");
                }

                _logService.Log($"Menu item '{item.ItemName}' created with ID={created.IdmenuItem}.", 1);
                return CreatedAtAction(nameof(GetMenuItem), new { id = created.IdmenuItem }, _mapper.Map<MenuResponseDTO>(created));
            }
            catch (Exception ex)
            {
                _logService.Log($"Error creating menu item: {ex.Message}", 3);
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
                if (!success)
                {
                    _logService.Log($"Attempted update on unlinked or non-existing menu item ID={id}.", 2);
                    return NotFound("Menu item is not linked to specified venue.");
                }

                _logService.Log($"Menu item ID={id} was updated.", 1);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logService.Log($"Error updating menu item ID={id}: {ex.Message}", 3);
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
                if (!success)
                {
                    _logService.Log($"Attempted delete on unlinked or non-existing menu item ID={id} in venue ID={venueId}.", 2);
                    return NotFound("Menu item not linked to this venue.");
                }

                _logService.Log($"Menu item ID={id} deleted from venue ID={venueId}.", 1);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logService.Log($"Error deleting menu item ID={id}: {ex.Message}", 3);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<MenuResponseDTO>> Search([FromQuery] string query)
        {
            try
            {
                var items = _menuService.GetAllQueryable()
                    .Where(m => EF.Functions.Like(m.ItemName, $"%{query}%"))
                    .ToList();

                var response = _mapper.Map<IEnumerable<MenuResponseDTO>>(items);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logService.Log($"Menu search failed: {ex.Message}", 3);
                return StatusCode(500, ex.Message);
            }
        }

    }
}

