namespace MediportaKMZadanieRekrutacyjne.Models
{
    public class Configuration
    {
        public Configuration(bool firstLaunchFlag, string apiKey, string dbConnectionString, string apiUrl, int currentPage)
        {
            FirstLaunchFlag = firstLaunchFlag;
            ApiKey = apiKey;
            DbConnectionString = dbConnectionString;
            ApiUrl = apiUrl;
            CurrentPage = currentPage;
        }

        public bool FirstLaunchFlag { get; set; }
        public string ApiKey {  get; set; }
        public string DbConnectionString {  get; set; }
        public string ApiUrl { get; set; }
        public int CurrentPage {  get; set; }
    }
}
