using Dao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Repositories.HospitalityTypes
{
    public class HospitalityTypeRepository : IHospitalityTypeRepository
    {
        private readonly HospitalityReservationDbContext _context;
        public HospitalityTypeRepository(HospitalityReservationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<HospitalityType> GetAll() => _context.HospitalityTypes.ToList();
        public HospitalityType? GetById(int id) => _context.HospitalityTypes.Find(id);
        public void Add(HospitalityType type) => _context.HospitalityTypes.Add(type);
        public void Update(HospitalityType type) => _context.HospitalityTypes.Update(type);
        public void Delete(HospitalityType type) => _context.HospitalityTypes.Remove(type);
        public void Save() => _context.SaveChanges();
    }
}
