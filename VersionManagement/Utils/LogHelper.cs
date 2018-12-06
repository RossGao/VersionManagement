using NLog;

namespace VersionManagement.Utils
{
    public static class LogHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="message">日志内容</param>
        public static void LogMessage(LogLevel level, string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                logger.Log(level, message);
            }
        }
    }
}
