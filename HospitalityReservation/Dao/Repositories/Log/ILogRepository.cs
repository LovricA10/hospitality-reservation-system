using Dao.Models;

namespace Dao.Repositories.Log
{
    public interface ILogRepository
    {
        void Add(LogEntry entry);
        void Save();
        IEnumerable<LogEntry> GetLastN(int count);
        int Count();
        IEnumerable<LogEntry> GetAll();
        LogEntry? GetById(int id);
        IQueryable<LogEntry> GetQueryable();
    }
}
