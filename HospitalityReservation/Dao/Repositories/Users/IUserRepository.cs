using Dao.Models;

namespace Dao.Repositories.Users
{
    public interface IUserRepository
    {
        User? GetByEmail(string email);
        void Add(User user);
        void Save();
        IEnumerable<User> GetAll();
        User? GetById(int id);
        void Update(User user);
        void Delete(User user);
        User? GetByUsername(string username);
    }
}
