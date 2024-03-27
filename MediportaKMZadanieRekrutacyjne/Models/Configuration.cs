namespace MediportaKMZadanieRekrutacyjne.Models
{
    public class Configuration
    {
        public Configuration(bool firstLaunchFlag, string apiKey, string dbConnectionString, string apiUrl)
        {
            FirstLaunchFlag = firstLaunchFlag;
            ApiKey = apiKey;
            DbConnectionString = dbConnectionString;
            ApiUrl = apiUrl;
        }

        public bool FirstLaunchFlag { get; set; }
        public string ApiKey {  get; set; }
        public string DbConnectionString {  get; set; }
        public string ApiUrl { get; set; }
    }
}
