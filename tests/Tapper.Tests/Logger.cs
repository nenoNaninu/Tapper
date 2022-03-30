using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Tapper.Tests;

public class Logger : ILogger
{
    public static readonly ILogger Empty = new Logger();

    private class Disposable : IDisposable
    {
        public static readonly Disposable Empty = new();

        public void Dispose()
        {
        }
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return Disposable.Empty;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
    }
}
