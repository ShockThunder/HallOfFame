using HallOfFame.Controllers;
using HallOfFame.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace HallOfFame.Tests.PersonControllerTests
{
    public class PutUnitTests
    {
        [Fact]
        public void TestPutPerson()
        {
            var logger = Mock.Of<ILogger<PersonController>>();

            var testPerson = new Person()
            {
                name = "Test",
                displayName = "TestDisplay",
                skills = new List<Skill>()
                {
                    new Skill {name = "asserting", level = 3},
                }
            };

            testPerson.skills[0].person = testPerson;


            var dbContext = DbContextMocker.GetPersonContext(nameof(TestPutPerson));
            
            var controller = new PersonController(dbContext, logger);      

            var value = controller.Put(testPerson).Value as Person;
            var id = value.id;

            var check = controller.GetPerson(id).Value as Person;

            dbContext.Dispose();

            bool result = false;

            if (testPerson.id == check.id && testPerson.name == check.name &&
                testPerson.displayName == check.displayName)
                result = true;

            for (int i = 0; i < testPerson.skills.Count; i++)
            {
                if ((testPerson.skills[i].name == check.skills[i].name &&
                    testPerson.skills[i].level == check.skills[i].level) == false)
                    result = false;
            }

            Assert.True(result);
        }


        [Fact]
        public void TestPutPersonSuccess()
        {
            var logger = Mock.Of<ILogger<PersonController>>();

            var testPerson = new Person()
            {
                name = "Test",
                displayName = "TestDisplay",
                skills = new List<Skill>()
                {
                    new Skill {name = "asserting", level = 3},
                }
            };

            testPerson.skills[0].person = testPerson;


            var dbContext = DbContextMocker.GetPersonContext(nameof(TestPutPersonSuccess));
            var controller = new PersonController(dbContext, logger);

            
            var response = controller.Put(testPerson);
            var value = response.StatusCode;

            dbContext.Dispose();

            Assert.True(value == StatusCodes.Status200OK);
        }

        [Fact]
        public void TestPutPersonBadRequest()
        {
            var logger = Mock.Of<ILogger<PersonController>>();

            var testPerson = new Person()
            {                
                displayName = "TestDisplay",
            };


            var dbContext = DbContextMocker.GetPersonContext(nameof(TestPutPersonBadRequest));
            var controller = new PersonController(dbContext, logger);

            
            var response = controller.Put(testPerson);
            var value = response.StatusCode;

            dbContext.Dispose();

            Assert.True(value == StatusCodes.Status400BadRequest);
        }

    }
}
