﻿using MediportaKMZadanieRekrutacyjne.Database;
using MediportaKMZadanieRekrutacyjne.Models;
using MediportaKMZadanieRekrutacyjne.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;

namespace MediportaKMZadanieRekrutacyjne.Config
{
    public static class InitialConfigurator
    {
        public static void CreateDbAndTable(Configuration config)
        {
            try
            {
                if (config.FirstLaunchFlag)
                {
                    using (SoApiDbContext dbCtx = new())
                    {
                        var dbCreateFlag = dbCtx.Database.EnsureCreated();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Method allows to retrive TAGS from StackExchangeAPI
        /// </summary>
        public static void CheckDbRetriveDataFromApi(Configuration config)
        {
            try
            {
                List<Tag> tags = new();
                bool hasMoreFlag = true;

                //TODO - this should not be hardcoded here as 24!
                int currentPage = ConfigurationManager.GetInstance().appConfiguration.CurrentPage;
                int pagesLimiter = currentPage + 50;

                using (SoApiDbContext dbCtx = new())
                {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void CalculateTagsPercentage()
        {
            try
            {
                int totalTagsCount;

                using (SoApiDbContext dbCtx = new())
                {
                    totalTagsCount = dbCtx.Tags.Sum(t => t.Count);

                    var tags = dbCtx.Tags;

                    foreach (var tag in tags)
                    {
                        tag.PopulationPercentage = Math.Round(((decimal)tag.Count / (decimal)totalTagsCount) * 100.00m, 5);

                        dbCtx.Tags.Update(tag);
                    }
                    dbCtx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
