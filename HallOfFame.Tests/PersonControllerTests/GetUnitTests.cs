﻿using HallOfFame.Controllers;
using HallOfFame.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace HallOfFame.Tests.PersonControllerTests
{
    public class GetUnitTests
    {
        [Fact]
        public void TestGetPersons()
        {
            var logger = Mock.Of<ILogger<PersonController>>();
            var dbContext = DbContextMocker.GetPersonContext(nameof(TestGetPersons));
            var controller = new PersonController(dbContext, logger);

            var response = controller.GetPersons();
            var value = response.Value as Person[];

            dbContext.Dispose();

            Assert.True(value != null);
        }

        [Fact]
        public void TestGetPersonsResponse()
        {
            var logger = Mock.Of<ILogger<PersonController>>();
            var dbContext = DbContextMocker.GetPersonContext(nameof(TestGetPersonsResponse));
            var controller = new PersonController(dbContext, logger);

            var response = controller.GetPersons();
            var value = response.StatusCode;

            dbContext.Dispose();

            Assert.True(value == StatusCodes.Status200OK);
        }


        [Fact]
        public void TestGetPerson()
        {
            var logger = Mock.Of<ILogger<PersonController>>();

            var testPerson = new Person()
            {
                name = "Knight",
                displayName = "Lothric",
                skills = new List<Skill>()
                {
                    new Skill { id = 1, name = "strength", level = 2 },
                    new Skill { id = 2, name = "health", level = 10 }
                }
            };
            testPerson.id = 1;
            testPerson.skills[0].person = testPerson;
            testPerson.skills[1].person = testPerson;

            var dbContext = DbContextMocker.GetPersonContext(nameof(TestGetPerson));
            var controller = new PersonController(dbContext, logger);

            var response = controller.GetPerson(1);
            var value = response.Value as Person;

            dbContext.Dispose();

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
        public void TestGetPersonResponseSuccess()
        {
            var logger = Mock.Of<ILogger<PersonController>>();
            var dbContext = DbContextMocker.GetPersonContext(nameof(TestGetPersonResponseSuccess));
            var controller = new PersonController(dbContext, logger);

            var response = controller.GetPerson(1);
            var value = response.StatusCode;

            dbContext.Dispose();

            Assert.True(value == StatusCodes.Status200OK);
        }

        [Fact]
        public void TestGetPersonResponseNotFound()
        {
            var logger = Mock.Of<ILogger<PersonController>>();
            var dbContext = DbContextMocker.GetPersonContext(nameof(TestGetPersonResponseNotFound));
            var controller = new PersonController(dbContext, logger);

            var response = controller.GetPerson(4);
            var value = response.StatusCode;

            dbContext.Dispose();

            Assert.True(value == StatusCodes.Status404NotFound);
        }

    }
}
