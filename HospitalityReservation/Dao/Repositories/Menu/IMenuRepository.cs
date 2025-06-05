using Dao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Repositories.Menu
{
    public interface IMenuRepository
    {
        IEnumerable<MenuItem> GetAll(int? venueId, int page, int pageSize);
        MenuItem? GetById(int id);
        void Add(MenuItem item);
        void Update(MenuItem item);
        void Delete(MenuItem item);
        void Save();
        IQueryable<MenuItem> GetQueryable(int? venueId = null);
    }
}
