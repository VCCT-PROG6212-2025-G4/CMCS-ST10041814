using CMCS.Models;
using CMCS.Models.Enums;
using CMCS.Repositories.Interfaces;
using CMCS.Services.Interfaces;
using CMCS.ViewModels;

namespace CMCS.Services
{
    //all code been checked and efficiency by the Chatgpt, original code was brainstorm and gide by the chat gpt
    public class ClaimService : IClaimService
    {
        // handle the logic behind the api call
        private readonly IClaimRepository _claimRepository;

        public ClaimService(IClaimRepository claimRepository)
        {
            _claimRepository = claimRepository;
        }

        // Lecturer / HR: Create Claim
        public void CreateClaim(Claim claim, User currentUser)
        {
            // set up the default status for each new claim and save it in the database
            if (currentUser.Role != "Lecturer" && currentUser.Role != "HR")
                throw new UnauthorizedAccessException();

            claim.ApprovalStatus = ClaimApprovalStatus.Pending;
            claim.PostStatus = ClaimPostStatus.NotPosted;
            claim.PaymentStatus = ClaimPaymentStatus.Unpaid;
            claim.CreatedAt = DateTime.Now;

            _claimRepository.Add(claim);
            _claimRepository.Save();
        }

        // Coordinator / HR: Approve / Reject / Pending
        public void SetApprovalStatus(
            int claimId,
            ClaimApprovalStatus status,
            User currentUser)
        {
            if (currentUser.Role != "Coordinator" && currentUser.Role != "HR")
                throw new UnauthorizedAccessException();

            var claim = _claimRepository.GetById(claimId)
                ?? throw new Exception("Claim not found.");
            // change and save the new status in the database 
            claim.ApprovalStatus = status;

            _claimRepository.Update(claim);
            _claimRepository.Save();
        }

        // Manager / HR: Post Claim
        public void PostClaim(int claimId, User currentUser)
        {
            if (currentUser.Role != "Manager" && currentUser.Role != "HR")
                throw new UnauthorizedAccessException();

            var claim = _claimRepository.GetById(claimId)
                ?? throw new Exception("Claim not found.");
            // check the user post the approved claim then post
            if (claim.ApprovalStatus != ClaimApprovalStatus.Approved
                && currentUser.Role != "HR")
                throw new InvalidOperationException("Claim must be approved.");
            if (claim.ApprovalStatus == ClaimApprovalStatus.Pending)
                claim.ApprovalStatus = ClaimApprovalStatus.Approved;
            claim.PostStatus = ClaimPostStatus.Posted;

            _claimRepository.Update(claim);
            _claimRepository.Save();
        }

        // User / HR: Pay
        public void MarkAsPaid(int claimId, User user)
        {
            if (user.Role != "HR" && user.Role != "User")
                throw new UnauthorizedAccessException();

            var claim = _claimRepository.GetById(claimId)
                ?? throw new Exception("Claim not found");
            // after paid the payment cannot make again
            if (claim.PaymentStatus == ClaimPaymentStatus.Paid)
                throw new InvalidOperationException("Claim already paid.");

            // for HR to bypass the rules as approval and post when mark as paid
            if (user.Role == "HR")
            {
                claim.ApprovalStatus = ClaimApprovalStatus.Approved;
                claim.PostStatus = ClaimPostStatus.Posted;
            }
            else
            {
                if (claim.ApprovalStatus != ClaimApprovalStatus.Approved)
                    throw new InvalidOperationException("Claim not approved.");

                if (claim.PostStatus != ClaimPostStatus.Posted)
                    throw new InvalidOperationException("Claim not posted.");
            }

            claim.PaymentStatus = ClaimPaymentStatus.Paid;

            _claimRepository.Update(claim);
            _claimRepository.Save();
        }



        // View single claim
        public Claim GetClaimDetail(int claimId, User currentUser)
        {
            // return claim if user avaiable to view 
            var claim = _claimRepository.GetById(claimId)
                ?? throw new Exception("Claim not found.");

            if (currentUser.Role == "HR")
                return claim;

            if (currentUser.Role == "Lecturer" && claim.LecturerID == currentUser.Id)
                return claim;

            if (currentUser.Role == "User" && claim.UserID == currentUser.Id)
                return claim;

            if (currentUser.Role == "Coordinator" || currentUser.Role == "Manager")
                return claim;

            throw new UnauthorizedAccessException();
        }

        // Claim list
        public IEnumerable<Claim> GetClaimsForUser(User currentUser)
        {
            return currentUser.Role switch
            {
                // different formate for different user by using the repository
                "HR" => _claimRepository.GetAll(),

                "Lecturer" => _claimRepository.GetByLecturerId(currentUser.Id),

                "User" => _claimRepository.GetByUserId(currentUser.Id),

                "Coordinator" => _claimRepository.GetAll(),

                "Manager" => _claimRepository.GetAll(),

                _ => Enumerable.Empty<Claim>()
            };
        }


        public IEnumerable<Claim> SearchClaims(
     User currentUser,
     ClaimSearchViewModel filter)
        {
            var query = _claimRepository.Query();

            // only lecturuer and user can see their own claims 
            switch (currentUser.Role)
            {
                case "Lecturer":
                    query = query.Where(c => c.LecturerID == currentUser.Id);
                    break;

                case "User":
                    query = query.Where(c => c.UserID == currentUser.Id);
                    break;
            }
            // return something that is not null or space entered
            if (string.IsNullOrWhiteSpace(filter.Keyword))
                return query.ToList();

            var keyword = filter.Keyword.Trim();
            // check by user, lecturer name monthe , and years as the key word, and return the possible claims relate to the key word by the filter
            switch (filter.SearchBy)
            {
                case "UserId":
                    if (int.TryParse(keyword, out var userId))
                        query = query.Where(c => c.UserID == userId);
                    break;

                case "Username":
                    query = query.Where(c =>
                        c.User != null &&
                        c.User.Username.Contains(keyword));
                    break;

                case "Lecturer":
                    query = query.Where(c =>
                        c.Lecturer != null &&
                        c.Lecturer.Username.Contains(keyword));
                    break;

                case "Month":
                    query = query.Where(c => c.Month.Contains(keyword));
                    break;

                case "Year":
                    if (int.TryParse(keyword, out var year))
                        query = query.Where(c => c.Year == year);
                    break;

                default:
                    // search all related claim autoly
                    query = query.Where(c =>
                        (c.User != null && c.User.Username.Contains(keyword)) ||
                        (c.Lecturer != null && c.Lecturer.Username.Contains(keyword)) ||
                        c.Month.Contains(keyword) ||
                        c.UserID.ToString().Contains(keyword) ||
                        c.Year.ToString().Contains(keyword));
                    break;
            }
            // rerturn query in the view
            return query.ToList();
        }


    }
}
