using Dao.Models;
using Dao.Repositories;
using Dao.Repositories.Log;

namespace Dao.Services
{
    public class LogService
    {
        private readonly ILogRepository _logRepo;

        public LogService(ILogRepository logRepo)
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
            return _logRepo.GetLastN(count);
        }

        public int Count()
        {
            return _logRepo.Count();
        }

        public IEnumerable<LogEntry> GetAll()
        {
            return _logRepo.GetAll();
        }

        public LogEntry? GetById(int id)
        {
            return _logRepo.GetById(id);
        }
    }
}

