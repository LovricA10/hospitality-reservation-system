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
        // GET: ReservationController
        public ActionResult Index(string? q, string? categoryId)
        {
            var reservations = _reservationService.GetAll();

            if (!string.IsNullOrWhiteSpace(q))
            {
                reservations = reservations.Where(r =>
                    (r.User?.Name != null && r.User.Name.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                    (r.Venue?.VenueName != null && r.Venue.VenueName.Contains(q, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                reservations = reservations.Where(r => r.Status != null && r.Status.Equals(categoryId, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var model = _mapper.Map<List<ReservationViewModel>>(reservations);

            var statusOptions = new List<SelectListItem>
    {
        new SelectListItem("Pending", "Pending"),
        new SelectListItem("Confirmed", "Confirmed"),
        new SelectListItem("Cancelled", "Cancelled")
    };

            ViewBag.CategoryList = new SelectList(statusOptions, "Value", "Text");
            ViewData["CurrentFilter"] = q;
            ViewData["CurrentCategory"] = categoryId;

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
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.Users = new SelectList(_userService.GetAll(), "Iduser", "Name");
            ViewBag.Venues = new SelectList(_venueService.GetAll(1, 100), "Idvenue", "VenueName");
            ViewBag.StatusList = GetStatusList();
            return View();
        }

        // POST: ReservationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(ReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Users = new SelectList(_userService.GetAll(), "Iduser", "Name", model.UserId);
                ViewBag.Venues = new SelectList(_venueService.GetAll(1, 100), "Idvenue", "VenueName", model.VenueId);
                ViewBag.StatusList = GetStatusList(model.Status);
                return View(model);
            }

            var reservation = _mapper.Map<Reservation>(model);
            _reservationService.Create(reservation);
            return RedirectToAction(nameof(Index));
        }

        // GET: ReservationController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var reservation = _reservationService.GetById(id);
            if (reservation == null) return NotFound();

            var model = _mapper.Map<ReservationViewModel>(reservation);

            ViewBag.Users = new SelectList(_userService.GetAll(), "Iduser", "Name", model.UserId);
            ViewBag.Venues = new SelectList(_venueService.GetAll(1, 100), "Idvenue", "VenueName", model.VenueId);
            ViewBag.StatusList = GetStatusList(model.Status);

            return View(model);
        }

        // POST: ReservationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, ReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Users = new SelectList(_userService.GetAll(), "Iduser", "Name", model.UserId);
                ViewBag.Venues = new SelectList(_venueService.GetAll(1, 100), "Idvenue", "VenueName", model.VenueId);
                ViewBag.StatusList = GetStatusList(model.Status);
                return View(model);
            }

            var updated = _mapper.Map<Reservation>(model);
            updated.Idreservation = id;

            var success = _reservationService.Update(id, updated);
            if (!success) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // GET: ReservationController/Delete/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var success = _reservationService.Delete(id);
            if (!success) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        private List<SelectListItem> GetStatusList(string? selected = null)
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem("Pending", "Pending"),
                new SelectListItem("Confirmed", "Confirmed"),
                new SelectListItem("Cancelled", "Cancelled")
            };

            if (!string.IsNullOrEmpty(selected))
            {
                foreach (var item in list)
                {
                    if (item.Value == selected)
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }

            return list;
        }
    }
}
