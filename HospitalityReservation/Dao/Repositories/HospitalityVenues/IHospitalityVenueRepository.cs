using Dao.Models;

namespace Dao.Repositories.HospitalityVenues
{
    public interface IHospitalityVenueRepository
    {
        IEnumerable<HospitalityVenue> GetAll(int page, int pageSize);
        HospitalityVenue? GetById(int id);
        void Add(HospitalityVenue entity);
        void Update(HospitalityVenue entity);
        void Delete(HospitalityVenue entity);
        void Save();
        HospitalityType? GetHospitalityTypeById(int typeId);
        IQueryable<HospitalityVenue> GetQueryable();
    }
}
