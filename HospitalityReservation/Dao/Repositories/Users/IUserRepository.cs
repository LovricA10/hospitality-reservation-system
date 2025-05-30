using Dao.Models;

namespace Dao.Repositories.Users
{
    public interface IUserRepository
    {
        User? GetByEmail(string email);
        void Add(User user);
        void Save();
    }
}
