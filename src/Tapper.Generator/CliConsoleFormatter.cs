using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace Tapper;

internal class CliConsoleFormatter : ConsoleFormatter
{
    public const string FormatterName = "Cli";

    public CliConsoleFormatter() : base(FormatterName)
    {
    }

    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider, TextWriter textWriter)
    {
        string message = logEntry.Formatter(logEntry.State, logEntry.Exception);

        if (logEntry.Exception == null && message == null)
        {
            return;
        }

        textWriter.WriteLine(message);
    }
}
