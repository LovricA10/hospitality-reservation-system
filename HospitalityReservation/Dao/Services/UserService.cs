using Dao.Models;
using Dao.Repositories;
using Dao.Repositories.Users;

namespace Dao.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public User? GetByEmail(string email)
        {
            return _userRepo.GetByEmail(email);
        }

        public User? Create(User user)
        {
            _userRepo.Add(user);
            _userRepo.Save();
            return user;
        }
    }
}

