using Autofac;
using Autofac.Extensions.DependencyInjection;
using EMS.ConsultaAberta.Crosscutting;
using EMS.ConsultaAberta.HttpServelessApi.Infrastructure;
using EMS.ConsultaAberta.Infra;
using EMS.Infrastructure.Web;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;

var appNamespace = typeof(Program).Namespace;
var appName = appNamespace?[(appNamespace.LastIndexOf('.', appNamespace.LastIndexOf('.') - 1) + 1)..];
var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")??"Development";

var builder = WebApplication.CreateBuilder(args);

// TODO : Adicionar configuracao para cofres (aka AzureKeyVault)
var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
if (environmentName == "Development")
    configBuilder.AddUserSecrets<Program>();

var configuration = configBuilder.Build();

var loggerSwitch = new LoggingLevelSwitch
{
    MinimumLevel = Enum.TryParse(configuration["LogSwitch"], true, out LogEventLevel level) 
        ? level 
        : LogEventLevel.Warning
};
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
        .ForContext("ApplicationName", appName)
        .Information("Starting application");
    
    builder.Services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.Name));
    builder.Services
        .AddControllers(options =>
        {
            options.Filters.Add(typeof(HttpGlobalExceptionFilter));
        });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
    builder.Services.AddOptions();
    builder.Services.AddSingleton(Log.Logger);
    EntitiesConfiguration.ApplyMongoEntitiesConfiguration();
    
    builder.Host.UseSerilog();
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new ApplicationModule()));
    
    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        // TODO: Debater uso em DEV ou PROD
        app.UseSerilogRequestLogging();
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseCors("CorsPolicy");
    app.UseHttpsRedirection();
    // TODO : Configurar Autorização
    //app.UseAuthorization();
    app.MapControllers();
    // TODO : Configurar Health Checks
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log
        .ForContext("ApplicationName", appName)
        .Fatal(ex, "Program terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}