using Gateway.Infrastructure.Logging.Dtos;

namespace Gateway.Application.Logging.Interfaces;

public interface ILoggingService
{
    Task LogAsync(LogEntry entry);
}