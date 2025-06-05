using AutoMapper;
using Dao.Models;
using Dao.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;
using System.Security.Claims;
using MVC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Controllers
{
    [Authorize]
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
        // GET: UserController
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string? q, string? categoryId)
        {
            var users = _userService.GetAll();

            if (!string.IsNullOrWhiteSpace(q))
            {
                users = users.Where(u =>
                    (!string.IsNullOrWhiteSpace(u.Name) && u.Name.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(u.LastName) && u.LastName.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(u.Email) && u.Email.Contains(q, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                users = users.Where(u => u.Role != null && u.Role.Equals(categoryId, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var model = _mapper.Map<List<UserViewModel>>(users);

            var roles = new List<SelectListItem>
            {
                new SelectListItem("User", "User"),
                new SelectListItem("Admin", "Admin")
            };

                ViewBag.CategoryList = new SelectList(roles, "Value", "Text");
                ViewData["CurrentFilter"] = q;
                ViewData["CurrentCategory"] = categoryId;

                return View(model);
        }


        // GET: UserController/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            var user = _userService.GetById(id);
            if (user == null) return NotFound();

            var model = _mapper.Map<UserViewModel>(user);
            return View(model);
        }

        // GET: UserController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(UserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _mapper.Map<User>(model);
            _userService.Create(user);

            return RedirectToAction(nameof(Index));
        }

        // GET: UserController/Edit/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            var success = _userService.Delete(id);
            if (!success) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginVM { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _userService.GetByUsername(model.Email);
            if (user == null || PasswordHashProvider.GetHash(model.Password, user.PwdSalt) != user.PwdHash)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.Role, user.Role ?? "User")
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Redirect(model.ReturnUrl ?? "/");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            //if (!ModelState.IsValid) return View(model);
            if (!ModelState.IsValid)
            {
                // DEBUG
                var allErrors = ModelState.Values.SelectMany(v => v.Errors).ToList();
                foreach (var err in allErrors)
                {
                    Console.WriteLine(err.ErrorMessage);
                }

                return View(model);
            }


            var salt = PasswordHashProvider.GenerateSalt();
            var hash = PasswordHashProvider.GetHash(model.Password, salt);

            var user = new User
            {
                Email = model.Email,
                Name = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                PwdSalt = salt,
                PwdHash = hash,
                Role = "User"
            };

            _userService.Create(user);
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var email = User.Identity.Name;
            var user = _userService.GetByEmail(email!);
            if (user == null) return NotFound();

            var model = new UserProfileVM
            {
                Email = user.Email,
                FirstName = user.Name,
                LastName = user.LastName,
                Phone = user.Phone
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Profile(UserProfileVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _userService.GetByEmail(User?.Identity?.Name!);
            if (user == null) return NotFound();

            user.Name = model.FirstName;
            user.LastName = model.LastName;
            user.Phone = model.Phone;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var salt = PasswordHashProvider.GenerateSalt();
                var hash = PasswordHashProvider.GetHash(model.Password, salt);
                user.PwdSalt = salt;
                user.PwdHash = hash;
            }

            _userService.Update(user.Iduser, user);
            ViewBag.Message = "Profile updated successfully.";
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Forbidden()
        {
            return View();
        }
    }
}
