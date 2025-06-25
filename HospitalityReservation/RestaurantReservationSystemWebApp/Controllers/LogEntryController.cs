using AutoMapper;
using Dao.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantReservationSystemWebApp.ViewModels;

namespace RestaurantReservationSystemWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LogEntryController : Controller
    {
        private readonly LogService _logService;
        private readonly IMapper _mapper;

        public LogEntryController(LogService logService, IMapper mapper)
        {
            _logService = logService;
            _mapper = mapper;
        }
        public ActionResult Index(string? q, string? categoryId, int page = 1, int pageSize = 10)
        {
            var query = _logService.GetAllQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(l => l.Message != null && EF.Functions.Like(l.Message, $"%{q}%"));
            }

            if (!string.IsNullOrWhiteSpace(categoryId) && int.TryParse(categoryId, out var level))
            {
                query = query.Where(l => l.Level == level);
            }

            var totalCount = query.Count();
            var logs = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var model = _mapper.Map<List<LogEntryViewModel>>(logs);

            var levelOptions = new List<SelectListItem>
            {
                new("Info", "1"),
                new("Warning", "2"),
                new("Error", "3")
            };

            ViewBag.CategoryList = new SelectList(levelOptions, "Value", "Text");
            ViewData["CurrentFilter"] = q;
            ViewData["CurrentCategory"] = categoryId;
            ViewData["Page"] = page;
            ViewData["PageSize"] = pageSize;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(model);
        }


        public ActionResult Details(int id)
        {
            var log = _logService.GetById(id);
            if (log == null)
                return NotFound();

            var model = _mapper.Map<LogEntryViewModel>(log);
            return View(model);
        }
    }
}
