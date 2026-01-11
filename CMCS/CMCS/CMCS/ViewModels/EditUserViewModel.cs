using System.ComponentModel.DataAnnotations;

namespace CMCS.ViewModels
{
    public class EditUserViewModel
    {
        // provided basic details of the selecting user for HR
        // only the email, phone number and password is editable in view
        public int Id { get; set; }

        public string UserCode { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string ContactNumber { get; set; }

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
    }
}
