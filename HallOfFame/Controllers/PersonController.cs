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
                    name = "Knight",
                    displayName = "Lothric",
                    skills = new List<Skill>()
                    {
                        new Skill { name = "strength", level = 2 },
                        new Skill { name = "health", level = 10 }
                    }
                });

                db.SaveChanges();
            }
        }
        /// <summary>
        /// Returns array of persons
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     GET/Persons
        ///
        /// </remarks>
        /// <returns>[Person, Person, ...]</returns>
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

        /// <summary>
        /// Returns person object
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET/Person/1
        ///
        /// </remarks>
        /// <param name="id"></param> 
        /// <returns>A selected person</returns>
        /// <response code="200">Returns selected person</response>
        /// <response code="404">Returns not found code</response>
        [HttpGet("{id}")]
        public JsonResult GetPerson(long id)
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

        /// <summary>
        /// Update person with new data
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Post/Persons/1
        ///     {
        ///         "name" = "Knight",
        ///         "displayName" = "Lothric",
        ///         "skills" =  [{ "name" = "strength", 
        ///                        "level" = "2"},
        ///                      { "name" = "health", 
        ///                        "level" = "10"}]
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="person"></param> 
        /// <returns>Updated person</returns>
        /// <response code="200">Returns updated person</response>
        /// <response code="500">Returns internal error message</response>
        /// <response code="400">Returns bad request code</response>
        /// <response code="404">Returns not found code</response>
        [HttpPost("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public JsonResult Post(long id, [FromBody] Person person)
        {
            var oldPerson = db.Persons.Include(pn => pn.skills).FirstOrDefault(pn => pn.id == id);
            person.id = id;

            if (oldPerson != null)
            {
                if(validator.validatePerson(person))
                {
                    try
                    {
                        string oldData = ConstructPersonString(oldPerson);

                        db.Skills.RemoveRange(oldPerson.skills);
                        db.SaveChanges();
                        db.Entry(oldPerson).State = EntityState.Detached;
                        db.Entry(person).State = EntityState.Modified;

                        foreach (var skill in person.skills)
                        {
                            skill.person = person;
                            db.Skills.Add(skill);
                        }

                        db.Update(person);
                        db.SaveChanges();

                        var newPerson = db.Persons.Include(pn => pn.skills).FirstOrDefault(pn => pn.id == id);

                        _logger.LogInformation($"success POST/PERSON/{id}");
                        _logger.LogInformation($"old data: {oldData}");
                        _logger.LogInformation($"new data: {ConstructPersonString(newPerson)}");

                        return new JsonResult(newPerson)
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

        /// <summary>
        /// Update person with new data
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Put/Person/
        ///     {
        ///         "name" = "Knight",
        ///         "displayName" = "Lothric",
        ///         "skills" =  [{ "name" = "strength", 
        ///                        "level" = "2"},
        ///                      { "name" = "health", 
        ///                        "level" = "10"}]
        ///     }
        /// </remarks>
        /// <param name="person"></param> 
        /// <returns>Updated person</returns>
        /// <response code="200">Returns updated person</response>
        /// <response code="500">Returns internal error message</response>
        /// <response code="400">Returns bad request code</response>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
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

        /// <summary>
        /// Delete person object
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete/Persons/1
        ///
        /// </remarks>
        /// <param name="id"></param> 
        /// <returns>A deleted person</returns>
        /// <response code="200">Returns deleted person</response>
        /// <response code="404">Returns not found code</response>
        /// <response code="500">Returns internal error message</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        public JsonResult Delete(long id)
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


        /// <summary>
        /// Construct string wiht person info to log it
        /// </summary>
        /// <param name="person"></param>
        /// <returns>string which contains person data</returns>
        private string ConstructPersonString(Person person)
        {
            string personData = string.Empty;

            personData = $"name: {person.name}, displayName: {person.displayName}. SKILLS: ";
            foreach (var skill in person.skills)
            {
               personData += $"skillName: {skill.name}, skillLevel: {skill.level}; ";
            }

            return personData;
        }
    }
}
