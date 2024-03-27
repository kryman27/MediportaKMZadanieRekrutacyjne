using MediportaKMZadanieRekrutacyjne.Config;
using MediportaKMZadanieRekrutacyjne.Models;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ConfigurationManager = MediportaKMZadanieRekrutacyjne.Config.ConfigurationManager;

namespace MediportaKMZadanieRekrutacyjne.Services
{
    public class StackOverflowAPIService
    {
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
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
