using MediportaKMZadanieRekrutacyjne.Crypto;
using MediportaKMZadanieRekrutacyjne.Models;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace MediportaKMZadanieRekrutacyjne.Config
{
    /// <summary>
    /// Singleton class for reading and managing configuration
    /// </summary>
    public class ConfigurationManager
    {
        public Configuration appConfiguration { get; set; }
        private static ConfigurationManager _instance;
        private static readonly object _lock = new object();

        private ConfigurationManager()
        {
            var appPath = AppDomain.CurrentDomain.BaseDirectory;
            var configPath = Path.Combine(appPath, "appConfig.json");
            var rawConfig = File.ReadAllText(configPath);
            JsonDocument jsonConfig = JsonDocument.Parse(rawConfig);
            var root = jsonConfig.RootElement;

            var apiUrlRaw = root.GetProperty("ApiUrl").GetString();
            var firstLaunchFlag = root.GetProperty("FirstLaunchFlag").GetBoolean();
            var apiKeyEncrypted = root.GetProperty("ApiKey").GetString();
            var connectionString = root.GetProperty("DbConnectionString").GetString();
            var currentPage = root.GetProperty("CurrentPage").GetInt32();

            var apiKeyDecrypted = Decrypter.DecryptKey(apiKeyEncrypted);

            var apiUrl = apiUrlRaw.Replace("APIKEYVALUE", apiKeyDecrypted);

            appConfiguration = new Configuration(firstLaunchFlag, apiKeyDecrypted, connectionString, apiUrl, currentPage);
        }

        public static ConfigurationManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance = new ConfigurationManager();
                    return _instance;
                }
            }
            else
                return _instance;
        }


        /// <summary>
        /// Saves current configuration to config file
        /// </summary>
        public static void ChangeConfigurationFile()
        {
            var appPath = AppDomain.CurrentDomain.BaseDirectory;
            var configPath = Path.Combine(appPath, "appConfig.json");

            File.Delete(configPath);

            var temp = _instance.appConfiguration;
            var newConfig = new Configuration(false, temp.ApiKey, temp.DbConnectionString, temp.ApiUrl, temp.CurrentPage);

            File.WriteAllText(configPath, JsonSerializer.Serialize(newConfig));
        }
    }
}
