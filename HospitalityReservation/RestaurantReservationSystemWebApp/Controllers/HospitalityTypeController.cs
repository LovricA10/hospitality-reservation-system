using AutoMapper;
using Dao.Models;
using Dao.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservationSystemWebApp.ViewModels;

namespace RestaurantReservationSystemWebApp.Controllers
{
    [Authorize]
    public class HospitalityTypeController : Controller
    {
        private readonly HospitalityTypeService _typeService;
        private readonly IMapper _mapper;
        private readonly LogService _logService;

        public HospitalityTypeController(HospitalityTypeService typeService, IMapper mapper, LogService logService)
        {
            _typeService = typeService;
            _mapper = mapper;
            _logService = logService;
        }
        [HttpGet]
        public ActionResult Index(string? q, int page = 1, int pageSize = 10)
        {
            var query = _typeService.GetAllQueryable();

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(t => t.TypeName != null && EF.Functions.Like(t.TypeName, $"%{q}%"));

            var totalCount = query.Count();
            var paged = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var model = _mapper.Map<List<HospitalityTypeViewModel>>(paged);

            ViewData["CurrentFilter"] = q;
            ViewData["Page"] = page;
            ViewData["PageSize"] = pageSize;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(model);
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var type = _typeService.GetById(id);
            if (type == null) return NotFound();

            var model = _mapper.Map<HospitalityTypeViewModel>(type);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(HospitalityTypeViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var type = _mapper.Map<HospitalityType>(model);
            _typeService.Create(type);

            _logService.Log($"Hospitality type '{type.TypeName}' created by {User.Identity?.Name}.", 1);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var type = _typeService.GetById(id);
            if (type == null) return NotFound();

            var model = _mapper.Map<HospitalityTypeViewModel>(type);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, HospitalityTypeViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var updated = _mapper.Map<HospitalityType>(model);
            updated.Idtype = id;

            _typeService.Update(updated);

            _logService.Log($"Hospitality type ID={id} updated by {User.Identity?.Name}.", 1);

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var type = _typeService.GetById(id);
            if (type == null) return NotFound();

            var model = _mapper.Map<HospitalityTypeViewModel>(type);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var existing = _typeService.GetById(id);
            if (existing != null)
            {
                _typeService.Delete(id);
                _logService.Log($"Hospitality type '{existing.TypeName}' (ID={id}) deleted by {User.Identity?.Name}.", 1);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
