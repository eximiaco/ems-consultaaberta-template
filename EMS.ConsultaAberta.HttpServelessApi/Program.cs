using EMS.ConsultaAberta.Crosscutting;
using EMS.ConsultaAberta.Infra;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;

var Namespace = typeof(Program).Namespace;
var AppName = Namespace?[(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1)..];
var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

var loggerSwitch = new LoggingLevelSwitch
{
    MinimumLevel = Enum.TryParse(builder.Configuration["LogSwitch"], true, out LogEventLevel level) 
        ? level 
        : LogEventLevel.Warning
};

//var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var loggerBuilder = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .MinimumLevel.ControlledBy(loggerSwitch)
    //.Enrich.WithProperty("Environment", environmentName)
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .Enrich.WithMachineName();
Log.Logger = loggerBuilder.CreateLogger();

try
{
    Log
        .ForContext("ApplicationName", AppName)
        .Information("Starting application");
    
    builder.Services.AddControllers();
    builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);
    builder.Services.Configure<MongoOptions>(builder.Configuration.GetSection(MongoOptions.Name));

    var app = builder.Build();

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    EntitiesConfiguration.ApplyMongoEntitiesConfiguration();

    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log
        .ForContext("ApplicationName", AppName)
        .Fatal(ex, "Program terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}