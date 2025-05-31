using AutoMapper;
using Dao.Models;
using Dao.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ReservationService _reservationService;
        private readonly HospitalityVenueService _venueService;
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public ReservationController(
            ReservationService reservationService,
            HospitalityVenueService venueService,
            UserService userService,
            IMapper mapper)
        {
            _reservationService = reservationService;
            _venueService = venueService;
            _userService = userService;
            _mapper = mapper;
        }

        // GET: ReservationController
        public ActionResult Index()
        {
            var reservations = _reservationService.GetAll();
            var model = _mapper.Map<List<ReservationViewModel>>(reservations);
            return View(model);
        }

        // GET: ReservationController/Details/5
        public ActionResult Details(int id)
        {
            var reservation = _reservationService.GetById(id);
            if (reservation == null) return NotFound();

            var model = _mapper.Map<ReservationViewModel>(reservation);
            return View(model);
        }

        // GET: ReservationController/Create
        public ActionResult Create(int page = 1, int pageSize = 10)
        {
            ViewBag.Venues = _venueService.GetAll(page,pageSize);
            ViewBag.Users = _userService.GetAll();
            return View();
        }

        // POST: ReservationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReservationViewModel model, int page = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Venues = _venueService.GetAll(page,pageSize);
                ViewBag.Users = _userService.GetAll();
                return View(model);
            }

            var reservation = _mapper.Map<Reservation>(model);
            _reservationService.Create(reservation);
            return RedirectToAction(nameof(Index));
        }

        // GET: ReservationController/Edit/5
        public ActionResult Edit(int id, int page = 1, int pageSize = 10)
        {
            var reservation = _reservationService.GetById(id);
            if (reservation == null) return NotFound();

            var model = _mapper.Map<ReservationViewModel>(reservation);
            ViewBag.Venues = _venueService.GetAll(page,pageSize);
            ViewBag.Users = _userService.GetAll();
            return View(model);
        }

        // POST: ReservationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ReservationViewModel model, int page = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Venues = _venueService.GetAll(page,pageSize);
                ViewBag.Users = _userService.GetAll();
                return View(model);
            }

            var updated = _mapper.Map<Reservation>(model);
            updated.Idreservation = id;

            var success = _reservationService.Update(id, updated);
            if (!success) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // GET: ReservationController/Delete/5
        public ActionResult Delete(int id)
        {
            var reservation = _reservationService.GetById(id);
            if (reservation == null) return NotFound();

            var model = _mapper.Map<ReservationViewModel>(reservation);
            return View(model);
        }

        // POST: ReservationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var success = _reservationService.Delete(id);
            if (!success) return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
