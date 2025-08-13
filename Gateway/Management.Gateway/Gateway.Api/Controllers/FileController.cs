using Gateway.Application.Logging.Interfaces;
using Gateway.Infrastructure.Logging.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
   private readonly ILoggingService _loggerService;

   public FileController(ILoggingService loggerService)
   {
      _loggerService = loggerService;
   }
   
   [HttpGet]
   public async Task<IActionResult> Get()
   {
      try
      {
         var result = Enumerable.Range(0, 5).Select(_ => Guid.NewGuid().ToString("N")).ToArray();
         
         return Ok("Finishing process.");
      }
      catch (Exception ex)
      {
         _loggerService.LogAsync(new LogEntry()
         {
            Method = "POST",
            Path = HttpContext.Request.Path,
            QueryString = ex.Message,
            Timestamp = DateTime.UtcNow
         });
         
         return BadRequest(ex.Message);
      }
   }
   
   [HttpPost]
   public async Task<IActionResult> Post()
   {
      try
      {
         return Ok("Finishing process.");
      }
      catch (Exception ex)
      {
         _loggerService.LogAsync(new LogEntry()
         {
            Method = "POST",
            Path = HttpContext.Request.Path,
            QueryString = ex.Message,
            Timestamp = DateTime.UtcNow
         });
         
         return BadRequest(ex.Message);
      }
   }
}