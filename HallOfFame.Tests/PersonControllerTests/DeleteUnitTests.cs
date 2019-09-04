using HallOfFame.Controllers;
using HallOfFame.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HallOfFame.Tests.PersonControllerTests
{
    public class DeleteUnitTests
    {
        

        [Fact]
        public void TestDeletePerson()
        {
            var logger = Mock.Of<ILogger<PersonController>>();

            var dbContext = DbContextMocker.GetPersonContext(nameof(TestDeletePerson));
            var controller = new PersonController(dbContext, logger);

            controller.Delete(1);

            var value = controller.GetPerson(1).StatusCode;

            dbContext.Dispose();

            Assert.True(value == StatusCodes.Status404NotFound);
        }

        [Fact]
        public void TestDeletePersonSuccess()
        {
            var logger = Mock.Of<ILogger<PersonController>>();
            var dbContext = DbContextMocker.GetPersonContext(nameof(TestDeletePersonSuccess));
            var controller = new PersonController(dbContext, logger);

            var response = controller.Delete(1);
            var value = response.StatusCode;

            dbContext.Dispose();

            Assert.True(value == StatusCodes.Status200OK);
        }

        [Fact]
        public void TestDeletePersonNotFound()
        {
            var logger = Mock.Of<ILogger<PersonController>>();
            var dbContext = DbContextMocker.GetPersonContext(nameof(TestDeletePersonNotFound));
            var controller = new PersonController(dbContext, logger);

            var response = controller.Delete(10);
            var value = response.StatusCode;

            dbContext.Dispose();

            Assert.True(value == StatusCodes.Status404NotFound);
        }
    }
}
