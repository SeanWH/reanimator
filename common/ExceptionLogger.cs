using System;
using NLog;

namespace Revival.Common
{
    public static class ExceptionLogger
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static void LogException(Exception exception, string message = "")
        {
            _logger.Error(exception, message);
        }
    }
}