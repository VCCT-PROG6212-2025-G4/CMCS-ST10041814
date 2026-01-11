using CMCS.Models.Enums;
using CMCS.Repositories.Interfaces;
using CMCS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMCS.Controllers.API
{
    //all code been checked and efficiency by the Chatgpt, original code was brainstorm and gide by the chat gpt
    [ApiController]
    [Route("api/claims")]
    public class ClaimApiController : BaseController
    {
        private readonly IClaimService _claimService;

        // constract
        public ClaimApiController(
            IUserRepository userRepository,
            IClaimService claimService)
            : base(userRepository)
        {
            _claimService = claimService;
        }
        // approve the claim
        [HttpPost("{id}/approve")]
        public IActionResult Approve(int id)
        {
            if (CurrentUser == null)
                return Unauthorized();
            _claimService.SetApprovalStatus(
                id,
                ClaimApprovalStatus.Approved,
                CurrentUser);

            return Ok();
        }
        // reject the claim
        [HttpPost("{id}/reject")]
        public IActionResult Reject(int id)
        {
            if (CurrentUser == null)
                return Unauthorized();
            _claimService.SetApprovalStatus(
                id,
                ClaimApprovalStatus.Rejected,
                CurrentUser);

            return Ok();
        }
        // post the claim
        [HttpPost("{id}/post")]
        public IActionResult Post(int id)
        {
            // require HR and manager approve to show
            try
            {
                _claimService.PostClaim(id, CurrentUser);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                // error handing
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                // ensure the user is authorized
                return Forbid();
            }
        }
        //mark the claim as payed
        [HttpPost("{id}/pay")]
        public IActionResult Pay(int id)
        {
            if (CurrentUser == null)
                return Unauthorized(); 
            _claimService.MarkAsPaid(id, CurrentUser);
            return Ok();
        }
    }
}
