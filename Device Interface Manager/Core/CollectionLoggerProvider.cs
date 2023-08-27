using System;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace Device_Interface_Manager.Core;
public class CollectionLoggerProvider : ILoggerProvider
{
    private readonly ObservableCollection<string> logMessages;

    public CollectionLoggerProvider(ObservableCollection<string> logMessages)
    {
        this.logMessages = logMessages;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new CollectionLogger(logMessages);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    private class CollectionLogger : ILogger
    {
        private readonly ObservableCollection<string> _logMessages;

        public CollectionLogger(ObservableCollection<string> logMessages)
        {
            _logMessages = logMessages;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _logMessages.Add(formatter(state, exception));
        }
    }
}