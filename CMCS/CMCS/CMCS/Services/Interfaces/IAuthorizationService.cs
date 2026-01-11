using CMCS.Models;

namespace CMCS.Services.Interfaces
{
    public interface IAuthorizationService
    {
        bool CanViewClaim(Claim claim, User user);
        bool CanUploadFile(Claim claim, User user);
        bool CanDeleteFile(Claim claim, User user);
        bool CanGenerateInvoice(User user);
        bool CanViewFile(Claim claim, User user);

    }
}
