using Gateway.Infrastructure.Logging.Dtos;

namespace Gateway.Infrastructure.Logging.Interfaces;

public interface IMongoLogger
{
    Task LogAsync(LogEntry entry);
}