using Gateway.Infrastructure.Logging.Dtos;
using Gateway.Infrastructure.Logging.Interfaces;

namespace Gateway.Infrastructure.Logging;

public class MongoLogger : IMongoLogger
{
    public Task LogAsync(LogEntry entry)
    {
        throw new NotImplementedException();
    }
}