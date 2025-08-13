using Gateway.Infrastructure.Logging.Dtos;
using Gateway.Infrastructure.Repositories.Interfaces;
using MongoDB.Driver;

namespace Gateway.Infrastructure.Repositories;

public class MongoLogRepository : IMongoLogRepository
{
    private readonly IMongoCollection<LogEntry> _collection;

    public MongoLogRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<LogEntry>("Gateway");
    }

    public async Task AddAsync(LogEntry logEntry)
    {
        await _collection.InsertOneAsync(logEntry);
    }
}