using CMCS.Data;
using CMCS.Models;
using CMCS.Repositories.Interfaces;

namespace CMCS.Repositories
{
    public class UserRepository : IUserRepository
    {
        //all code been checked and efficiency by the Chatgpt, original code was brainstorm and gide by the chat gpt
        private readonly AppDbContext _context;
        //Constrcuture
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        // find the user by id
        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }
        // get the user code for login 
        public User GetByUserCode(string userCode)
        {
            return _context.Users.FirstOrDefault(u => u.UserCode == userCode);
        }
        // get all the user for HR
        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }
        // adding user with register 
        public void Add(User user)
        {
            _context.Users.Add(user);
        }
        // update user details
        public void Update(User user)
        {
            _context.Users.Update(user);
        }
        // remove users
        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }
        // refresh the user database
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
