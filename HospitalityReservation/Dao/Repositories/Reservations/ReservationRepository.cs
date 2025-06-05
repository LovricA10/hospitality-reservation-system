using Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace Dao.Repositories.Reservations
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly HospitalityReservationDbContext _context;

        public ReservationRepository(HospitalityReservationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Reservation> GetAll(int page, int pageSize) =>
            _context.Reservations.Include(r => r.User).Include(r => r.Venue)
                .Skip((page - 1) * pageSize).Take(pageSize).ToList();

        public Dao.Models.Reservation? GetById(int id) =>
            _context.Reservations.Include(r => r.User).Include(r => r.Venue)
                .FirstOrDefault(r => r.Idreservation == id);

        public void Add(Reservation reservation) => _context.Reservations.Add(reservation);
        public void Update(Reservation reservation) => _context.Reservations.Update(reservation);
        public void Delete(Reservation reservation) => _context.Reservations.Remove(reservation);
        public void Save() => _context.SaveChanges();

        public IQueryable<Reservation> GetQueryable()
        {
            return _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Venue)
                .AsQueryable();
        }

    }
}
