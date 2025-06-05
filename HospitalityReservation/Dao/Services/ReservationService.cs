using Dao.Models;
using Dao.Repositories;
using Dao.Repositories.Reservations;
using Microsoft.EntityFrameworkCore;

namespace Dao.Services
{
    public class ReservationService
    {
        private readonly IReservationRepository _reservationRepo;

        public ReservationService(IReservationRepository reservationRepo)
        {
            _reservationRepo = reservationRepo;
        }

        public IEnumerable<Reservation> GetAll(int page = 1, int pageSize = 10)
        {
            return _reservationRepo.GetAll(page, pageSize);
        }

        public Reservation? GetById(int id)
        {
            return _reservationRepo.GetById(id);
        }

        public Reservation? Create(Reservation reservation)
        {
            _reservationRepo.Add(reservation);
            _reservationRepo.Save();
            return reservation;
        }

        public bool Update(int id, Reservation updated)
        {
            var reservation = _reservationRepo.GetById(id);
            if (reservation == null) return false;

            reservation.NumberOfGuests = updated.NumberOfGuests;
            reservation.Status = updated.Status;
            reservation.ReservationDate = updated.ReservationDate;

            _reservationRepo.Update(reservation);
            _reservationRepo.Save();
            return true;
        }

        public bool Delete(int id)
        {
            var reservation = _reservationRepo.GetById(id);
            if (reservation == null) return false;

            _reservationRepo.Delete(reservation);
            _reservationRepo.Save();
            return true;
        }
        public IQueryable<Reservation> GetAllQueryable()
        {
            return _reservationRepo.GetQueryable();
        }
    }
}
