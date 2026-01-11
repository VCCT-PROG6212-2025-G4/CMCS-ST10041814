using Microsoft.AspNetCore.Mvc;

namespace CMCS.Controllers
{
    //all code been checked and efficiency by the Chatgpt, original code was brainstorm and gide by the chat gpt
    public class HomeController : Controller
    {
        // lead different page depend on the role of current user
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");

            return role switch
            {
                "HR" => RedirectToAction("Index", "HR"),
                "Manager" => RedirectToAction("Index", "Claim"),
                "Coordinator" => RedirectToAction("Index", "Claim"),
                "Lecturer" => RedirectToAction("Index", "Claim"),
                "User" => RedirectToAction("Index", "Claim"),
                _ => RedirectToAction("Login", "Account")
            };
        }
    }
}
