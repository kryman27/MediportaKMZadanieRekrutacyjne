using MediportaKMZadanieRekrutacyjne.Config;
using MediportaKMZadanieRekrutacyjne.Services;

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
        InitialConfigurator.CheckDbRetriveDataFromApi();

        app.Run();
    }
}