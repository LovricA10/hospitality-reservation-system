using Dao.Models;
using Dao.Repositories;
using Dao.Repositories.HospitalityVenues;
using Microsoft.EntityFrameworkCore;

namespace Dao.Services
{
    public class HospitalityVenueService
    {
        private readonly IHospitalityVenueRepository _venueRepo;

        public HospitalityVenueService(IHospitalityVenueRepository venueRepo)
        {
            _venueRepo = venueRepo;
        }

        public IEnumerable<HospitalityVenue> GetAll(int page, int pageSize)
        {
            return _venueRepo.GetAll(page, pageSize);
        }

        public HospitalityVenue? GetById(int id)
        {
            return _venueRepo.GetById(id);
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
            return _venueRepo.GetHospitalityTypeById(typeId);
        }
        public IQueryable<HospitalityVenue> GetAllQueryable()
        {
            return _venueRepo.GetQueryable();
        }
    }
}

