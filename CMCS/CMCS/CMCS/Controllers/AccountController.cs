using CMCS.Models;
using CMCS.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMCS.Controllers
{
    //all code been checked and efficiency by the Chatgpt, original code was brainstorm and gide by the chat gpt
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Login

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string userCode, string password)
        {
            var user = _userRepository.GetByUserCode(userCode);

            // check if the user and password is correct
            if (user == null || user.Password != password)
            {
                // error handing
                ViewBag.Error = "Invalid username or password";
                return View();
            }

            // sign in and session with role in page
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Role", user.Role);

            return RedirectToAction("Index", "Home");
        }

        // Register

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            // cannot create HR account
            if (model.Role == "HR")
            {
                ViewBag.Error = "HR accounts cannot be registered.";
                return View(model);
            }

            // UserCode repeat issues
            if (_userRepository.GetByUserCode(model.UserCode) != null)
            {
                ViewBag.Error = "User Code already exists.";
                return View(model);
            }

            // add user
            //model.Username = model.UserCode;
            _userRepository.Add(model);
            _userRepository.Save();

            // check role user
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            var currentRole = HttpContext.Session.GetString("Role");

            // not login yet
            if (currentUserId == null)
            {
                //set up the session with user
                HttpContext.Session.SetInt32("UserId", model.Id);
                HttpContext.Session.SetString("Role", model.Role);
                return RedirectToAction("Index", "Home");
            }

            // login with HR account then return to HR page to create new user
            if (currentRole == "HR")
            {
                return RedirectToAction("Register", "Account");
            }

            return RedirectToAction("Login", "Account");
        }



        // Logout

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
