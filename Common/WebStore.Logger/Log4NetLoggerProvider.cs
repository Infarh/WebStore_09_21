﻿using System.Collections.Concurrent;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace WebStore.Logger
{
    public class Log4NetLoggerProvider : ILoggerProvider
    {
        private readonly string _ConfigurationFile;
        private readonly ConcurrentDictionary<string, Log4NetLogger> _Loggers = new();

        public Log4NetLoggerProvider(string ConfigurationFile) => _ConfigurationFile = ConfigurationFile;

        public ILogger CreateLogger(string Сategory) =>
            _Loggers.GetOrAdd(Сategory, category =>
            {
                var xml = new XmlDocument();
                xml.Load(_ConfigurationFile);
                return new Log4NetLogger(category, xml["log4net"]);
            });

        public void Dispose() => _Loggers.Clear();
    }
}
