using Microsoft.Build.Locator;
using Tapper;

MSBuildLocator.RegisterDefaults();

var app = ConsoleApp.Create(args);

app.AddCommands<App>();

await app.RunAsync();
