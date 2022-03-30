using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.Logging;

namespace Tapper;

public class App : ConsoleAppBase
{
    private readonly ILogger<App> _logger;

    public App(ILogger<App> logger)
    {
        _logger = logger;
    }

    [RootCommand]
    public async Task Transpile(
        [Option("p", "Path to the project file (XXX.csproj)")] string project,
        [Option("o", "Output directory")] string output,
        [Option("eol", "lf / crlf / cr")] string newLine = "lf",
        [Option("i", "Indent size")] int indent = 2,
        [Option("s", "Json / MessagePack : The output type will be suitable for the selected serializer.")] string serializer = "json",
        [Option("n", "PascalCase / camelCase / none (The name in C# is used as it is.)")] string namingStyle = "none")
    {
        newLine = newLine switch
        {
            "crlf" => "\r\n",
            "lf" => "\n",
            "cr" => "\r",
            _ => throw new ArgumentException($"{newLine} is not supported.")
        };


        _logger.Log(LogLevel.Information, "Start loading the csproj of {path}.", Path.GetFullPath(project));

        output = Path.GetFullPath(output);

        if (!Enum.TryParse<SerializerOption>(serializer, true, out var serializerOption))
        {
            _logger.Log(LogLevel.Information, "Only json or messagepack can be selected for serializer. {type} is not supported.", serializer);
            return;
        }

        if (!Enum.TryParse<NamingStyle>(namingStyle, true, out var style))
        {
            _logger.Log(LogLevel.Information, "The naming style can only be selected from None, CamelCase, or PascalCase. {style} is not supported.", namingStyle);
            return;
        }

        try
        {
            var compilation = await this.CreateCompilationAsync(project);

            await TranspileCore(compilation, output, newLine, indent, serializerOption, style);

            _logger.Log(LogLevel.Information, "======== Transpilation is completed. ========");
            _logger.Log(LogLevel.Information, "Please check the output folder: {output}", output);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Information, "======== Exception ========");
            _logger.Log(LogLevel.Error, "{ex}", ex);
        }
    }

    private async Task<Compilation> CreateCompilationAsync(string projectPath)
    {
        var logger = new ConsoleLogger(LoggerVerbosity.Quiet);
        using var workspace = MSBuildWorkspace.Create();

        var msBuildProject = await workspace.OpenProjectAsync(projectPath, logger, null, this.Context.CancellationToken);

        _logger.Log(LogLevel.Information, "Create Compilation...");
        var compilation = await msBuildProject.GetCompilationAsync(this.Context.CancellationToken);

        if (compilation is null)
        {
            throw new InvalidOperationException("Failed to create compilation.");
        }

        return compilation;
    }

    private async Task TranspileCore(Compilation compilation, string outputDir, string newLine, int indent, SerializerOption serializerOption, NamingStyle namingStyle)
    {
        var transpiler = new Transpiler(compilation, newLine, indent, serializerOption, namingStyle, _logger);

        var generatedSourceCodes = transpiler.Transpile();

        await OutputToFiles(outputDir, generatedSourceCodes);
    }

    private async Task OutputToFiles(string outputDir, IEnumerable<GeneratedSourceCode> generatedSourceCodes)
    {
        if (Directory.Exists(outputDir))
        {
            var tsFiles = Directory.GetFiles(outputDir, "*.ts");

            _logger.Log(LogLevel.Information, "Cleanup old files...");

            foreach (var tsFile in tsFiles)
            {
                File.Delete(tsFile);
            }
        }
        else
        {
            Directory.CreateDirectory(outputDir);
        }

        foreach (var generatedSourceCode in generatedSourceCodes)
        {
            using var fs = File.Create(Path.Join(outputDir, generatedSourceCode.SourceName));
            await fs.WriteAsync(Encoding.UTF8.GetBytes(generatedSourceCode.Content));
        }
    }
}
