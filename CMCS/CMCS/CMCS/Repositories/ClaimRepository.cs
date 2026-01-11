using CMCS.Data;
using CMCS.Models;
using CMCS.Models.Enums;
using CMCS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Repositories
{
    /// <summary>
    /// to store delete, edit the claim in database
    /// </summary>
    public class ClaimRepository : IClaimRepository
    {
        //all code been checked and efficiency by the Chatgpt, original code was brainstorm and gide by the chat gpt
        private readonly AppDbContext _context;
        // constructure
        public ClaimRepository(AppDbContext context)
        {
            _context = context;
        }

        // get claim id with lecturer and user and check if the claim is need for later use
        public Claim GetById(int claimId)
        {
            return _context.Claims
                .Include(c => c.Lecturer)
                .Include(c => c.User)
                .FirstOrDefault(c => c.ClaimID == claimId);
        }
        // get all the claim and list
        public IEnumerable<Claim> GetAll()
        {
            return _context.Claims
                .Include(c => c.Lecturer)
                .Include(c => c.User)
                .ToList();
        }
        // select the claim where including the lecturer id
        public IEnumerable<Claim> GetByLecturerId(int lecturerId)
        {
            return _context.Claims
                .Include(c => c.User)
                .Where(c => c.LecturerID == lecturerId)
                .ToList();
        }
        // select the claim where including the user id
        public IEnumerable<Claim> GetByUserId(int userId)
        {
            return _context.Claims
                .Include(c => c.Lecturer)
                .Where(c => c.UserID == userId)
                .ToList();
        }
        // add the claim with detials
        public void Add(Claim claim)
        {
            _context.Claims.Add(claim);
        }
        // update the claim
        public void Update(Claim claim)
        {
            _context.Claims.Update(claim);
        }
        // remove the claim
        public void Delete(Claim claim)
        {
            _context.Claims.Remove(claim);
        }
        // refresh the database with new input
        public void Save()
        {
            _context.SaveChanges();
        }
        // for searching the claim where meet the requirement as a query
        public IQueryable<Claim> Query()
        {
            return _context.Claims
                .Include(c => c.User)
                .Include(c => c.Lecturer)
                .AsQueryable();
        }
    }
}
