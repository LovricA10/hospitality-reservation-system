using Dao.Models;

namespace Dao.Repositories.Log
{
    public interface ILogRepository
    {
        void Add(LogEntry entry);
        void Save();
        IEnumerable<LogEntry> GetLastN(int count);
        int Count();
    }
}
