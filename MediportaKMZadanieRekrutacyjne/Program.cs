using MediportaKMZadanieRekrutacyjne.Config;
using MediportaKMZadanieRekrutacyjne.Database;
using MediportaKMZadanieRekrutacyjne.Services;
using System.Diagnostics;
using System.Text.Json.Serialization;

public class Program
{
    private const int _pagesLimit = 50;
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<StackOverflowAPIService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        var dbCtx = new SoApiDbContext();

        var configuration = MediportaKMZadanieRekrutacyjne.Config.ConfigurationManager.GetInstance().appConfiguration;
        var initialConfigurator = new InitialConfigurator();

        app.Logger.LogInformation("Database initiating started");
        initialConfigurator.CreateDbAndTable(configuration, app.Logger);
        app.Logger.LogInformation("Database initiated");

        app.Logger.LogInformation("Tags data download in progress");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        if (configuration.FirstLaunchFlag == true)
            initialConfigurator.CheckDbRetriveDataFromApi<SoApiDbContext>(dbCtx, _pagesLimit);
        else
            app.Logger.LogInformation("Database already exists");

        stopwatch.Stop();
        var timeElapsed = stopwatch.Elapsed.TotalSeconds;
        app.Logger.LogInformation($"Data download completed in {timeElapsed} seconds");

        stopwatch.Reset();
        app.Logger.LogInformation($"Calculation tags percentage");
        stopwatch.Start();
        MediportaKMZadanieRekrutacyjne.Config.ConfigurationManager.ChangeConfigurationFile();
        stopwatch.Stop();
        timeElapsed = stopwatch.Elapsed.TotalSeconds;
        app.Logger.LogInformation($"Percentage calculated in {timeElapsed} seconds");

        initialConfigurator.CalculateTagsPercentage<SoApiDbContext>(dbCtx);

        app.Run();
    }
}