using Dao.Models;

namespace Dao.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly HospitalityReservationDbContext _context;

        public UserRepository(HospitalityReservationDbContext context)
        {
            _context = context;
        }

        public User? GetByEmail(string email) =>
            _context.Users.FirstOrDefault(u => u.Email == email);

        public void Add(User user) => _context.Users.Add(user);
        public void Save() => _context.SaveChanges();
    }
}
