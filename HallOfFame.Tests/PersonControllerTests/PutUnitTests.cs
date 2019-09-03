using HallOfFame.Controllers;
using HallOfFame.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
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
            dbContext.Dispose();

            var dbContextNext = DbContextMocker.GetPersonContext(nameof(TestPutPerson));
            var controller = new PersonController(dbContextNext, logger);

            
            var response = controller.Put(testPerson);
            var value = response.Value as Person;

            dbContextNext.Dispose();

            bool result = false;

            if (testPerson.id == value.id && testPerson.name == value.name &&
                testPerson.displayName == value.displayName)
                result = true;

            for (int i = 0; i < testPerson.skills.Count; i++)
            {
                if ((testPerson.skills[i].id == value.skills[i].id && testPerson.skills[i].name == value.skills[i].name &&
                    testPerson.skills[i].level == value.skills[i].level) == false)
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


            var dbContext = DbContextMocker.GetPersonContext(nameof(TestPutPerson));
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


            var dbContext = DbContextMocker.GetPersonContext(nameof(TestPutPerson));
            var controller = new PersonController(dbContext, logger);

            
            var response = controller.Put(testPerson);
            var value = response.StatusCode;

            dbContext.Dispose();

            Assert.True(value == StatusCodes.Status400BadRequest);
        }

    }
}
