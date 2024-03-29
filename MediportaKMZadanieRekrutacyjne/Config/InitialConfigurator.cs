using MediportaKMZadanieRekrutacyjne.Database;
using MediportaKMZadanieRekrutacyjne.Models;
using MediportaKMZadanieRekrutacyjne.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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
                    //string rawSql = "CREATE DATABASE SODB;\r\nGO\r\nUSE SODB;\r\nGO\r\nCREATE TABLE [dbo].[Tags] (\r\n    [TagID]                INT            IDENTITY (1, 1) NOT NULL,\r\n    [HasSynonyms]          BIT            NOT NULL,\r\n    [IsModeratorOnly]      BIT            NOT NULL,\r\n    [IsRequired]           BIT            NOT NULL,\r\n    [Count]                INT            NOT NULL,\r\n    [Name]                 NVARCHAR (MAX) NULL,\r\n    [PopulationPercentage] DECIMAL (7, 5) NULL\r\n);\r\nGO";

                    string createDBquery = "CREATE DATABASE SODB;";
                    string createTableQuery = "USE SODB; CREATE TABLE [dbo].[Tags] ([TagID] INT IDENTITY (1, 1) NOT NULL, [HasSynonyms] BIT NOT NULL, [IsModeratorOnly] BIT NOT NULL, [IsRequired] BIT NOT NULL, [Count] INT NOT NULL, [Name] NVARCHAR (MAX) NULL, [PopulationPercentage] DECIMAL (7, 5) NULL);";

                    using (SoApiDbContext dbCtx = new())
                    {
                        dbCtx.Database.SetConnectionString(config.SQLServerConnectionString);
                        
                        if(!dbCtx.Database.CanConnect())
                        {
                            dbCtx.Database.ExecuteSqlRaw(createDBquery);
                            dbCtx.SaveChanges();
                        }

                        dbCtx.Database.ExecuteSqlRaw(createTableQuery);
                        dbCtx.SaveChanges();
                    }

                    //using (SqlConnection connection = new SqlConnection(config.SQLServerConnectionString))
                    //{
                    //    connection.Open();

                    //    using(SqlCommand command = new(rawSql, connection))
                    //    {
                    //        var temp = command.ExecuteNonQuery();
                    //    }
                    //}
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
