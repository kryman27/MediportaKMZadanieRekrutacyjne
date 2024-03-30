using MediportaKMZadanieRekrutacyjne.Config;
using MediportaKMZadanieRekrutacyjne.Crypto;

namespace MediportaKMZadanieRekrutacyjne.Test
{
    public class UnitTests
    {
        [Fact]
        public void PercentageCalculationTest()
        {
            //Arrange
            using InMemoryDbContext dbCtx = new InMemoryDbContext();
            dbCtx.Tags.Add(new Models.Tag() { TagID = 1, HasSynonyms = true, IsModeratorOnly = true, IsRequired = true, Count = 100, Name = "Test1"});
            dbCtx.Tags.Add(new Models.Tag() { TagID = 2, HasSynonyms = true, IsModeratorOnly = true, IsRequired = true, Count = 50, Name = "Test2"});
            dbCtx.Tags.Add(new Models.Tag() { TagID = 3, HasSynonyms = true, IsModeratorOnly = true, IsRequired = true, Count = 150, Name = "Test3"});
            dbCtx.Tags.Add(new Models.Tag() { TagID = 4, HasSynonyms = true, IsModeratorOnly = true, IsRequired = true, Count = 200, Name = "Test4"});
            dbCtx.SaveChanges();

            //Act
            new InitialConfigurator().CalculateTagsPercentage<InMemoryDbContext>(dbCtx);
            var calculationResults = dbCtx.Tags.ToList();

            //Assert
            Assert.Equal((Math.Round(100.00m / 500.00m * 100.00m, 5)), calculationResults[0].PopulationPercentage);
            Assert.Equal((Math.Round(50.00m / 500.00m * 100.00m, 5)), calculationResults[1].PopulationPercentage);
            Assert.Equal((Math.Round(150.00m / 500.00m * 100.00m, 5)), calculationResults[2].PopulationPercentage);
            Assert.Equal((Math.Round(200.00m / 500.00m * 100.00m, 5)), calculationResults[3].PopulationPercentage);
        }

        [Fact]
        public void DecrypterTest()
        {
            //Arrange
            string encryptedText = "dmbqxosdc^sdrs^rsqhmf";

            //Act
            string result = Decrypter.DecryptKey(encryptedText);

            //Assert
            Assert.Equal("encrypted_test_string", result);
        }
    }
}