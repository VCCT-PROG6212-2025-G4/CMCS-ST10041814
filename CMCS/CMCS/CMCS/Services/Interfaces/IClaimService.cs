using CMCS.Models;
using CMCS.Models.Enums;
using CMCS.ViewModels;

namespace CMCS.Services.Interfaces
{
    public interface IClaimService
    {
        // Lecturer
        void CreateClaim(Claim claim, User currentUser);

        // Coordinator
        public void SetApprovalStatus(int claimId, ClaimApprovalStatus status, User currentUser);

        // Manager
        void PostClaim(int claimId, User currentUser);

        // User
        void MarkAsPaid(int claimId, User currentUser);

        // Common
        Claim GetClaimDetail(int claimId, User currentUser);
        IEnumerable<Claim> GetClaimsForUser(User currentUser);
        IEnumerable<Claim> SearchClaims(
    User currentUser,
    ClaimSearchViewModel filter);


    }
}
