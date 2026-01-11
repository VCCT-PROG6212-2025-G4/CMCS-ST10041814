namespace CMCS.Services.Interfaces
{
    public interface IClaimFileService
    {
        void Upload(int claimId, IFormFile file);
        IEnumerable<string> GetFiles(int claimId);
        void Delete(int claimId, string fileName);
    }
}
