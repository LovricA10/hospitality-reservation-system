using Dao.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Repositories
{
    public class GenericRepo<T> : IRepo<T> where T : class
    {
        private readonly HospitalityReservationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepo(HospitalityReservationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll() => _dbSet.ToList();

        public T? GetById(int id) => _dbSet.Find(id);

        public void Add(T entity) => _dbSet.Add(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public void Save() => _context.SaveChanges();
        public IQueryable<T> GetQueryable() => _dbSet.AsQueryable();
    }
}
