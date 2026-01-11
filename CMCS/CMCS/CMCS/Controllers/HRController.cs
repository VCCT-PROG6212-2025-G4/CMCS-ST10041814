using CMCS.Models;
using CMCS.Repositories.Interfaces;
using CMCS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CMCS.Controllers
{
    //most of the code been checked and efficiency by the Chatgpt, original code was brainstorm and gide by the chat gpt, only one methods gnerated by AI
    public class HRController : BaseController
    {
        // use standard MVC controller instead of API controller because this view is HR-only

        private readonly IUserRepository _userRepo;

        public HRController(IUserRepository userRepository)
            : base(userRepository)
        {
            // this HRController is created by AI
            // Generate by the OpenAI for ensure the user data always from the DB
            // Opent ai suggest basecontroller to protect the HR control session
            _userRepo = userRepository;
        }

        // LIST USERS
        public IActionResult Index()
        {
            // check authentic user is HR only
            if (!IsLoggedIn || CurrentUser.Role != "HR")
                return Unauthorized();
            // get all the users 
            var users = _userRepo.GetAll();
            return View(users);
        }

        // EDIT 
        public IActionResult Edit(int id)
        {
            // when user click edit button
            if (!IsLoggedIn || CurrentUser.Role != "HR")
                return Unauthorized();
            // get the selected user id and check if it is still available 
            var user = _userRepo.GetById(id);
            if (user == null) return NotFound();

            //input user data in the view model as selected user
            var vm = new EditUserViewModel
            {
                Id = user.Id,
                UserCode = user.UserCode,
                Username = user.Username,
                Role = user.Role,
                Email = user.Email,
                ContactNumber = user.ContactNumber
            };
            //go to the edit view 
            return View(vm);
        }

        // EDIT 
        [HttpPost]
        public IActionResult Edit(EditUserViewModel vm)
        {
            // HR can only edit sub data, or the user name might repeat
            // and the claim might be crash or require new change of claim details as new user role or name
            // therefore only the email, phone number and password can be changed
            if (!IsLoggedIn || CurrentUser.Role != "HR")
                return Unauthorized();

            var user = _userRepo.GetById(vm.Id);
            if (user == null) return NotFound();

            // save data in the variables 
            user.Email = vm.Email;
            user.ContactNumber = vm.ContactNumber;
            // check if the password is not null or space 
            if (!string.IsNullOrWhiteSpace(vm.NewPassword))
            {
                user.Password = vm.NewPassword; 
            }
            // save data in actual database
            _userRepo.Update(user);
            _userRepo.Save();

            return RedirectToAction("Index");
        }

        // DELETE
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!IsLoggedIn || CurrentUser.Role != "HR")
                return Unauthorized();

            var user = _userRepo.GetById(id);
            if (user == null) return NotFound();
            // sent request repository buildin delete logic 
            _userRepo.Delete(user);
            _userRepo.Save(); // cascade deletes claims

            return RedirectToAction("Index");
        }
    }
}
