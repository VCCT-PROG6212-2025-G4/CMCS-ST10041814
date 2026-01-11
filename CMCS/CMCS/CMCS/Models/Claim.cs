using CMCS.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{
    public class Claim
    {
        [Key]
        public int ClaimID { get; set; }

        // Lecturer
        public int LecturerID { get; set; }
        public User Lecturer { get; set; }

        // Student / User
        public int UserID { get; set; }
        public User User { get; set; }

        public string Month { get; set; }
        public int Year { get; set; }

        public decimal HoursWorked { get; set; }
        public decimal HourRate { get; set; }

        // Coordinator
        public ClaimApprovalStatus ApprovalStatus { get; set; } = ClaimApprovalStatus.Pending; // Pending = 0, Approved = 1, Rejected = 2

        // Manager
        public ClaimPostStatus PostStatus { get; set; } = ClaimPostStatus.NotPosted; //NotPosted = 0, Posted = 1

        // User
        public ClaimPaymentStatus PaymentStatus { get; set; } = ClaimPaymentStatus.Unpaid; // Unpaid = 0, Paid = 1    

        public DateTime CreatedAt { get; set; }
    }
}