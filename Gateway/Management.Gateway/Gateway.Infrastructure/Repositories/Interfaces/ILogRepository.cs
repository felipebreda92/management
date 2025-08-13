using Gateway.Infrastructure.Logging.Dtos;

namespace Gateway.Infrastructure.Repositories.Interfaces;

public interface ILogRepository
{
    Task AddAsync(LogEntry logEntry);
}