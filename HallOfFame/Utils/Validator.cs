using HallOfFame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HallOfFame.Utils
{
    public class Validator
    {
        public bool validatePerson(Person person)
        {
            if (person.name == null || person.displayName == null)
                return false;

            return true;
        }
    }
}
