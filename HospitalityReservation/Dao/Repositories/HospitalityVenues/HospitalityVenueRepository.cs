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
        public void Update(HospitalityVenue entity)
        {
            var local = _context.HospitalityVenues.Local.FirstOrDefault(v => v.Idvenue == entity.Idvenue);
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }
            _context.HospitalityVenues.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(HospitalityVenue entity) => _context.HospitalityVenues.Remove(entity);
        public void Save() => _context.SaveChanges();
        public HospitalityType? GetHospitalityTypeById(int typeId) =>
            _context.HospitalityTypes.FirstOrDefault(h => h.Idtype == typeId);

        public IQueryable<HospitalityVenue> GetQueryable() => _context.HospitalityVenues.Include(v => v.Type).AsQueryable();

        public IEnumerable<Reservation> GetReservationsByVenueId(int venueId) =>
         _context.Reservations.Where(r => r.VenueId == venueId).ToList();

        public IEnumerable<MenuItem> GetMenuItemsByVenueId(int venueId) =>
            _context.VenueMenuItems
                .Include(vm => vm.MenuItem)
                .Where(vm => vm.VenueId == venueId)
                .Select(vm => vm.MenuItem)
                .ToList();

        public void DeleteReservation(Reservation reservation) =>
            _context.Reservations.Remove(reservation);

        public void DeleteMenuItem(MenuItem item) =>
            _context.MenuItems.Remove(item);

        public void RemoveVenueMenuLink(int venueId, int menuItemId)
        {
            var link = _context.VenueMenuItems.FirstOrDefault(vm => vm.VenueId == venueId && vm.MenuItemId == menuItemId);
            if (link != null)
                _context.VenueMenuItems.Remove(link);
        }

    }
}
