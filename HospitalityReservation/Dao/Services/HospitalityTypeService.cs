using Dao.Models;
using Dao.Repositories.HospitalityTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Services
{
    public class HospitalityTypeService
    {
        private readonly IHospitalityTypeRepository _repo;
        public HospitalityTypeService(IHospitalityTypeRepository repo)
        {
            _repo = repo;
        }

        public List<HospitalityType> GetAll() => _repo.GetAll().ToList();
        public HospitalityType? GetById(int id) => _repo.GetById(id);
        public void Create(HospitalityType type)
        {
            _repo.Add(type);
            _repo.Save();
        }
        public void Update(HospitalityType type)
        {
            _repo.Update(type);
            _repo.Save();
        }
        public void Delete(int id)
        {
            var existing = _repo.GetById(id);
            if (existing != null)
            {
                _repo.Delete(existing);
                _repo.Save();
            }
        }
    }
}
