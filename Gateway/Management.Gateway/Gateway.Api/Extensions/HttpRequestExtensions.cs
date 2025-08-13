using System.Text;

namespace Gateway.Api.Extensions;

public static class HttpRequestExtensions
{
    public static async Task<string> ReadBodyAsStringAsync(this HttpRequest request)
    {
        if (request.Body == null || !request.ContentLength.HasValue || request.ContentLength == 0)
            return "No request body given";

        request.EnableBuffering();

        if (!request.ContentLength.HasValue || request.ContentLength == 0)
        {
            request.Body.Position = 0;
            return "No request body given (content length is zero or missing)";
        }
        
        using var reader = new StreamReader(
            request.Body,
            encoding: Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            bufferSize: 1024,
            leaveOpen: true);

        string body = await reader.ReadToEndAsync();

        request.Body.Position = 0;

        return body;
    }
}