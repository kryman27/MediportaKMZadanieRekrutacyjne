using System.Text.Json;

namespace MediportaKMZadanieRekrutacyjne.Config
{
    public class ConfigurationManager
    {
        public readonly string apiUrl;
        private static ConfigurationManager _instance;
        private static readonly object _lock = new object();

        private ConfigurationManager()
        {
            var appPath = AppDomain.CurrentDomain.BaseDirectory;
            var configPath = Path.Combine(appPath, "MealPlannerConfig.json");
            var rawConfig = File.ReadAllText(configPath);
            JsonDocument jsonConfig = JsonDocument.Parse(rawConfig);
            var root = jsonConfig.RootElement;

            apiUrl = root.GetProperty("ApiUrl").GetString();
        }

        public ConfigurationManager GetInstance()
        {
            if (_instance == null)
            {
                lock(_lock)
                {
                    _instance = new ConfigurationManager();
                    return _instance;
                }
            }
            else
                return _instance;
        }

        private string DecryptKey(string encryptedKey)
        {
            string decryptedKey = string.Empty;

            //TODO - decrypting logic needed here


            return decryptedKey;
        }
    }
}
