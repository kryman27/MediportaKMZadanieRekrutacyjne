using MediportaKMZadanieRekrutacyjne.Config;
using MediportaKMZadanieRekrutacyjne.Database;
using MediportaKMZadanieRekrutacyjne.Models;
using MediportaKMZadanieRekrutacyjne.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediportaKMZadanieRekrutacyjne.Controllers
{
    [ApiController]
    [Route("api")]
    public class StackOverflowController : ControllerBase
    {

        private readonly ILogger<StackOverflowController> logger;

        public StackOverflowController(ILogger<StackOverflowController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [Route("test")]
        public IResult Test()
        {
            return Results.Ok("hello");
        }

        [HttpGet]
        [Route("tags")]
        public IResult GetTags([FromQuery] int tagsPerPage, [FromQuery] int pageNumber, [FromQuery] SortBy sortBy, [FromQuery] SortOrder sortOrder)
        {
            int initialIndex = tagsPerPage * pageNumber;
            double totalNumberOfPages;
            List<Tag> result = new();

            try
            {
                using (SoApiDbContext dbCtx = new())
                {
                    totalNumberOfPages = (double)dbCtx.Tags.Count() / (double)tagsPerPage;

                    var tempList = dbCtx.Tags.Skip(initialIndex - tagsPerPage).Take(tagsPerPage).ToList();

                    switch (sortBy, sortOrder)
                    {
                        case (SortBy.name, SortOrder.asc):
                            result.AddRange(tempList.OrderBy(t => t.Name));
                            break;

                        case (SortBy.name, SortOrder.desc):
                            result.AddRange(tempList.OrderByDescending(t => t.Name));
                            break;

                        case (SortBy.percentage, SortOrder.asc):
                            result.AddRange(tempList.OrderBy(t => t.PopulationPercentage));
                            break;
                        case (SortBy.percentage, SortOrder.desc):
                            result.AddRange(tempList.OrderByDescending(t => t.PopulationPercentage));
                            break;

                        default:
                            break;
                    }

                    Response.Headers.Add("X-total-pages-number", $"{Math.Round(totalNumberOfPages, MidpointRounding.ToPositiveInfinity)}");
                    logger.LogInformation($"{result.Count} tags returned correctly");
                    return Results.Ok(result);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return Results.Problem(ex.Message);
            }
        }

        [HttpPost]
        [Route("tags")]
        public IResult ForceTagsUpdate()
        {
            try
            {
                List<Tag> tags = new();
                bool hasMoreFlag = true;

                int currentPage = 1;
                int pagesLimiter = 50;

                using (SoApiDbContext dbCtx = new())
                {
                    dbCtx.Database.ExecuteSqlRaw("TRUNCATE TABLE dbo.Tags");

                    var soService = new StackOverflowAPIService();

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

                    InitialConfigurator.CalculateTagsPercentage(logger);

                    logger.LogInformation($"Tags updated: {tags.Count}");

                    return Results.Ok($"Tags updated: {tags.Count}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return Results.Problem(ex.Message);
            }
        }
    }
}
