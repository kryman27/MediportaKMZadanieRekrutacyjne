using MediportaKMZadanieRekrutacyjne.Config;
using MediportaKMZadanieRekrutacyjne.Services;
using System.Diagnostics;
using System.Text.Json.Serialization;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<StackOverflowAPIService>();
        //TODO - config file manager and handling is needed

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        //initial checking of tags database and retrive data from StackExchangeAPI
        var configuration = MediportaKMZadanieRekrutacyjne.Config.ConfigurationManager.GetInstance().appConfiguration;

        app.Logger.LogInformation("Database initiating started");
        InitialConfigurator.CreateDbAndTable(configuration, app.Logger);
        app.Logger.LogInformation("Database initiated");

        app.Logger.LogInformation("Tags data download in progress");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        if (configuration.FirstLaunchFlag == true)
        {
            InitialConfigurator.CheckDbRetriveDataFromApi(configuration, app.Logger);
        }
        else
        {
            app.Logger.LogInformation("Database already exists");
        }
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

        InitialConfigurator.CalculateTagsPercentage(app.Logger);

        app.Run();
    }
}