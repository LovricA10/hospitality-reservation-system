using Dao.Models;

namespace Dao.Repositories.VenueMenu
{
    public class VenueMenuRepository : IVenueMenuRepository
    {
        private readonly HospitalityReservationDbContext _context;

        public VenueMenuRepository(HospitalityReservationDbContext context)
        {
            _context = context;
        }

        public void Add(VenueMenuItem item) => _context.VenueMenuItems.Add(item);
        public void Delete(VenueMenuItem item) => _context.VenueMenuItems.Remove(item);
        public IQueryable<VenueMenuItem> GetQueryable() => _context.VenueMenuItems.AsQueryable();
        public void Save() => _context.SaveChanges();
    }
}
