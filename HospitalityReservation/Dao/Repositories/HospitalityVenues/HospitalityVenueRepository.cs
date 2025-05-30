using Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace Dao.Repositories.HospitalityVenues
{
    public class HospitalityVenueRepository : IHospitalityVenueRepository
    {
        private readonly HospitalityReservationDbContext _context;

        public HospitalityVenueRepository(HospitalityReservationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<HospitalityVenue> GetAll(int page, int pageSize) =>
             _context.HospitalityVenues.Include(v => v.Type)
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize).ToList();

        public HospitalityVenue? GetById(int id) =>
            _context.HospitalityVenues.Include(v => v.Type).FirstOrDefault(v => v.Idvenue == id);

        public void Add(HospitalityVenue entity) => _context.HospitalityVenues.Add(entity);
        public void Update(HospitalityVenue entity) => _context.HospitalityVenues.Update(entity);
        public void Delete(HospitalityVenue entity) => _context.HospitalityVenues.Remove(entity);
        public void Save() => _context.SaveChanges();
        public HospitalityType? GetHospitalityTypeById(int typeId) =>
            _context.HospitalityTypes.FirstOrDefault(h => h.Idtype == typeId);
    }
}
