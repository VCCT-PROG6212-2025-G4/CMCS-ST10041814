using CMCS.Models;
using CMCS.Services.Interfaces;

namespace CMCS.Services
{
    //all code been checked and efficiency by the Chatgpt, original code was brainstorm and gide by the chat gpt
    public class AuthorizationService : IAuthorizationService
    {
        // limite user in conotroller
        public bool CanViewClaim(Claim claim, User user)
        {
            // limite lecturer can only see their own claims
            // limite user aswell,
            // open for coordinator, manager and HR
            return user.Role switch
            {
                "HR" => true,
                "Lecturer" => claim.LecturerID == user.Id,
                "User" => claim.UserID == user.Id,
                "Coordinator" => true,
                "Manager" => true,
                _ => false
            };
        }
        // limirw user to upload files
        public bool CanUploadFile(Claim claim, User user)
        {
            // HR can upload files in any claims
            // Lecturer can only upload files to their own claims
            //user cannot upload
            return user.Role switch
            {
                "HR" => true,
                "Lecturer" => claim.LecturerID == user.Id,
                "User" => claim.UserID == user.Id,
                _ => false
            };
        }
        // limite who can delete the files
        public bool CanDeleteFile(Claim claim, User user)
        {
            return user.Role switch
            {
                //HR can delete any files
                //Lecturer can only delete their own files
                "HR" => true,
                "Lecturer" => claim.LecturerID == user.Id,
                _ => false
            };
        }
        // HR and manager can generate invoice
        public bool CanGenerateInvoice(User user)
        {
            return user.Role == "HR" || user.Role == "Manager";
        }

        // check who can view the or download the files
        public bool CanViewFile(Claim claim, User user)
        {
            // check if the claim and user exist
            if (claim == null || user == null)
                return false;
            // HR , mananger , coordinator can download the files
            // lectuer and user can only download their own files
            return user.Role switch
            {
                "HR" => true,
                "Manager" => true,
                "Coordinator" => true,
                "Lecturer" => claim.LecturerID == user.Id,
                "User" => claim.UserID == user.Id,
                _ => false
            };
        }

    }
}
