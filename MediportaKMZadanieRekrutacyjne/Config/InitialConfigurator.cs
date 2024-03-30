using MediportaKMZadanieRekrutacyjne.Database;
using MediportaKMZadanieRekrutacyjne.Interfaces;
using MediportaKMZadanieRekrutacyjne.Models;
using MediportaKMZadanieRekrutacyjne.Services;
using Microsoft.EntityFrameworkCore;

namespace MediportaKMZadanieRekrutacyjne.Config
{
    public class InitialConfigurator
    {
        private static ILogger logger = new LoggerFactory().CreateLogger<InitialConfigurator>();

        /// <summary>
        /// Method checks if database exists, and create it if necessarry, due to entities registered in SoApiDbContext
        /// </summary>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        public void CreateDbAndTable(Configuration config, ILogger logger)
        {
            try
            {
                using (SoApiDbContext dbCtx = new())
                {
                    var dbCreateFlag = dbCtx.Database.EnsureCreated();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Method allows to retrive TAGS from StackExchangeAPI and add them to database
        /// </summary>
        public void CheckDbRetriveDataFromApi<T>(T dbCtx, int limit) where T : DbContext, IDbCtx
        {
            try
            {
                List<Tag> tags = new();
                bool hasMoreFlag = true;

                int currentPage = ConfigurationManager.GetInstance().appConfiguration.CurrentPage;
                int pagesLimiter = currentPage + limit;

                if (ConfigurationManager.GetInstance().appConfiguration.FirstLaunchFlag || dbCtx.Tags.Count() == 0)
                {
                    var soService = new StackOverflowAPIService();

                    //TODO - consider second condition, is this constriction neccessary?
                    while (hasMoreFlag && currentPage <= pagesLimiter)
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
                    ConfigurationManager.GetInstance().appConfiguration.CurrentPage = currentPage;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Calculates percentage of every tag in whole db population
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbCtx"></param>
        public void CalculateTagsPercentage<T>(T dbCtx) where T : DbContext, IDbCtx
        {
            try
            {
                int totalTagsCount;

                totalTagsCount = dbCtx.Tags.Sum(t => t.Count);

                var tags = dbCtx.Tags;

                foreach (var tag in tags)
                {
                    tag.PopulationPercentage = Math.Round(((decimal)tag.Count / (decimal)totalTagsCount) * 100.00m, 5);

                    dbCtx.Tags.Update(tag);
                }
                dbCtx.SaveChanges();

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}
