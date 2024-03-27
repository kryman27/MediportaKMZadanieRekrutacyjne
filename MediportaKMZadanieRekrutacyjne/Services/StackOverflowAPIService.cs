using MediportaKMZadanieRekrutacyjne.Models;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MediportaKMZadanieRekrutacyjne.Services
{
    public class StackOverflowAPIService
    {
        //private string apiUrl = "https://api.stackexchange.com/2.3/tags?page=1&order=asc&sort=name&site=stackoverflow";

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
                    var url = $"https://api.stackexchange.com/2.3/tags?page={pageNumber}&order=asc&sort=name&site=stackoverflow&key=Hcl6TN1VjfObT)ahuHhKMA((";
                    var responseString = await client.GetStringAsync(url);

                    Console.WriteLine(responseString);
                    result = JsonSerializer.Deserialize<APIResponseModel>(responseString);


                    //var request = new HttpRequestMessage(HttpMethod.Get, url);
                    //request.Headers.Add("key", "Hcl6TN1VjfObT)ahuHhKMA((");

                    //var response = await client.SendAsync(request);
                    //var rawResult = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(rawResult);
                    //result = JsonSerializer.Deserialize<APIResponseModel>(rawResult);


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
