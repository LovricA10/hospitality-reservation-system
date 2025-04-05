using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]

        public ActionResult<IEnumerable<MenuResponseDTO>> GetAllMenu()
        {
            try
            {
                return Ok(_context.MenuItems.Select(m => new MenuResponseDTO
                {
                    IdmenuItem = m.IdmenuItem,
                    ItemName = m.ItemName,
                    ItemType = m.ItemType,
                    Price = m.Price,
                    
                }).ToList());
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{Id}")]

        public ActionResult<MenuResponseDTO> GetMenuById(int Id)
        {
            try
            {
                var menu = _context.MenuItems.FirstOrDefault(m=> m.IdmenuItem == Id);

                if (menu == null)
                {
                    return NotFound($"Menu item with ID {Id} not found.");
                }

                var responseDto = new MenuResponseDTO
                {
                    IdmenuItem = menu.IdmenuItem,
                    ItemName = menu.ItemName,
                    ItemType = menu.ItemType,
                    Price = menu.Price
                };

                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<MenuResponseDTO> CreateMenu([FromBody] MenuCreateDTO menuDto)
        {
            try
            {
                if (menuDto == null)
                {
                    return BadRequest("Invalid Data.");
                }

                var menu = new MenuItem
                {
                    ItemName = menuDto.ItemName,
                    ItemType = menuDto.ItemType,
                    Price = menuDto.Price
                };

                _context.MenuItems.Add(menu);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetMenuById), new { id = menu.IdmenuItem }, new MenuResponseDTO
                {
                    IdmenuItem = menu.IdmenuItem,
                    ItemName = menu.ItemName,
                    ItemType = menu.ItemType,
                    Price = menu.Price
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMenu(int id, [FromBody] MenuUpdateDTO menuDto)
        {
            try
            {
                if (menuDto == null)
                {
                    return BadRequest("Invalid data.");
                }

                var menu = _context.MenuItems.FirstOrDefault(m => m.IdmenuItem == id);
                if (menu == null)
                {
                    return NotFound($"Menu item with ID {id} not found.");
                }
                menu.ItemName = menuDto.ItemName ?? menu.ItemName;
                menu.ItemType = menuDto.ItemType ?? menu.ItemType;
                menu.Price = menuDto.Price;

                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMenu(int id)
        {
            try
            {
                var menu = _context.MenuItems.Find(id);
                if (menu == null)
                {
                    return NotFound($"Menu item with ID {id} not found.");
                }

                _context.MenuItems.Remove(menu);
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
