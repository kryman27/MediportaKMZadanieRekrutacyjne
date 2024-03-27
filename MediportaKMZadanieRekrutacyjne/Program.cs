using MediportaKMZadanieRekrutacyjne.Config;
using MediportaKMZadanieRekrutacyjne.Services;
using System.Diagnostics;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
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

        Console.WriteLine("Downloading tags started");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        InitialConfigurator.CheckDbRetriveDataFromApi(configuration);
        stopwatch.Stop();
        var timeElapsed = stopwatch.Elapsed.TotalSeconds;
        Console.WriteLine($"Download completed in {timeElapsed} seconds");
        MediportaKMZadanieRekrutacyjne.Config.ConfigurationManager.ChangeConfigurationFile();

        //uruchomDrugiProgramCoDoci¹gaTagi(configuration)
        Process process = new Process();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = $"/C {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TagsDownloader.exe")}";
        process.StartInfo.UseShellExecute = true;
        process.Start();


        app.Run();
    }
}