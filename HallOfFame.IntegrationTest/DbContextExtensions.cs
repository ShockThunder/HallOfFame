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
                name = "Knight",
                displayName = "Lothric",
                skills = new List<Skill>()
                {
                    new Skill { name = "strength", level = 2 },
                    new Skill { name = "health", level = 10 }
                }
            });
            context.Add(new Person
            {
                name = "Pyromancer",
                displayName = "Izalit",
                skills = new List<Skill>()
                {
                    new Skill { name = "fireball", level = 2 },
                    new Skill { name = "firestorm", level = 2 }
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
