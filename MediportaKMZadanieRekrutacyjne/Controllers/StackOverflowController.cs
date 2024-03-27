using MediportaKMZadanieRekrutacyjne.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediportaKMZadanieRekrutacyjne.Controllers
{
    [ApiController]
    [Route("stack-overflow")]
    public class StackOverflowController : ControllerBase
    {
        [HttpGet]
        [Route("test")]
        public async Task<IResult> StackOverflowTest()
        {
            var service = new StackOverflowAPIService();

            var test = await service.GetTags(1);

            

            return Results.Ok(test);
        }
    }
}
