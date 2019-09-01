using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HallOfFame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HallOfFame.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        PersonContext db;
        public PersonController(PersonContext context)
        {
            db = context;
            if (!db.Persons.Any())
            {
                db.Persons.Add(new Person
                {
                    name = "Leha",
                    displayName = "Alex",
                    skills = new List<Skill>() { new Skill { name = "programming", level = 2},
                                                 new Skill { name = "sneaking", level = 10}
                    }
                });

                db.SaveChanges();
            }
        }

        // GET api/v1/persons
        [HttpGet("~/api/v1/persons")]
        public List<Person> Get()
        {
            return db.Persons.Include(pn => pn.skills).ToList();
        }

        // GET api/v1/person/5
        [HttpGet("{id}")]
        public Person Get(int id)
        {
            return db.Persons.FirstOrDefault(ps => ps.id == id);
        }

        // POST api/v1/person/
        [HttpPost("{id}")]
        public void Post(int id, [FromBody] Person person)
        {
            var oldPerson = db.Persons.AsNoTracking().First(pn => pn.id == id);
            oldPerson = person;
            oldPerson.id = id;
            db.Persons.Update(oldPerson);
            db.SaveChanges();
        }

        // PUT api/v1/person/
        [HttpPut]
        public void Put([FromBody] Person person)
        {
            db.Persons.Add(person);
            db.SaveChanges();
        }

        // DELETE api/v1/person/id
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var person = db.Persons.First(pn => pn.id == id);
            db.Persons.Remove(person);
            db.SaveChanges();
        }
    }
}
