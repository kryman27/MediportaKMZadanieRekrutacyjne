using MediportaKMZadanieRekrutacyjne.Models;
using System.Text.Json;
using ConfigurationManager = MediportaKMZadanieRekrutacyjne.Config.ConfigurationManager;

namespace MediportaKMZadanieRekrutacyjne.Services
{
    public class StackOverflowAPIService
    {
        ILoggerFactory loggerFactory = new LoggerFactory();
        ILogger<APIResponseModel> logger;

        public StackOverflowAPIService()
        {
            logger = loggerFactory.CreateLogger<APIResponseModel>();
        }

        /// <summary>
        /// Method for retriving data from Stack Exchange API
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns>Task<APIResponseModel></returns>
        public async Task<APIResponseModel> GetTags(int pageNumber)
        {
            APIResponseModel result = null;

            try
            {
                var handler = new HttpClientHandler();
                if (handler.SupportsAutomaticDecompression)
                {
                    handler.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;
                }

                using (var client = new HttpClient(handler))
                {
                    var url = ConfigurationManager.GetInstance().appConfiguration.ApiUrl;
                    var finalUrl = url.Replace("PAGENUMBERVALUE", pageNumber.ToString());

                    var responseString = await client.GetStringAsync(finalUrl);

                    result = JsonSerializer.Deserialize<APIResponseModel>(responseString);

                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return null;
        }
    }
}
