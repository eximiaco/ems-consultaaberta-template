using EMS.ConsultaAberta.Crosscutting;
using EMS.ConsultaAberta.Infra;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;

var Namespace = typeof(Program).Namespace;
var AppName = Namespace?[(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1)..];
var builder = WebApplication.CreateBuilder(args);
var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();
var configuration = configBuilder.Build();
var loggerSwitch = new LoggingLevelSwitch
{
    MinimumLevel = Enum.TryParse(configuration["LogSwitch"], true, out LogEventLevel level) 
        ? level 
        : LogEventLevel.Warning
};

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")??"Development";

var loggerBuilder = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .MinimumLevel.ControlledBy(loggerSwitch)
    .Enrich.WithProperty("Environment", environmentName)
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
    builder.Services.Configure<MongoOptions>(configuration.GetSection(MongoOptions.Name));
    builder.Services.AddScoped<MateriaisRepository>();
    EntitiesConfiguration.ApplyMongoEntitiesConfiguration();
    
    var app = builder.Build();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSerilogRequestLogging();
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
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