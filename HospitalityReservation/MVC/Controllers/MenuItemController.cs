using AutoMapper;
using Dao.Models;
using Dao.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly MenuService _menuService;
        private readonly IMapper _mapper;
        private readonly HospitalityVenueService _venueService;

        public MenuItemController(MenuService menuService, HospitalityVenueService venueService, IMapper mapper)
        {
            _menuService = menuService;
            _venueService = venueService;
            _mapper = mapper;
        }
        // GET: MenuItemController
        public ActionResult Index()
        {
            var items = _menuService.GetAll();
            var model = _mapper.Map<List<MenuItemViewModel>>(items);
            return View(model);
        }

        // GET: MenuItemController/Details/5
        public ActionResult Details(int id)
        {
            var item = _menuService.GetById(id);
            if (item == null) return NotFound();

            var model = _mapper.Map<MenuItemViewModel>(item);
            return View(model);
        }

        // GET: MenuItemController/Create
        public ActionResult Create()
        {
            var venues = _venueService.GetAll(1, 100);
            ViewBag.VenueList = new SelectList(venues, "Idvenue", "VenueName");
            return View();
        }

        // POST: MenuItemController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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

            return RedirectToAction(nameof(Index));
        }

        // GET: MenuItemController/Edit/5
        public ActionResult Edit(int id)
        {
            var item = _menuService.GetById(id);
            if (item == null) return NotFound();

            var model = _mapper.Map<MenuItemViewModel>(item);

            var venues = _venueService.GetAll(1, 100);
            ViewBag.VenueList = new SelectList(venues, "Idvenue", "VenueName", model.VenueId);

            return View(model);
        }

        // POST: MenuItemController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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

            return RedirectToAction(nameof(Index));
        }

        // GET: MenuItemController/Delete/5
        public ActionResult Delete(int id)
        {
            var item = _menuService.GetById(id);
            if (item == null) return NotFound();

            var model = _mapper.Map<MenuItemViewModel>(item);
            return View(model);
        }

        // POST: MenuItemController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int venueId)
        {
            var success = _menuService.Delete(id, venueId);
            if (!success) return NotFound("Item not linked to venue.");

            return RedirectToAction(nameof(Index));
        }
    }
}
