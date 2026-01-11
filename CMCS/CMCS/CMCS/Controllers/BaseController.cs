using CMCS.Models;
using CMCS.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMCS.Controllers
{
    // Generate by the OpenAI for ensure the user data always from the DB
    public abstract class BaseController : Controller
    {
        protected readonly IUserRepository _userRepository;

        protected BaseController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected User CurrentUser
        {
            get
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                    return null;

                return _userRepository.GetById(userId.Value);
            }
        }

        protected bool IsLoggedIn => CurrentUser != null;
    }
}
