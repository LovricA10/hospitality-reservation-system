using Dao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Repositories.HospitalityTypes
{
    public interface IHospitalityTypeRepository
    {
        IEnumerable<HospitalityType> GetAll();
        HospitalityType? GetById(int id);
        void Add(HospitalityType type);
        void Update(HospitalityType type);
        void Delete(HospitalityType type);
        void Save();

        IQueryable<HospitalityType> GetQueryable();
    }
}
