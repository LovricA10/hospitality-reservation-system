using Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace Dao.Services
{
    public class MenuService
    {
        private readonly HospitalityReservationDbContext _context;

        public MenuService(HospitalityReservationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<MenuItem> GetAll(int? venueId = null, int page = 1, int pageSize = 10)
        {
            var query = _context.VenueMenuItems
                .Include(vm => vm.MenuItem)
                .AsQueryable();

            if (venueId.HasValue)
                query = query.Where(vm => vm.VenueId == venueId);

            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(vm => vm.MenuItem)
                .ToList();
        }

        public MenuItem? GetById(int id)
        {
            return _context.MenuItems
                .Include(m => m.VenueMenuItems)
                .FirstOrDefault(m => m.IdmenuItem == id);
        }

        public MenuItem? Create(MenuItem item, int venueId)
        {
            _context.MenuItems.Add(item);
            _context.SaveChanges();

            var link = new VenueMenuItem
            {
                MenuItemId = item.IdmenuItem,
                VenueId = venueId
            };

            _context.VenueMenuItems.Add(link);
            _context.SaveChanges();

            return item;
        }

        public bool Update(int id, MenuItem updated, int venueId)
        {
            var item = _context.MenuItems
                .Include(m => m.VenueMenuItems)
                .FirstOrDefault(m => m.IdmenuItem == id);

            if (item == null || !item.VenueMenuItems.Any(vm => vm.VenueId == venueId)) return false;

            item.ItemName = updated.ItemName;
            item.ItemType = updated.ItemType;
            item.Price = updated.Price;
            _context.SaveChanges();

            return true;
        }

        public bool Delete(int id, int venueId)
        {
            var item = _context.MenuItems
                .Include(m => m.VenueMenuItems)
                .FirstOrDefault(m => m.IdmenuItem == id);

            if (item == null) return false;

            var link = item.VenueMenuItems.FirstOrDefault(vm => vm.VenueId == venueId);
            if (link == null) return false;

            _context.VenueMenuItems.Remove(link);

            if (item.VenueMenuItems.Count == 1)
                _context.MenuItems.Remove(item);

            _context.SaveChanges();
            return true;
        }
    }
}
