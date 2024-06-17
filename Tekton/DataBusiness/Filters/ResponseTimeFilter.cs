using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BusinessLayer.Filters
{
    public class ResponseTimeFilter : IAsyncActionFilter
    {
        private readonly ILogger<ResponseTimeFilter> _logger;
        private readonly ILoggerFactory _loggerFactory;

        public ResponseTimeFilter(ILogger<ResponseTimeFilter> logger, ILoggerFactory loggerFactory)
        {
            _logger = logger;
            loggerFactory.CreateLogger<ResponseTimeFilter>();
            var folderLog = System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"AppLog\logTektonAPI.txt");
            loggerFactory.AddFile(folderLog, LogLevel.Critical, null, false, null, null, "{Timestamp:o} {Message}{NewLine}");
            _loggerFactory = loggerFactory;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            await next();

            stopwatch.Stop();

            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            var message =
                $"Request [{context.HttpContext.Request.Method}] {context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString} se ejecutó en {elapsedMilliseconds} milisegundos";

            _logger.LogCritical(message);
        }
    }
}
