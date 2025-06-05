using AutoMapper;
using Dao.Models;
using Dao.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.ViewModels;

namespace MVC.Controllers
{
    [Authorize]
    public class MenuItemController : Controller
    {
        private readonly MenuService _menuService;
        private readonly HospitalityVenueService _venueService;
        private readonly IMapper _mapper;
        private readonly LogService _logService;

        public MenuItemController(MenuService menuService, HospitalityVenueService venueService, IMapper mapper, LogService logService)
        {
            _menuService = menuService;
            _venueService = venueService;
            _mapper = mapper;
            _logService = logService;
        }

        public ActionResult Index(string? q, string? categoryId, int page = 1, int pageSize = 10)
        {
            var query = _menuService.GetAllQueryable();

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(i => i.ItemName != null && i.ItemName.Contains(q, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(categoryId))
                query = query.Where(i => i.ItemType != null && i.ItemType.Equals(categoryId, StringComparison.OrdinalIgnoreCase));

            var totalCount = query.Count();

            var pagedItems = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = _mapper.Map<List<MenuItemViewModel>>(pagedItems);

            var categories = new List<SelectListItem>
            {
                new SelectListItem { Value = "Food", Text = "Food" },
                new SelectListItem { Value = "Drink", Text = "Drink" }
            };

            ViewBag.CategoryList = new SelectList(categories, "Value", "Text");
            ViewData["CurrentFilter"] = q;
            ViewData["CurrentCategory"] = categoryId;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewData["Page"] = page;
            ViewData["PageSize"] = pageSize;

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var venues = _venueService.GetAll(1, 100);
            ViewBag.VenueList = new SelectList(venues, "Idvenue", "VenueName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(MenuItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var venues = _venueService.GetAll(1, 100);
                ViewBag.VenueList = new SelectList(venues, "Idvenue", "VenueName", model.VenueId);
                return View(model);
            }

            var item = _mapper.Map<MenuItem>(model);
            _menuService.Create(item, model.VenueId);

            _logService.Log($"Menu item '{item.ItemName}' created by {User.Identity?.Name}.", 1);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var item = _menuService.GetById(id);
            if (item == null) return NotFound();

            var model = _mapper.Map<MenuItemViewModel>(item);
            var venues = _venueService.GetAll(1, 100);
            ViewBag.VenueList = new SelectList(venues, "Idvenue", "VenueName", model.VenueId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, MenuItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var venues = _venueService.GetAll(1, 100);
                ViewBag.VenueList = new SelectList(venues, "Idvenue", "VenueName", model.VenueId);
                return View(model);
            }

            var updated = _mapper.Map<MenuItem>(model);
            updated.IdmenuItem = id;

            var success = _menuService.Update(id, updated, model.VenueId);
            if (!success) return NotFound("Menu item not found or not linked to venue.");

            _logService.Log($"Menu item ID={id} updated by {User.Identity?.Name}.", 1);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var item = _menuService.GetById(id);
            if (item == null) return NotFound();

            var model = _mapper.Map<MenuItemViewModel>(item);
            var venueMenu = item.VenueMenuItems.FirstOrDefault();
            if (venueMenu != null)
            {
                model.VenueId = venueMenu.VenueId.GetValueOrDefault();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id, int venueId)
        {
            var success = _menuService.Delete(id, venueId);
            if (!success) return NotFound("Item not linked to venue.");

            _logService.Log($"Menu item ID={id} deleted by {User.Identity?.Name}.", 1);

            return RedirectToAction(nameof(Index));
        }
    }
}
