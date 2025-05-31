using AutoMapper;
using Dao.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class LogEntryController : Controller
    {
        private readonly LogService _logService;
        private readonly IMapper _mapper;

        public LogEntryController(LogService logService, IMapper mapper)
        {
            _logService = logService;
            _mapper = mapper;
        }
        // GET: LogEntryController
        public ActionResult Index()
        {
            var logs = _logService.GetAll();
            var model = _mapper.Map<List<LogEntryViewModel>>(logs);
            return View(model);
        }

        // GET: LogEntryController/Details/5
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
