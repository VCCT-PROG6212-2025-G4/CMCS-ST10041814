using CMCS.Models;
namespace CMCS.ViewModels
{
    public class ClaimGroupViewModel
    {
        // provided basic ownership of claim in the index of claim view
        public string Title { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }

}
