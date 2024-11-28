using Cocona;
using Microsoft.Build.Locator;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Tapper;

MSBuildLocator.RegisterDefaults();

var builder = CoconaApp.CreateBuilder();

builder.Logging
    .AddConsole(options => options.FormatterName = CliConsoleFormatter.FormatterName)
    .AddConsoleFormatter<CliConsoleFormatter, ConsoleFormatterOptions>();

var app = builder.Build();

app.AddCommands<App>();

await app.RunAsync();
