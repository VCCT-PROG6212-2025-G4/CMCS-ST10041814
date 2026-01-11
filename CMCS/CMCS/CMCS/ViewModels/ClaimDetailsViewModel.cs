using CMCS.Models;
using System.Collections.Generic;

namespace CMCS.ViewModels
{
    public class ClaimDetailsViewModel
    {
        // return the currrent claim details with file path
        public Claim Claim { get; set; }

        public string CurrentUserRole { get; set; }

        public bool CanApprove { get; set; }
        public bool CanReject { get; set; }
        public bool CanPost { get; set; }
        public bool CanPay { get; set; }
        public bool CanUploadFile { get; set; }
        public bool CanGenerateInvoice { get; set; }

        public List<string> Files { get; set; } = new();
    }
}
