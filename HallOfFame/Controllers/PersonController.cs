using System;
using System.Collections.Generic;
using System.Linq;
using HallOfFame.Utils;
using HallOfFame.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HallOfFame.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        ErrorController errorController = new ErrorController();
        Validator validator = new Validator();
        PersonContext db;

        public PersonController(PersonContext context, ILogger<PersonController> logger)
        {
            _logger = logger;
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

            _logger.LogInformation("success GET/PERSONS");
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
                _logger.LogInformation($"success GET/PERSON/{id}.");
                return new JsonResult(result)
                {
                    StatusCode = StatusCodes.Status200OK
                };                
            }
            else
            {
                _logger.LogWarning($"not found GET/PERSON/{id}");
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

                        _logger.LogInformation($"success POST/PERSON/{id}");
                        _logger.LogInformation($"old data: {ConstructPersonString(oldPerson)}");
                        _logger.LogInformation($"new data: {ConstructPersonString(person)}");
                        return new JsonResult(person)
                        {
                            StatusCode = StatusCodes.Status200OK
                        };
                    }
                    catch(DbUpdateException e)
                    {
                        _logger.LogError(e, $"InternalServerError POST/PERSON/{id}");
                        return errorController.InternalError500(e.Message);
                    }                                     
                }
                else
                {
                    _logger.LogWarning($"bad request POST/PERSON/{id}");
                    return errorController.BadRequest400();
                }                
            }
            else
            {
                _logger.LogWarning($"not found POST/PERSON/{id}");
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

                    _logger.LogInformation($"success PUT/PERSON");
                    return new JsonResult(person)
                    {
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                catch (DbUpdateException e)
                {
                    _logger.LogError(e, $"InternalServerError PUT/PERSON");
                    return errorController.InternalError500(e.Message);
                }
            }
            else
            {
                _logger.LogWarning($"bad request PUT/PERSON");
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

                    _logger.LogInformation($"success DELETE/PERSON/{id}");
                    return new JsonResult(person)
                    {
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                catch (DbUpdateException e)
                {
                    _logger.LogError(e, $"InternalServerError DELETE/PERSON/{id}");
                    return errorController.InternalError500(e.Message);
                }                
            }
            else
            {
                _logger.LogWarning($"not found DELETE/PERSON/{id}");
                return errorController.NotFound404();
            }            
        }

        private string ConstructPersonString(Person person)
        {
            string personData = string.Empty;

            personData = $"name: {person.name}, displayName: {person.displayName}. SKILLS: ";
            foreach (var skill in person.skills)
            {
               personData += $"skillName: {skill.name}, skillLevel: {skill.level}";
            }

            return personData;
        }
    }
}
