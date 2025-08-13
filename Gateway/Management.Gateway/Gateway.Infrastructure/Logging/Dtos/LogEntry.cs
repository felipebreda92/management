namespace Gateway.Infrastructure.Logging.Dtos;

public class LogEntry
{
    public string Method { get; set; }
    public string Path { get; set; }
    public string QueryString { get; set; }
    public string Body { get; set; }
    public string Type { get; set; }
    public string StatusCode { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}