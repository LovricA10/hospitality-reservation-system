using AutoMapper;
using Dao.Models;
using Dao.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantReservationSystemWebApp.ViewModels;

namespace RestaurantReservationSystemWebApp.Controllers
{
    [Authorize]
    public class HospitalityVenueController : Controller
    {
        private readonly HospitalityVenueService _venueService;
        private readonly IMapper _mapper;
        private readonly HospitalityTypeService _typeService;
        private readonly LogService _logService;

        public HospitalityVenueController(
            HospitalityVenueService venueService,
            HospitalityTypeService typeService,
            IMapper mapper,
            LogService logService)
        {
            _venueService = venueService;
            _typeService = typeService;
            _mapper = mapper;
            _logService = logService;
        }

        public ActionResult Index(string? q, int? categoryId, int page = 1, int pageSize = 10)
        {
            var query = _venueService.GetAllQueryable();

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(v => EF.Functions.Like(v.VenueName!, $"%{q}%"));

            if (categoryId.HasValue)
                query = query.Where(v => v.TypeId == categoryId.Value);

            var totalCount = query.Count();
            var venues = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var model = _mapper.Map<List<HospitalityVenueViewModel>>(venues);

            var categories = _typeService.GetAll();
            ViewBag.CategoryList = new SelectList(categories, "Idtype", "TypeName");
            ViewData["CurrentFilter"] = q;
            ViewData["CurrentCategory"] = categoryId?.ToString();
            ViewData["Page"] = page;
            ViewData["PageSize"] = pageSize;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(model);
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var venue = _venueService.GetById(id);
            if (venue == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<HospitalityVenueViewModel>(venue);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create(object? selected = null)
        {
            var types = _typeService.GetAll();
            ViewBag.TypeList = new SelectList(types, "Idtype", "TypeName", selected);
            return View();
        }

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
            if (_venueService.GetAllQueryable()
               .Any(v => v.VenueName != null &&
               EF.Functions.Like(v.VenueName, model.VenueName)))
            {
                var types = _typeService.GetAll();
                ViewBag.TypeList = new SelectList(types, "Idtype", "TypeName", model.TypeId);

                ModelState.AddModelError("VenueName", "A venue with this name already exists.");
                return View(model);
            }


            var venue = _mapper.Map<HospitalityVenue>(model);
            _venueService.Create(venue);

            _logService.Log($"Venue '{venue.VenueName}' created by {User.Identity?.Name}.", 1);

            return RedirectToAction(nameof(Index));
        }

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

            if (_venueService.GetAllQueryable()
               .Any(v => v.VenueName != null &&
               v.Idvenue != id &&
               EF.Functions.Like(v.VenueName, model.VenueName)))
            {
                var types = _typeService.GetAll();
                ViewBag.TypeList = new SelectList(types, "Idtype", "TypeName", model.TypeId);

                ModelState.AddModelError("VenueName", "A venue with this name already exists.");
                return View(model);
            }


            var existingVenue = _venueService.GetById(id);
            if (existingVenue == null) return NotFound();

            var updated = _mapper.Map<HospitalityVenue>(model);
            updated.Idvenue = id;
            _venueService.Update(updated);

            _logService.Log($"Venue ID={id} updated by {User.Identity?.Name}.", 1);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var venue = _venueService.GetById(id);
            if (venue == null) return NotFound();

            var model = _mapper.Map<HospitalityVenueViewModel>(venue);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            var venue = _venueService.GetById(id);
            if (venue == null) return NotFound();

            _venueService.Delete(venue);

            _logService.Log($"Venue '{venue.VenueName}' (ID={id}) deleted by {User.Identity?.Name}.", 1);

            return RedirectToAction(nameof(Index));
        }
    }
}
