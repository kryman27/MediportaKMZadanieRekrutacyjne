using MediportaKMZadanieRekrutacyjne.Database;
using MediportaKMZadanieRekrutacyjne.Models;
using System.Text.Json;

namespace MediportaKMZadanieRekrutacyjne.Services
{
    public class TagsService
    {
        public int currentPage;
        public int pageLimiter;
        private readonly int pagesOverhead = 10;
        public async Task<int> GetTags(int pageNumber)
        {
            APIResponseModel result = null;

            try
            {
                List<Tag> tags = new();
                bool hasMoreFlag = true;
                currentPage = pageNumber;
                pageLimiter = pageNumber + pagesOverhead;

                var handler = new HttpClientHandler();
                if (handler.SupportsAutomaticDecompression)
                {
                    handler.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;
                }

                using (var client = new HttpClient(handler))
                {
                    while (hasMoreFlag && currentPage <= pageLimiter)
                    {
                        var url = "https://api.stackexchange.com/2.3/tags?page=PAGENUMBERVALUE&order=asc&sort=name&site=stackoverflow&key=Hcl6TN1VjfObT)ahuHhKMA((";
                        var finalUrl = url.Replace("PAGENUMBERVALUE", currentPage.ToString());

                        var responseString = await client.GetStringAsync(finalUrl);

                        result = JsonSerializer.Deserialize<APIResponseModel>(responseString);

                        if (result != null && result.Items != null)
                        {
                            foreach (var rt in result.Items)
                            {
                                Tag temp = new Tag
                                {
                                    HasSynonyms = rt.HasSynonyms,
                                    IsModeratorOnly = rt.IsModeratorOnly,
                                    IsRequired = rt.IsRequired,
                                    Count = rt.Count,
                                    Name = rt.Name,
                                };

                                tags.Add(temp);
                            }
                            hasMoreFlag = result.HasMore;
                            currentPage++;
                            Console.WriteLine(currentPage);
                        }
                    }
                }

                using (SoApiDbContext dbCtx = new())
                {
                    dbCtx.Tags.AddRange(tags);
                    dbCtx.SaveChanges();
                }

                return hasMoreFlag == false ? 0 : currentPage;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }
    }
}
