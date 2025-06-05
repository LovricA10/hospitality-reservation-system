using Dao.Models;

namespace Dao.Repositories.Log
{
    public class LogRepository : ILogRepository
    {
        private readonly HospitalityReservationDbContext _context;

        public LogRepository(HospitalityReservationDbContext context)
        {
            _context = context;
        }

        public void Add(LogEntry entry) => _context.Logs.Add(entry);
        public void Save() => _context.SaveChanges();

        public IEnumerable<LogEntry> GetLastN(int count) =>
            _context.Logs.OrderByDescending(l => l.Timestamp).Take(count).ToList();

        public int Count() => _context.Logs.Count();

        public IEnumerable<LogEntry> GetAll()
        => _context.Logs.OrderByDescending(l => l.Timestamp).ToList();
        
        public LogEntry? GetById(int id)
        => _context.Logs.FirstOrDefault(l => l.Id == id);

        public IQueryable<LogEntry> GetQueryable()
        {
            return _context.Logs.OrderByDescending(l => l.Timestamp).AsQueryable();
        }
    }
}

