using Dao.Models;
using Dao.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dao.Services
{
    public class ReservationService
    {
        private readonly IRepo<Reservation> _reservationRepo;
        private readonly HospitalityReservationDbContext _context;

        public ReservationService(IRepo<Reservation> reservationRepo, HospitalityReservationDbContext context)
        {
            _reservationRepo = reservationRepo;
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
            _reservationRepo.Add(reservation);
            _reservationRepo.Save();
            return reservation;
        }

        public bool Update(int id, Reservation updated)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.Idreservation == id);
            if (reservation == null) return false;

            reservation.NumberOfGuests = updated.NumberOfGuests;
            reservation.Status = updated.Status;
            reservation.ReservationDate = updated.ReservationDate;
            _reservationRepo.Save();
            return true;
        }

        public bool Delete(int id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation == null) return false;

            _reservationRepo.Delete(reservation);
            _reservationRepo.Save();
            return true;
        }
    }
}
