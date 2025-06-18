using AutoMapper;
using Dao.Models;
using Dao.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantReservationSystemWebApp.ViewModels;

namespace RestaurantReservationSystemWebApp.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly ReservationService _reservationService;
        private readonly HospitalityVenueService _venueService;
        private readonly UserService _userService;
        private readonly LogService _logService;
        private readonly IMapper _mapper;

        public ReservationController(
            ReservationService reservationService,
            HospitalityVenueService venueService,
            UserService userService,
            LogService logService,
            IMapper mapper)
        {
            _reservationService = reservationService;
            _venueService = venueService;
            _userService = userService;
            _logService = logService;
            _mapper = mapper;
        }

        public ActionResult Index(string? q, string? categoryId, int page = 1, int pageSize = 10)
        {
            var query = _reservationService.GetAllQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(r =>
                    r.User != null && r.User.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    r.Venue != null && r.Venue.VenueName.Contains(q, StringComparison.OrdinalIgnoreCase)
                );
            }

            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                query = query.Where(r => r.Status != null && r.Status.Equals(categoryId, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = query.Count();
            var reservations = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
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
            ViewData["Page"] = page;
            ViewData["PageSize"] = pageSize;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var reservation = _reservationService.GetById(id);
            if (reservation == null) return NotFound();

            var model = _mapper.Map<ReservationViewModel>(reservation);
            return View(model);
        }


        public ActionResult Create()
        {
            ViewBag.Users = new SelectList(_userService.GetAll(), "Iduser", "Name");
            ViewBag.Venues = new SelectList(_venueService.GetAll(1, 100), "Idvenue", "VenueName");
            ViewBag.StatusList = GetStatusList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

            _logService.Log($"Reservation created for venue ID={reservation.VenueId} by user ID={reservation.UserId}.", 1);

            return RedirectToAction(nameof(Index));
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
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

            _logService.Log($"Reservation ID={id} updated by {User.Identity?.Name}.", 1);

            return RedirectToAction(nameof(Index));
        }
        public ActionResult Delete(int id)
        {
            var reservation = _reservationService.GetById(id);
            if (reservation == null) return NotFound();

            var model = _mapper.Map<ReservationViewModel>(reservation);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var success = _reservationService.Delete(id);
            if (!success) return NotFound();

            _logService.Log($"Reservation ID={id} deleted by {User.Identity?.Name}.", 1);

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
