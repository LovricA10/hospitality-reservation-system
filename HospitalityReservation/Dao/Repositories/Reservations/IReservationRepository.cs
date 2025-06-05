using Dao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Repositories.Reservations
{
    public interface IReservationRepository
    {
        IEnumerable<Reservation> GetAll(int page, int pageSize);
        Reservation? GetById(int id);
        void Add(Reservation reservation);
        void Update(Reservation reservation);
        void Delete(Reservation reservation);
        void Save();
        IQueryable<Reservation> GetQueryable();
    }
}
