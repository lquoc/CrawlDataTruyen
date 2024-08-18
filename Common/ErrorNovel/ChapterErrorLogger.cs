using NLog;
using NLog.Config;
using System.Globalization;

namespace Common.ErrorNovel
{

    public class ChapterErrorLogger
    {
        private readonly Logger _chapterlogger;
        private static ChapterErrorLogger mychapterLogger;
        public ChapterErrorLogger()
        {
            LogManager.Configuration = new XmlLoggingConfiguration(Path.Combine(AppContext.BaseDirectory, "Log", "nlog.dev.config"));
            _chapterlogger = LogManager.GetLogger("errorchapter");
        }
        public static ChapterErrorLogger GetInstance()
        {
            return mychapterLogger ??= new ChapterErrorLogger();
        }

        public enum LogLevel
        {
            Debug,
            Info,
            Warn,
            Error,
            Fatal
        }

        public void Info(string msg)
        {
            WriteLog(msg, LogLevel.Info);
        }
        private void WriteLog(string msg, LogLevel level, Exception e = null)
        {
            var ei = new LogEventInfo(Level(level), _chapterlogger.Name, CultureInfo.CurrentCulture, msg, null, e)
            {
                TimeStamp = DateTime.Now,
                Level = Level(level)
            };
            _chapterlogger.Log(ei);
        }
        private NLog.LogLevel Level(LogLevel level)
        {
            return level switch
            {
                LogLevel.Debug => NLog.LogLevel.Debug,
                LogLevel.Info => NLog.LogLevel.Info,
                LogLevel.Warn => NLog.LogLevel.Warn,
                LogLevel.Error => NLog.LogLevel.Error,
                LogLevel.Fatal => NLog.LogLevel.Fatal,
            };
        }
    }

}
