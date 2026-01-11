using CMCS.Models;

namespace CMCS.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User GetById(int id);
        User GetByUserCode(string userCode);

        IEnumerable<User> GetAll();

        void Add(User user);
        void Update(User user);
        void Delete(User user);

        void Save();
    }
}
