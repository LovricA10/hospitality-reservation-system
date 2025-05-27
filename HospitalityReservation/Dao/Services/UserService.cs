using Dao.Models;
using Dao.Repositories;

namespace Dao.Services
{
    public class UserService
    {
        private readonly IRepo<User> _userRepo;

        public UserService(IRepo<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public User? GetByEmail(string email)
        {
            return (_userRepo as GenericRepo<User>)?.GetQueryable()
               .FirstOrDefault(u => u.Email == email);
        }

        public User? Create(User user)
        {
            _userRepo.Add(user);
            _userRepo.Save();
            return user;
        }
    }
}

