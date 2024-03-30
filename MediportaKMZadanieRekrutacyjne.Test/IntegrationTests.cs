using MediportaKMZadanieRekrutacyjne.Config;
using MediportaKMZadanieRekrutacyjne.Models;
using MediportaKMZadanieRekrutacyjne.Services;

namespace MediportaKMZadanieRekrutacyjne.Test
{
    public class IntegrationTests
    {
        [Fact]
        public void ReceivingDataFromStackExchangeApiAndAddingItToDbTest()
        {
            //Arrange
            using InMemoryDbContext dbCtx = new InMemoryDbContext();

            //Act
            new InitialConfigurator().CheckDbRetriveDataFromApi<InMemoryDbContext>(dbCtx, 0);

            //Assert
            Assert.InRange<int>(dbCtx.Tags.ToList().Count, 1, 50);
        }

        [Fact]
        public void ServiceGetTagsTest()
        {
            //Arrange
            var service = new StackOverflowAPIService();

            //Act
            var result = service.GetTags(1);

            //Assert
            Assert.True(result.Result.Items != null && result.Result.Items.Count != 0);
        }
    }
}
