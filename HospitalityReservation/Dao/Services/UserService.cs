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

        public List<User> GetAll()
        {
            return _userRepo.GetAll().ToList();
        }

        public User? GetById(int id)
        {
            return _userRepo.GetById(id);
        }

        public bool Update(int id, User updatedUser)
        {
            var existingUser = _userRepo.GetById(id);
            if (existingUser == null) return false;

            existingUser.Name = updatedUser.Name;
            existingUser.Email = updatedUser.Email;
            existingUser.PwdHash = updatedUser.PwdHash;
            existingUser.PwdSalt = updatedUser.PwdSalt;
            existingUser.Role = updatedUser.Role;

            _userRepo.Update(existingUser);
            _userRepo.Save();
            return true;
        }

        public bool Delete(int id)
        {
            var user = _userRepo.GetById(id);
            if (user == null) return false;

            _userRepo.Delete(user);
            _userRepo.Save();
            return true;
        }
    }
}

