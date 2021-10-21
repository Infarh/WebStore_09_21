using System;
using System.ComponentModel;
using System.Reflection;
using System.Xml;
using log4net;
using Microsoft.Extensions.Logging;

namespace WebStore.Logger
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _Log;

        public Log4NetLogger(string Category, XmlElement Configuration)
        {
            var logger_repository = LogManager
               .CreateRepository(
                    Assembly.GetEntryAssembly(), 
                    typeof(log4net.Repository.Hierarchy.Hierarchy));

            _Log = LogManager.GetLogger(logger_repository.Name, Category);

            log4net.Config.XmlConfigurator.Configure(Configuration);
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        //public bool IsEnabled(LogLevel Level)
        //{
        //    switch (Level)
        //    {
        //        default:
        //            throw new InvalidEnumArgumentException(nameof(Level), (int)Level, typeof(LogLevel));

        //        case LogLevel.None:
        //            return false;

        //        case LogLevel.Trace:
        //            return _Log.IsDebugEnabled;

        //        case LogLevel.Debug:
        //            return _Log.IsDebugEnabled;

        //        case LogLevel.Information:
        //            return _Log.IsInfoEnabled;

        //        case LogLevel.Warning:
        //            return _Log.IsWarnEnabled;

        //        case LogLevel.Error:
        //            return _Log.IsErrorEnabled;

        //        case LogLevel.Critical:
        //            return _Log.IsFatalEnabled;
        //    }
        //}

        public bool IsEnabled(LogLevel Level) =>
            Level switch
            {
                LogLevel.None => false,
                LogLevel.Trace => _Log.IsDebugEnabled,
                LogLevel.Debug => _Log.IsDebugEnabled,
                LogLevel.Information => _Log.IsInfoEnabled,
                LogLevel.Warning => _Log.IsWarnEnabled,
                LogLevel.Error => _Log.IsErrorEnabled,
                LogLevel.Critical => _Log.IsFatalEnabled,
                _ => throw new InvalidEnumArgumentException(nameof(Level), (int)Level, typeof(LogLevel))
            };

        public void Log<TState>(
            LogLevel Level,
            EventId Id, 
            TState State,
            Exception Error,
            Func<TState, Exception, string> Formatter)
        {
            if (Formatter is null)
                throw new ArgumentNullException(nameof(Formatter));

            if(!IsEnabled(Level))
                return;

            var log_string = Formatter(State, Error);
            if(string.IsNullOrEmpty(log_string) && Error is null)
                return;

            switch (Level)
            {
                default:
                    throw new InvalidEnumArgumentException(nameof(Level), (int)Level, typeof(LogLevel));

                case LogLevel.None:
                    break;

                case LogLevel.Trace:
                case LogLevel.Debug:
                    _Log.Debug(log_string);
                    break;

                case LogLevel.Information:
                    _Log.Info(log_string);
                    break;

                case LogLevel.Warning:
                    _Log.Warn(log_string);
                    break;

                case LogLevel.Error:
                    _Log.Error(log_string, Error);
                    break;

                case LogLevel.Critical:
                    _Log.Fatal(log_string, Error);
                    break;
            }
        }
    }
}
