using System;
using System.Collections.Generic;
using System.Linq;
using HallOfFame.Utils;
using HallOfFame.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HallOfFame.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        ErrorController errorController = new ErrorController();
        Validator validator = new Validator();
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
      
        [HttpGet("~/api/v1/Persons")]
        public JsonResult GetPersons()
        {
            var result = db.Persons.Include(pn => pn.skills).ToArray();

            _logger.LogInformation("access to => ", result);
            return new JsonResult(result)
            {                
                StatusCode = StatusCodes.Status200OK
            };
        }


        [HttpGet("{id}")]
        public JsonResult GetPerson(int id)
        {
            var result = db.Persons.Include(pn => pn.skills).FirstOrDefault(pn => pn.id == id);
            if (result != null)
            {
                return new JsonResult(result)
                {
                    StatusCode = StatusCodes.Status200OK
                };                
            }
            else
            {
                return errorController.NotFound404();
            }
        }

        // POST api/v1/person/id
        [HttpPost("{id}")]
        public JsonResult Post(int id, [FromBody] Person person)
        {
            var oldPerson = db.Persons.Include(pn => pn.skills).AsNoTracking().FirstOrDefault(pn => pn.id == id);
            person.id = id;
            if (oldPerson != null)
            {
                if(validator.validatePerson(person))
                {
                    try
                    {
                        db.Update(person);
                        db.SaveChanges();

                        return new JsonResult(person)
                        {
                            StatusCode = StatusCodes.Status200OK
                        };
                    }
                    catch(DbUpdateException e)
                    {
                        return errorController.InternalError500(e.Message);
                    }                                     
                }
                else
                {
                    return errorController.BadRequest400();
                }                
            }
            else
            {
                return errorController.NotFound404();
            }
            
        }

        // PUT api/v1/person/
        [HttpPut]
        public JsonResult Put([FromBody] Person person)
        {
            if (validator.validatePerson(person))
            {
                try
                {
                    db.Persons.Add(person);
                    db.SaveChanges();
                    return new JsonResult(person)
                    {
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                catch (DbUpdateException e)
                {
                    return errorController.InternalError500(e.Message);
                }
            }
            else
            {
                return errorController.BadRequest400();
            }
        }

        // DELETE api/v1/person/id
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            var person = db.Persons.FirstOrDefault(pn => pn.id == id);
            if (person != null)
            {
                try
                {
                    db.Persons.Remove(person);
                    db.SaveChanges();
                    return new JsonResult(person)
                    {
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                catch (DbUpdateException e)
                {
                    return errorController.InternalError500(e.Message);
                }                
            }
            else
            {
                return errorController.NotFound404();
            }            
        }
    }
}
