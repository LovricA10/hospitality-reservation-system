using Dao.Models;
using Dao.Repositories;
using Dao.Repositories.Menu;
using Dao.Repositories.VenueMenu;
using Microsoft.EntityFrameworkCore;

namespace Dao.Services
{
    public class MenuService
    {
        private readonly IMenuRepository _menuRepo;
        private readonly IVenueMenuRepository _venueMenuRepo;

        public MenuService(IMenuRepository menuRepo, IVenueMenuRepository venueMenuRepo)
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
            return _menuRepo.GetById(id);
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
            var item = _menuRepo.GetById(id);

            if (item == null) return false;

            var hasLink = _venueMenuRepo.GetQueryable()
                .Any(vm => vm.MenuItemId == id && vm.VenueId == venueId);

            if (!hasLink) return false;

            item.ItemName = updated.ItemName;
            item.ItemType = updated.ItemType;
            item.Price = updated.Price;

            _menuRepo.Update(item);
            _menuRepo.Save();

            return true;
        }

        public bool Delete(int id, int venueId)
        {
            var item = _menuRepo.GetById(id);
            if (item == null) return false;

            var link = _venueMenuRepo.GetQueryable()
                .FirstOrDefault(vm => vm.MenuItemId == id && vm.VenueId == venueId);

            if (link == null) return false;

            _venueMenuRepo.Delete(link);

            var totalLinks = _venueMenuRepo.GetQueryable()
                .Count(vm => vm.MenuItemId == id);

            if (totalLinks == 1)
                _menuRepo.Delete(item);

            _menuRepo.Save();
            _venueMenuRepo.Save();

            return true;
        }
    }
}
