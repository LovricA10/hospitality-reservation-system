using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Controllers.DTOs;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly HospitalityReservationDbContext _context;

        public MenuController(HospitalityReservationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public ActionResult<IEnumerable<MenuResponseDTO>> GetMenu([FromQuery] int? venueId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = _context.VenueMenuItems
                    .Include(vm => vm.MenuItem)
                    .Include(vm => vm.Venue)
                    .AsQueryable();

                if (venueId.HasValue)
                {
                    query = query.Where(vm => vm.VenueId == venueId.Value);
                }

                var paginatedMenu = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(vm => new MenuResponseDTO
                    {
                        IdmenuItem = vm.MenuItem.IdmenuItem,
                        ItemName = vm.MenuItem.ItemName,
                        ItemType = vm.MenuItem.ItemType,
                        Price = vm.MenuItem.Price
                    })
                    .ToList();

                return Ok(paginatedMenu);
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
                var menuItem = _context.MenuItems
                    .Include(m => m.VenueMenuItems)
                    .ThenInclude(vm => vm.Venue)
                    .FirstOrDefault(m => m.IdmenuItem == id);

                if (menuItem == null)
                    return NotFound();

                if (venueId.HasValue && !menuItem.VenueMenuItems.Any(vm => vm.VenueId == venueId))
                {
                    return BadRequest("Menu item is not available in this venue.");
                }

                return Ok(new MenuResponseDTO
                {
                    IdmenuItem = menuItem.IdmenuItem,
                    ItemName = menuItem.ItemName,
                    ItemType = menuItem.ItemType,
                    Price = menuItem.Price
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<MenuResponseDTO> CreateMenu([FromBody] MenuCreateDTO dto)
        {
            try
            {
                var newItem = new MenuItem
                {
                    ItemName = dto.ItemName,
                    ItemType = dto.ItemType,
                    Price = dto.Price
                };

                _context.MenuItems.Add(newItem);
                _context.SaveChanges();

                var venue = _context.HospitalityVenues.Find(dto.HospitalityVenueID);
                if (venue == null)
                    return BadRequest("Venue does not exist.");

                var link = new VenueMenuItem
                {
                    MenuItemId = newItem.IdmenuItem,
                    VenueId = venue.Idvenue
                };

                _context.VenueMenuItems.Add(link);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetMenuItem), new { id = newItem.IdmenuItem }, new MenuResponseDTO
                {
                    IdmenuItem = newItem.IdmenuItem,
                    ItemName = newItem.ItemName,
                    ItemType = newItem.ItemType,
                    Price = newItem.Price
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMenu(int id, [FromBody] MenuUpdateDTO dto)
        {
            try
            {
                var menuItem = _context.MenuItems
                    .Include(m => m.VenueMenuItems)
                    .FirstOrDefault(m => m.IdmenuItem == id);

                if (menuItem == null)
                    return NotFound();

                if (!menuItem.VenueMenuItems.Any(vm => vm.VenueId == dto.HospitalityVenueID))
                    return NotFound("Menu item is not linked to specified venue.");

                menuItem.ItemName = dto.ItemName ?? menuItem.ItemName;
                menuItem.ItemType = dto.ItemType ?? menuItem.ItemType;
                menuItem.Price = dto.Price ?? menuItem.Price;

                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMenu(int id, [FromQuery] int venueId)
        {
            try
            {
                var menuItem = _context.MenuItems
                    .Include(m => m.VenueMenuItems)
                    .FirstOrDefault(m => m.IdmenuItem == id);

                if (menuItem == null)
                    return NotFound();

                var venueLink = menuItem.VenueMenuItems.FirstOrDefault(vm => vm.VenueId == venueId);
                if (venueLink == null)
                    return NotFound("Menu item not linked to this venue.");

                _context.VenueMenuItems.Remove(venueLink);

                if (menuItem.VenueMenuItems.Count == 1) // Only one link exists
                {
                    _context.MenuItems.Remove(menuItem);
                }

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
