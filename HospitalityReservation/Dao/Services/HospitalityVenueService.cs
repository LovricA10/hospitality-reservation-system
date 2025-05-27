using Dao.Models;
using Dao.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dao.Services
{
    public class HospitalityVenueService
    {
        private readonly IRepo<HospitalityVenue> _venueRepo;
        private readonly HospitalityReservationDbContext _context;

        public HospitalityVenueService(IRepo<HospitalityVenue> venueRepo, HospitalityReservationDbContext context)
        {
            _venueRepo = venueRepo;
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
            _venueRepo.Add(venue);
            _venueRepo.Save();
            return venue;
        }

        public void Update(HospitalityVenue venue)
        {
            _venueRepo.Update(venue);
            _venueRepo.Save();
        }

        public void Delete(HospitalityVenue venue)
        {
            _venueRepo.Delete(venue);
            _venueRepo.Save();
        }

        public HospitalityType? GetHospitalityTypeById(int typeId)
        {
            return _context.HospitalityTypes.FirstOrDefault(h => h.Idtype == typeId);
        }
    }
}

