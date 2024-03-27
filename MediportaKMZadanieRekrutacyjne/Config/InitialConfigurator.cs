using MediportaKMZadanieRekrutacyjne.Database;
using MediportaKMZadanieRekrutacyjne.Models;
using MediportaKMZadanieRekrutacyjne.Services;

namespace MediportaKMZadanieRekrutacyjne.Config
{
    public static class InitialConfigurator
    {
        /// <summary>
        /// Method allows to retrive TAGS from StackExchangeAPI
        /// </summary>
        public static void CheckDbRetriveDataFromApi()
        {
            try
            {
                List<Tag> tags = new();
                bool hasMoreFlag = true;

                //TODO - this should not be hardcoded here as 24!
                int currentPage = 24;

                using (SoApiDbContext dbCtx = new())
                {
                    if (dbCtx.Tags.Count() == 0)
                    {
                        var soService = new StackOverflowAPIService();

                        //TODO - consider second condition, is this constriction neccessary?
                        while (hasMoreFlag && currentPage < 28)
                        {
                            var retrievedTags = soService.GetTags(currentPage);

                            foreach (var rt in retrievedTags.Result.Items)
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
                            hasMoreFlag = retrievedTags.Result.HasMore;
                            currentPage++;
                        }

                        dbCtx.Tags.AddRange(tags);
                        dbCtx.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
