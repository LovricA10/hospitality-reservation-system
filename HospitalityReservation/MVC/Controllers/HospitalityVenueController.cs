using AutoMapper;
using Dao.Models;
using Dao.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class HospitalityVenueController : Controller
    {
        private readonly HospitalityVenueService _venueService;
        private readonly IMapper _mapper;

        public HospitalityVenueController(HospitalityVenueService venueService, IMapper mapper)
        {
            _venueService = venueService;
            _mapper = mapper;
        }
        // GET: HospitalityVenueController
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var venues = _venueService.GetAll(page, pageSize);
            var model = _mapper.Map<List<HospitalityVenueViewModel>>(venues);
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: HospitalityVenueController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HospitalityVenueViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var venue = _mapper.Map<HospitalityVenue>(model);
            _venueService.Create(venue);

            return RedirectToAction(nameof(Index));
        }

        // GET: HospitalityVenueController/Edit/5
        public ActionResult Edit(int id)
        {
            var venue = _venueService.GetById(id);
            if (venue == null) return NotFound();

            var model = _mapper.Map<HospitalityVenueViewModel>(venue);
            return View(model);
        }

        // POST: HospitalityVenueController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, HospitalityVenueViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var existingVenue = _venueService.GetById(id);
            if (existingVenue == null) return NotFound();

            var updated = _mapper.Map<HospitalityVenue>(model);
            updated.Idvenue = id;
            _venueService.Update(updated);

            return RedirectToAction(nameof(Index));
        }

        // GET: HospitalityVenueController/Delete/5
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
        public ActionResult DeleteConfirmed(int id)
        {
            var venue = _venueService.GetById(id);
            if (venue == null) return NotFound();

            _venueService.Delete(venue);

            return RedirectToAction(nameof(Index));
        }
    }
}
