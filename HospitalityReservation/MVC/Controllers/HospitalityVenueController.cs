using AutoMapper;
using Dao.Models;
using Dao.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.ViewModels;

namespace MVC.Controllers
{
    [Authorize]
    public class HospitalityVenueController : Controller
    {
        private readonly HospitalityVenueService _venueService;
        private readonly IMapper _mapper;
        private readonly HospitalityTypeService _typeService;

        public HospitalityVenueController(HospitalityVenueService venueService, HospitalityTypeService typeService, IMapper mapper)
        {
            _venueService = venueService;
            _typeService = typeService;
            _mapper = mapper;
        }
        // GET: HospitalityVenueController
        public ActionResult Index(string? q, int? categoryId, int page = 1, int pageSize = 10)
        {
            var venues = _venueService.GetAll(page, pageSize);

            if (!string.IsNullOrWhiteSpace(q))
                venues = venues.Where(v => v.VenueName != null && v.VenueName.Contains(q, StringComparison.OrdinalIgnoreCase));

            if (categoryId.HasValue)
                venues = venues.Where(v => v.TypeId == categoryId.Value);

            var model = _mapper.Map<List<HospitalityVenueViewModel>>(venues);

            // Fill ViewBag
            var categories = _typeService.GetAll();
            ViewBag.CategoryList = new SelectList(categories, "Idtype", "TypeName");
            ViewData["CurrentFilter"] = q;
            ViewData["CurrentCategory"] = categoryId?.ToString();

            return View(model);
        }

        // GET: HospitalityVenueController/Details/5
        public ActionResult Details(int id)
        {
            var venue = _venueService.GetById(id);
            if (venue == null) return NotFound();

            var model = _mapper.Map<HospitalityVenueViewModel>(venue);
            return View(model);
        }

        // GET: HospitalityVenueController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create(object? selected = null)
        {
            var types = _typeService.GetAll();
            ViewBag.TypeList = new SelectList(types, "Idtype", "TypeName", selected);
            return View();
        }

        // POST: HospitalityVenueController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(HospitalityVenueViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var types = _typeService.GetAll();
                ViewBag.TypeList = new SelectList(types, "Idtype", "TypeName", model.TypeId);
                return View(model);
            }

            var venue = _mapper.Map<HospitalityVenue>(model);
            _venueService.Create(venue);

            return RedirectToAction(nameof(Index));
        }

        // GET: HospitalityVenueController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var venue = _venueService.GetById(id);
            if (venue == null) return NotFound();

            var model = _mapper.Map<HospitalityVenueViewModel>(venue);

            var types = _typeService.GetAll();
            ViewBag.TypeList = new SelectList(types, "Idtype", "TypeName", model.TypeId);

            return View(model);
        }

        // POST: HospitalityVenueController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, HospitalityVenueViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var types = _typeService.GetAll();
                ViewBag.TypeList = new SelectList(types, "Idtype", "TypeName", model.TypeId);
                return View(model);
            }

            var existingVenue = _venueService.GetById(id);
            if (existingVenue == null) return NotFound();

            var updated = _mapper.Map<HospitalityVenue>(model);
            updated.Idvenue = id;
            _venueService.Update(updated);

            return RedirectToAction(nameof(Index));
        }

        // GET: HospitalityVenueController/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var venue = _venueService.GetById(id);
            if (venue == null) return NotFound();

            var model = _mapper.Map<HospitalityVenueViewModel>(venue);
            return View(model);
        }

        // POST: HospitalityVenueController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            var venue = _venueService.GetById(id);
            if (venue == null) return NotFound();

            _venueService.Delete(venue);

            return RedirectToAction(nameof(Index));
        }
    }
}
