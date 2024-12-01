using Cocona;
using Microsoft.Build.Locator;
using Tapper;
using Tapper.Logging;

MSBuildLocator.RegisterDefaults();

var builder = CoconaApp.CreateBuilder();

builder.Logging.AddSimpleConsoleApp();

var app = builder.Build();

app.AddCommands<App>();

await app.RunAsync();
