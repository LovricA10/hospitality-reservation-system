using Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace Dao.Services
{
    public class HospitalityVenueService
    {
        private readonly HospitalityReservationDbContext _context;

        public HospitalityVenueService(HospitalityReservationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<HospitalityVenue> GetAll(int page, int pageSize)
        {
            return _context.HospitalityVenues
                .Include(v => v.Type)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public HospitalityVenue? GetById(int id)
        {
            return _context.HospitalityVenues
                .Include(v => v.Type)
                .FirstOrDefault(v => v.Idvenue == id);
        }

        public HospitalityVenue Create(HospitalityVenue venue)
        {
            _context.HospitalityVenues.Add(venue);
            _context.SaveChanges();
            return venue;
        }

        public void Update(HospitalityVenue venue)
        {
            _context.SaveChanges();
        }

        public void Delete(HospitalityVenue venue)
        {
            _context.HospitalityVenues.Remove(venue);
            _context.SaveChanges();
        }

        public HospitalityType? GetHospitalityTypeById(int typeId)
        {
            return _context.HospitalityTypes.FirstOrDefault(h => h.Idtype == typeId);
        }
    }
}

