using MessagePack.AspNetCoreMvcFormatter;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Add(new MessagePackInputFormatter(ContractlessStandardResolver.Options));
    options.OutputFormatters.Add(new MessagePackOutputFormatter(ContractlessStandardResolver.Options));
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
