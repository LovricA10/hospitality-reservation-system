using AutoMapper;
using Dao.Models;
using Dao.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class HospitalityTypeController : Controller
    {
        private readonly HospitalityTypeService _typeService;
        private readonly IMapper _mapper;

        public HospitalityTypeController(HospitalityTypeService typeService, IMapper mapper)
        {
            _typeService = typeService;
            _mapper = mapper;
        }
        // GET: HospitalityTypeController
        public ActionResult Index(string? q, int page = 1, int pageSize = 10)
        {
            var types = _typeService.GetAll();

            if (!string.IsNullOrWhiteSpace(q))
                types = types.Where(t => t.TypeName != null && t.TypeName.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();

            var totalCount = types.Count();
            var paged = types.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = _mapper.Map<List<HospitalityTypeViewModel>>(paged);

            ViewData["CurrentFilter"] = q;
            ViewData["TotalCount"] = totalCount;
            ViewData["Page"] = page;
            ViewData["PageSize"] = pageSize;

            return View(model);
        }

        // GET: HospitalityTypeController/Details/5
        public ActionResult Details(int id)
        {
            var type = _typeService.GetById(id);
            if (type == null) return NotFound();

            var model = _mapper.Map<HospitalityTypeViewModel>(type);
            return View(model);
        }

        // GET: HospitalityTypeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HospitalityTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HospitalityTypeViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var type = _mapper.Map<HospitalityType>(model);
            _typeService.Create(type);

            return RedirectToAction(nameof(Index));
        }

        // GET: HospitalityTypeController/Edit/5
        public ActionResult Edit(int id)
        {
            var type = _typeService.GetById(id);
            if (type == null) return NotFound();

            var model = _mapper.Map<HospitalityTypeViewModel>(type);
            return View(model);
        }

        // POST: HospitalityTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, HospitalityTypeViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var updated = _mapper.Map<HospitalityType>(model);
            updated.Idtype = id;

            _typeService.Update(updated);
            return RedirectToAction(nameof(Index));
        }

        // GET: HospitalityTypeController/Delete/5
        public ActionResult Delete(int id)
        {
            var type = _typeService.GetById(id);
            if (type == null) return NotFound();

            var model = _mapper.Map<HospitalityTypeViewModel>(type);
            return View(model);
        }

        // POST: HospitalityType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _typeService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
