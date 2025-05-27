using Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace Dao.Services
{
    public class ReservationService
    {
        private readonly HospitalityReservationDbContext _context;

        public ReservationService(HospitalityReservationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Reservation> GetAll(int page = 1, int pageSize = 10)
        {
            return _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Venue)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public Reservation? GetById(int id)
        {
            return _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Venue)
                .FirstOrDefault(r => r.Idreservation == id);
        }

        public Reservation? Create(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            return reservation;
        }

        public bool Update(int id, Reservation updated)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.Idreservation == id);
            if (reservation == null) return false;

            reservation.NumberOfGuests = updated.NumberOfGuests;
            reservation.Status = updated.Status;
            reservation.ReservationDate = updated.ReservationDate;
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation == null) return false;

            _context.Reservations.Remove(reservation);
            _context.SaveChanges();
            return true;
        }
    }
}
