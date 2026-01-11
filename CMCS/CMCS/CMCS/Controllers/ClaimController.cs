using CMCS.Models;
using CMCS.Models.Enums;
using CMCS.Repositories;
using CMCS.Repositories.Interfaces;
using CMCS.Services.Interfaces;
using CMCS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CMCS.Controllers
{
    // all code been checked and efficiency by the Chatgpt, original code was brainstorm and gide by the chat gpt
    public class ClaimController : BaseController
    {

        //construct, get all the require server and repository 
        private readonly IClaimService _claimService;
        private readonly IClaimRepository _claimRepository;
        private readonly IClaimFileService _fileService;
        private readonly IAuthorizationService _authService;

        public ClaimController(
            IUserRepository userRepository,
            IClaimService claimService,
            IClaimRepository claimRepository,
            IClaimFileService fileService,
            IAuthorizationService authService)
            : base(userRepository)
        {
            _claimService = claimService;
            _claimRepository = claimRepository;
            _fileService = fileService;
            _authService = authService;
        }

        // Claim list (shared view)
        [HttpGet]
        public IActionResult Index(ClaimSearchViewModel filter)
        {
            // authorized user
            if (!IsLoggedIn)
                return RedirectToAction("Login", "Account");
            // Retrieve claims relevant to the current user
            var claims = filter == null
                ? _claimService.GetClaimsForUser(CurrentUser)
                : _claimService.SearchClaims(CurrentUser, filter);
            // get the group claim
            var groups = BuildGroups(claims, CurrentUser.Role);
            return View(groups);
        }

        // get the claim by grouping in each user's claim
        private List<ClaimGroupViewModel> BuildGroups(
    IEnumerable<Claim> claims,
    string role)
        {
            // get user role, then find the related claim and seperate the claim by the another related user
            // Lecturer: group by Student
            if (role == "Lecturer")
            {
                return claims
                    // find if there is claim exist
                    .Where(c => c.User != null)
                    .GroupBy(c => c.UserID)
                    // use the view model support the output view
                    .Select(g => new ClaimGroupViewModel
                    {
                        OwnerId = g.Key,
                        OwnerName = g.First().User.Username,
                        Claims = g
                    }).ToList();
            }

            // Student/User: group by Lecturer
            if (role == "User")
            {
                return claims
                    .Where(c => c.Lecturer != null)
                    .GroupBy(c => c.LecturerID)
                    .Select(g => new ClaimGroupViewModel
                    {
                        OwnerId = g.Key,
                        OwnerName = g.First().Lecturer.Username,
                        Claims = g
                    }).ToList();
            }

            // Manager / HR / Coordinator
            // group ALL claims by Lecturer
            if (role == "Manager" || role == "HR" || role == "Coordinator")
            {
                return claims
                    .Where(c => c.Lecturer != null)
                    .GroupBy(c => c.LecturerID)
                    .Select(g => new ClaimGroupViewModel
                    {
                        OwnerId = g.Key,
                        OwnerName = g.First().Lecturer.Username,
                        Claims = g
                    }).ToList();
            }

            // must not return this
            return new();
        }

        // create claim view 
        [HttpGet]
        public IActionResult Create()
        {
            // check user
            if (!IsLoggedIn)
                return RedirectToAction("Login", "Account");

            var vm = new CreateClaimViewModel();

            // if the user is HR then require to select the lecturer to create the claim
            // lecuter create the claim with their id, therefore no need for select the lecturer
            if (CurrentUser.Role == "HR")
            {
                // get all the lecturer role user in the list
                vm.Lecturers = _userRepository.GetAll()
                    .Where(u => u.Role == "Lecturer")
                    .ToList();
            }
            // get all the user for selecting in the claim create
            vm.Students = _userRepository.GetAll()
                .Where(u => u.Role == "User")
                .ToList();

            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(CreateClaimViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            // add the new varaible , then save it in the db later
            var claim = new Claim
            {
                // if the user is hr then get the lecturer id or use the current user id as only lecturer create the claim
                LecturerID = CurrentUser.Role == "HR"
                    ? vm.LecturerId!.Value
                    : CurrentUser.Id,

                UserID = vm.StudentId,
                Month = vm.Month,
                Year = vm.Year,
                HoursWorked = vm.HoursWorked,
                HourRate = vm.HourRate,
                CreatedAt = DateTime.Now
            };
            // save the claim with all the data, when lecturer create the hour rate is 0, and coordinator or HR can edit the value
            _claimService.CreateClaim(claim, CurrentUser);
            // return to the index of claim for user
            return RedirectToAction("Index");
        }

        // DETAILS
        public IActionResult Details(int id)
        {
            // check current user role
            if (!IsLoggedIn)
                return RedirectToAction("Login", "Account");
            // get the id from the view full information of the item
            var claim = _claimService.GetClaimDetail(id, CurrentUser);
            // call the files by api
            ViewBag.Files = _fileService.GetFiles(id);
            return View(claim);
        }

        // UPDATE Coordinator / HR only
        [HttpPost]
        public IActionResult Update(int id, Claim input)
        {
            // try to get the claim by the claim id
            var claim = _claimRepository.GetById(id);

            if (claim == null)
                return NotFound();

            // if the claim is post then block the user edit the claim
            if (claim.PostStatus == Models.Enums.ClaimPostStatus.Posted ||
                claim.PaymentStatus == Models.Enums.ClaimPaymentStatus.Paid)
                return BadRequest("Claim cannot edit after post.");

            if (CurrentUser.Role != "Coordinator" && CurrentUser.Role != "HR")
                return Unauthorized();

            // If content changes then PDF must be removed
            foreach (var f in _fileService.GetFiles(id).Where(f => f.EndsWith(".pdf")))
            {
                _fileService.Delete(id, f);
            }
            // only the date and amount can be changed the user id and role cannot changed
            claim.Month = input.Month;
            claim.Year = input.Year;
            claim.HoursWorked = input.HoursWorked;
            claim.HourRate = input.HourRate;

            // save the new edit form in the database
            _claimRepository.Update(claim);
            _claimRepository.Save();
            // renew the page
            return RedirectToAction("Details", new { id });
        }
    }
}