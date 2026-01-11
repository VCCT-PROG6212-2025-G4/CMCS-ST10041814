using CMCS.Models;

namespace CMCS.ViewModels
{
    public class CreateClaimViewModel
    {
        // when lectuer or HR create the claim they must fill in those details
        // hour rate were default 0 if lectuer create the claim
        // HR can set up the hour rate initialy create
        public int? LecturerId { get; set; }
        public int StudentId { get; set; }

        public string Month { get; set; }
        public int Year { get; set; }
        public decimal HoursWorked { get; set; }
        public decimal HourRate { get; set; }

        public List<User> Lecturers { get; set; } = new();
        public List<User> Students { get; set; } = new();
    }
}
