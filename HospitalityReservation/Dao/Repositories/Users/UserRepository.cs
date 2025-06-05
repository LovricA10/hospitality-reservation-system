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

        public IEnumerable<User> GetAll()
        => _context.Users.ToList();

        public User? GetById(int id)
        => _context.Users.FirstOrDefault(u => u.Iduser == id);

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }

        public User? GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Email == username);
        }

        public IQueryable<User> GetQueryable()
        {
            return _context.Users.AsQueryable();
        }
    }
}
