using AutoMapper;
using Dao.Models;
using Dao.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public UserController(UserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        // GET: UserController
        public ActionResult Index()
        {
            var users = _userService.GetAll();
            var model = _mapper.Map<List<UserViewModel>>(users);
            return View(model);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            var user = _userService.GetById(id);
            if (user == null) return NotFound();

            var model = _mapper.Map<UserViewModel>(user);
            return View(model);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _mapper.Map<User>(model);
            _userService.Create(user);

            return RedirectToAction(nameof(Index));
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            var user = _userService.GetById(id);
            if (user == null) return NotFound();

            var model = _mapper.Map<UserViewModel>(user);
            return View(model);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var updated = _mapper.Map<User>(model);
            updated.Iduser = id;

            var success = _userService.Update(id, updated);
            if (!success) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            var user = _userService.GetById(id);
            if (user == null) return NotFound();

            var model = _mapper.Map<UserViewModel>(user);
            return View(model);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var success = _userService.Delete(id);
            if (!success) return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
