using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cocona;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.Logging;

namespace Tapper;

public class App : CoconaConsoleAppBase
{
    private readonly Microsoft.Extensions.Logging.ILogger _logger;

    public App(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger("Tapper");
    }

    public async Task Transpile(
        [Option('p', Description = "Path to the project file (Xxx.csproj)")]
        string project,
        [Option('o', Description = "Output directory")]
        string output,
        [Option("eol", Description ="lf / crlf / cr")]
        NewLineOption newLine = NewLineOption.Lf,
        [Option('i', Description ="Indent size")]
        int indent = 4,
        [Option("asm", Description ="The flag whether to extend the transpile target to the referenced assembly.")]
        bool assemblies = false,
        [Option('s', Description ="JSON / MessagePack : The output type will be suitable for the selected serializer.")]
        SerializerOption serializer = SerializerOption.Json,
        [Option('n', Description ="camelCase / PascalCase / none (The name in C# is used as it is.)")]
        NamingStyle namingStyle = NamingStyle.CamelCase,
        [Option(Description ="value / name / nameCamel / NamePascal / union / unionCamel / UnionPascal")]
        EnumStyle @enum = EnumStyle.Value,
        [Option("attr", Description ="The flag whether attributes such as JsonPropertyName should affect transpilation.")]
        bool attribute = true)
    {
        _logger.Log(LogLevel.Information, "Start loading the csproj of {path}.", Path.GetFullPath(project));

        output = Path.GetFullPath(output);

        try
        {
            var compilation = await this.CreateCompilationAsync(project);

            await TranspileCore(compilation, output, newLine, indent, assemblies, serializer, namingStyle, @enum, attribute);

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

    private async Task TranspileCore(
        Compilation compilation,
        string outputDir,
        NewLineOption newLine,
        int indent,
        bool referencedAssembliesTranspilation,
        SerializerOption serializerOption,
        NamingStyle namingStyle,
        EnumStyle enumStyle,
        bool enableAttributeReference)
    {
        var options = new TranspilationOptions(
            compilation,
            serializerOption,
            namingStyle,
            enumStyle,
            newLine,
            indent,
            referencedAssembliesTranspilation,
            enableAttributeReference
        );

        var transpiler = new Transpiler(compilation, options, _logger);

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
            await using var fs = File.Create(Path.Join(outputDir, generatedSourceCode.SourceName));
            await fs.WriteAsync(Encoding.UTF8.GetBytes(generatedSourceCode.Content));
        }
    }
}
