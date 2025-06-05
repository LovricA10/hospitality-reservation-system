using Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace Dao.Repositories.Menu
{
    public class MenuRepository : IMenuRepository
    {
        private readonly HospitalityReservationDbContext _context;

        public MenuRepository(HospitalityReservationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<MenuItem> GetAll(int? venueId, int page, int pageSize)
        {
            var query = _context.VenueMenuItems.Include(vm => vm.MenuItem).AsQueryable();
            if (venueId.HasValue)
                query = query.Where(vm => vm.VenueId == venueId);

            return query.Skip((page - 1) * pageSize).Take(pageSize).Select(vm => vm.MenuItem).ToList();
        }

        public MenuItem? GetById(int id) =>
            _context.MenuItems.Include(m => m.VenueMenuItems).FirstOrDefault(m => m.IdmenuItem == id);

        public void Add(MenuItem item) => _context.MenuItems.Add(item);
        public void Update(MenuItem item) => _context.MenuItems.Update(item);
        public void Delete(MenuItem item) => _context.MenuItems.Remove(item);
        public void Save() => _context.SaveChanges();

        public IQueryable<MenuItem> GetQueryable(int? venueId = null)
        {
            var query = _context.VenueMenuItems
                .Include(vm => vm.MenuItem)
                .AsQueryable();

            if (venueId.HasValue)
                query = query.Where(vm => vm.VenueId == venueId);

            return query.Select(vm => vm.MenuItem);
        }
    }
}
