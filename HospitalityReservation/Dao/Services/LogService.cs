using Dao.Models;
using Dao.Repositories;

namespace Dao.Services
{
    public class LogService
    {
        private readonly IRepo<LogEntry> _logRepo;

        public LogService(IRepo<LogEntry> logRepo)
        {
            _logRepo = logRepo;
        }

        public LogEntry Log(string message, int level = 1)
        {
            var entry = new LogEntry
            {
                Message = message,
                Level = level,
                Timestamp = DateTime.UtcNow
            };

            _logRepo.Add(entry);
            _logRepo.Save();
            return entry;
        }

        public IEnumerable<LogEntry> GetLastN(int count)
        {
            return _logRepo.GetQueryable()
                .OrderByDescending(l => l.Timestamp)
                .Take(count)
                .ToList();
        }

        public int Count() => _logRepo.GetQueryable().Count();
    }
}

