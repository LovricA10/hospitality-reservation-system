using Dao.Models;

namespace Dao.Services
{
    public class UserService
    {
        private readonly HospitalityReservationDbContext _context;

        public UserService(HospitalityReservationDbContext context)
        {
            _context = context;
        }

        public User? GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User? Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }
    }
}

