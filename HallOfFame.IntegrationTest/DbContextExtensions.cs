using System.Collections.Generic;
using HallOfFame.Models;

namespace HallOfFame.IntegrationTest
{
    public static class DbContextExtensions
    {
        public static void Seed(this PersonContext context)
        {
            context.Add(new Person
            {
                name = "Leha",
                displayName = "Alex",
                skills = new List<Skill>()
                {
                    new Skill { name = "programming", level = 2 },
                    new Skill { name = "sneaking", level = 10 }
                }
            });
            context.Add(new Person
            {
                name = "Mihail",
                displayName = "SuperMan",
                skills = new List<Skill>()
                {
                    new Skill { name = "flying", level = 2 },
                    new Skill { name = "lasers", level = 2 }
                }
            });
            context.Add(new Person
            {
                name = "Deprived",
                displayName = "King",
                skills = new List<Skill>()
                {
                }
            });

            context.SaveChanges();
        }
    }
}
