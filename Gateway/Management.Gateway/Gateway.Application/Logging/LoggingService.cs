using Gateway.Application.Logging.Interfaces;
using Gateway.Infrastructure.Logging.Dtos;
using Gateway.Infrastructure.Repositories.Interfaces;

namespace Gateway.Application.Logging;

public class LoggingService : ILoggingService
{
    private readonly ILogRepository _logRepository;

    public LoggingService(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public Task LogAsync(LogEntry entry)
    {
        // aqui pode ter regras de negócio se precisar
        return _logRepository.AddAsync(entry);
    }
}