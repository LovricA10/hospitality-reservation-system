using Dao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Repositories.VenueMenu
{
    public interface IVenueMenuRepository
    {
        void Add(VenueMenuItem item);
        void Delete(VenueMenuItem item);
        IQueryable<VenueMenuItem> GetQueryable();
        void Save();
    }
}
