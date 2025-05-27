using Dao.Models;
using Dao.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dao.Services
{
    public class MenuService
    {
        private readonly IRepo<MenuItem> _menuRepo;
        private readonly IRepo<VenueMenuItem> _venueMenuRepo;

        public MenuService(IRepo<MenuItem> menuRepo, IRepo<VenueMenuItem> venueMenuRepo)
        {
            _menuRepo = menuRepo;
            _venueMenuRepo = venueMenuRepo;
        }
        public IEnumerable<MenuItem> GetAll(int? venueId = null, int page = 1, int pageSize = 10)
        {
            var query = _venueMenuRepo.GetQueryable()
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
            return _menuRepo.GetQueryable()
                .Include(m => m.VenueMenuItems)
                .FirstOrDefault(m => m.IdmenuItem == id);
        }

        public MenuItem? Create(MenuItem item, int venueId)
        {
            _menuRepo.Add(item);
            _menuRepo.Save();

            var link = new VenueMenuItem
            {
                MenuItemId = item.IdmenuItem,
                VenueId = venueId
            };

            _venueMenuRepo.Add(link);
            _venueMenuRepo.Save();

            return item;
        }

        public bool Update(int id, MenuItem updated, int venueId)
        {
            var item = _menuRepo.GetQueryable()
                .Include(m => m.VenueMenuItems)
                .FirstOrDefault(m => m.IdmenuItem == id);

            if (item == null || !item.VenueMenuItems.Any(vm => vm.VenueId == venueId)) return false;

            item.ItemName = updated.ItemName;
            item.ItemType = updated.ItemType;
            item.Price = updated.Price;

            _menuRepo.Update(item);
            _menuRepo.Save();

            return true;
        }

        public bool Delete(int id, int venueId)
        {
            var item = _menuRepo.GetQueryable()
                .Include(m => m.VenueMenuItems)
                .FirstOrDefault(m => m.IdmenuItem == id);

            if (item == null) return false;

            var link = item.VenueMenuItems.FirstOrDefault(vm => vm.VenueId == venueId);
            if (link == null) return false;

            _venueMenuRepo.Delete(link);

            if (item.VenueMenuItems.Count == 1)
                _menuRepo.Delete(item);

            _menuRepo.Save();
            _venueMenuRepo.Save();

            return true;
        }
    }
}
