using System;
using Microsoft.Extensions.Logging;

namespace FractalSource.Services
{
    public static class LoggerExtensions
    {
        public static void LogMethodStart(this ILogger logger, string methodName)
        {
            logger.LogMethodEvent(methodName, MethodEventType.Started);
        }

        public static void LogMethodEnd(this ILogger logger, string methodName)
        {
            logger.LogMethodEvent(methodName, MethodEventType.Completed);
        }

        private static void LogMethodEvent(this ILogger logger, string methodName, MethodEventType methodEventType)
        {
            logger.LogInformation($"{methodName} {methodEventType} at: {DateTimeOffset.Now}");
        }
    }
}