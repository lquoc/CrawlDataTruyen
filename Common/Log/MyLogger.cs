using NLog;
using NLog.Config;
using System.Globalization;

namespace Migration.Common;

public class MyLogger
{
    private readonly Logger _logger;
    private static MyLogger myLogger;
    public MyLogger()
    {
        LogManager.Configuration = new XmlLoggingConfiguration(Path.Combine(AppContext.BaseDirectory, "Log", "nlog.dev.config"));
        _logger = LogManager.GetLogger("logger");
    }
    public static MyLogger GetInstance()
    {
        return myLogger ??= new MyLogger();
    }

    public enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
    public void Debug(string msg)
    {
        WriteLog(msg, LogLevel.Debug);
    }
    public void Info(string msg)
    {
        WriteLog(msg, LogLevel.Info);
    }
    public void Warn(string msg)
    {
        WriteLog(msg, LogLevel.Warn);
    }
    public void Warn(string msg, Exception e)
    {
        WriteLog(msg, LogLevel.Warn, e);
    }
    public void Error(string msg)
    {
        WriteLog(msg, LogLevel.Error);
    }
    public void Error(string msg, Exception err)
    {
        WriteLog(msg, LogLevel.Error, err);
    }
    public void Fatal(string msg)
    {
        WriteLog(msg, LogLevel.Fatal);
    }
    public void Fatal(string msg, Exception err)
    {
        WriteLog(msg, LogLevel.Fatal, err);
    }
    private void WriteLog(string msg, LogLevel level, Exception e = null)
    {
        var ei = new LogEventInfo(Level(level), _logger.Name, CultureInfo.CurrentCulture, msg, null, e)
        {
            TimeStamp = DateTime.Now,
            Level = Level(level)
        };
        _logger.Log(ei);
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
