using System;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace PRUNner
{
    public static class LogConfigurator
    {
        private static LoggingConfiguration? _config;
        
        public static void Initialize()
        {
            _config = new LoggingConfiguration();
            _config.AddRule(LogLevel.Debug, LogLevel.Fatal, new ConsoleTarget());
            LogManager.Configuration = _config;
        }

        public static void AddTarget(Target target, LogLevel logLevel)
        {
            if (_config == null)
            {
                throw new Exception("LogConfigurator has not been initialized! :(");
            }
            
            _config.AddRule(logLevel, LogLevel.Fatal, target);
            LogManager.Configuration = _config;
        }
    }
}