﻿using HallOfFame.Controllers;
using HallOfFame.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HallOfFame.Tests.PersonControllerTests
{
    public class DeleteUnitTests
    {
        [Fact]
        public void TestDeletePerson()
        {
            var dbContext = DbContextMocker.GetPersonContext(nameof(TestDeletePerson));
            var controller = new PersonController(dbContext);

            var response = controller.Delete(1);
            var value = response.Value as Person;

            dbContext.Dispose();

            Assert.True(value != null);
        }

        [Fact]
        public void TestDeletePersonSuccess()
        {
            var dbContext = DbContextMocker.GetPersonContext(nameof(TestDeletePerson));
            var controller = new PersonController(dbContext);

            var response = controller.Delete(1);
            var value = response.StatusCode;

            dbContext.Dispose();

            Assert.True(value == StatusCodes.Status200OK);
        }

        [Fact]
        public void TestDeletePersonNotFound()
        {
            var dbContext = DbContextMocker.GetPersonContext(nameof(TestDeletePerson));
            var controller = new PersonController(dbContext);

            var response = controller.Delete(10);
            var value = response.StatusCode;

            dbContext.Dispose();

            Assert.True(value == StatusCodes.Status404NotFound);
        }
    }
}