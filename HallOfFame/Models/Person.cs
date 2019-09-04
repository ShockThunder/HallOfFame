using System.Collections.Generic;

namespace HallOfFame.Models
{
    public class Person
    {
        public long id { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public List<Skill> skills { get; set; }
    }
}
