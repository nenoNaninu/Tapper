using System.Text.Json;
using System.Text.Json.Serialization;
using MessagePack.AspNetCoreMvcFormatter;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var configure = builder.Configuration;

// Add services to the container.

builder.Services
    .AddControllers(options =>
    {
        options.InputFormatters.Add(new MessagePackInputFormatter(ContractlessStandardResolver.Options));
        options.OutputFormatters.Add(new MessagePackOutputFormatter(ContractlessStandardResolver.Options));
    })
    .AddJsonOptions(options =>
    {
        if (configure["UseStringEnum"] == "true")
        {
            options.JsonSerializerOptions
                .Converters
                .Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        }
    });
//.AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.PropertyNamingPolicy = null;
//    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
//});

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpLogging();

app.MapControllers();

app.Run();
