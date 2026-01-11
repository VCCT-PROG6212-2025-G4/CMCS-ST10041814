using CMCS.Models;

namespace CMCS.Repositories.Interfaces
{
    public interface IClaimRepository
    {
        Claim GetById(int claimId);

        IEnumerable<Claim> GetAll();

        IEnumerable<Claim> GetByLecturerId(int lecturerId);

        IEnumerable<Claim> GetByUserId(int userId);

        void Add(Claim claim);
        void Update(Claim claim);
        void Delete(Claim claim);

        void Save();

        IQueryable<Claim> Query();
    }
}
