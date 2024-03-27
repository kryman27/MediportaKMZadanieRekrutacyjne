using MediportaKMZadanieRekrutacyjne.Services;
using System.Text.Json;

public class Program
{
    private static void Main(string[] args)
    {
        var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appConfig.json");
        var rawConfig = File.ReadAllText(configPath);
        JsonDocument jsonConfig = JsonDocument.Parse(rawConfig);
        var root = jsonConfig.RootElement;

        var currentPage = root.GetProperty("CurrentPage").GetInt32();
        //var currentPage = 7;

        Console.WriteLine(currentPage);
        var tagService = new TagsService();
        int pageNum = 0;

        do
        {
            pageNum = tagService.GetTags(currentPage).Result;
            currentPage = pageNum;
        } while (pageNum != 0);

        Console.ReadKey();
        Environment.Exit(0);
    }
}