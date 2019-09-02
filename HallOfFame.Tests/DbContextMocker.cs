using HallOfFame.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HallOfFame.Tests
{
    public static class DbContextMocker
    {
        public static PersonContext GetPersonContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PersonContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new PersonContext(options);

            dbContext.Seed();

            return dbContext;
        }
    }
}
