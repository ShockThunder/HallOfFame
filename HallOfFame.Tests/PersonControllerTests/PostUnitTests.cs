using HallOfFame.Controllers;
using HallOfFame.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HallOfFame.Tests
{
    public class PostUnitTests
    {
        [Fact]
        public void TestPostPerson()
        {
            var testPerson = new Person()
            {
                id = 1,
                name = "Test",
                displayName = "TestDisplay",
                skills = new List<Skill>()
                {
                    new Skill { id = 1, name = "asserting", level = 3},
                }
            };

            testPerson.skills[0].person = testPerson;


            // Здесь делается диспоуз первого контекста и добавление второго, чтобы избежать ошибки трекинга
            // https://github.com/aspnet/EntityFrameworkCore/issues/12459

            var dbContext = DbContextMocker.GetPersonContext(nameof(TestPostPerson));
            dbContext.Dispose();

            var dbContextNext = DbContextMocker.GetPersonContext(nameof(TestPostPerson));
            var controller = new PersonController(dbContextNext);

            var response = controller.Post(1, testPerson);
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
        public void TestPostResponseSuccess()
        {
            var testPerson = new Person()
            {
                id = 1,
                name = "Test",
                displayName = "TestDisplay",
                skills = new List<Skill>()
                {
                    new Skill { id = 1, name = "asserting", level = 3},
                }
            };

            testPerson.skills[0].person = testPerson;


            // Здесь делается диспоуз первого контекста и добавление второго, чтобы избежать ошибки трекинга
            // https://github.com/aspnet/EntityFrameworkCore/issues/12459

            var dbContext = DbContextMocker.GetPersonContext(nameof(TestPostPerson));
            dbContext.Dispose();

            var dbContextNext = DbContextMocker.GetPersonContext(nameof(TestPostPerson));
            var controller = new PersonController(dbContextNext);

            var response = controller.Post(1, testPerson);
            var value = response.StatusCode;

            dbContextNext.Dispose();

            Assert.True(value == StatusCodes.Status200OK);
        }

        [Fact]
        public void TestPostResponseBadRequest()
        {
            var testPerson = new Person()
            {                
                displayName = "TestDisplay", 
            };


            // Здесь делается диспоуз первого контекста и добавление второго, чтобы избежать ошибки трекинга
            // https://github.com/aspnet/EntityFrameworkCore/issues/12459

            var dbContext = DbContextMocker.GetPersonContext(nameof(TestPostPerson));
            dbContext.Dispose();

            var dbContextNext = DbContextMocker.GetPersonContext(nameof(TestPostPerson));
            var controller = new PersonController(dbContextNext);

            var response = controller.Post(1, testPerson);
            var value = response.StatusCode;

            dbContextNext.Dispose();

            Assert.True(value == StatusCodes.Status400BadRequest);
        }


        [Fact]
        public void TestPostResponseNotFound()
        {
            var testPerson = new Person()
            {
                id = 1,
                name = "Test",
                displayName = "TestDisplay",
                skills = new List<Skill>()
                {
                    new Skill { id = 1, name = "asserting", level = 3},
                }
            };
            testPerson.skills[0].person = testPerson;

            // Здесь делается диспоуз первого контекста и добавление второго, чтобы избежать ошибки трекинга
            // https://github.com/aspnet/EntityFrameworkCore/issues/12459

            var dbContext = DbContextMocker.GetPersonContext(nameof(TestPostPerson));
            dbContext.Dispose();

            var dbContextNext = DbContextMocker.GetPersonContext(nameof(TestPostPerson));
            var controller = new PersonController(dbContextNext);

            var response = controller.Post(50, testPerson);
            var value = response.StatusCode;

            dbContextNext.Dispose();

            Assert.True(value == StatusCodes.Status404NotFound);
        }
    }
}
