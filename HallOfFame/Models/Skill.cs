using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HallOfFame.Models
{
    public class Skill
    {
        [JsonIgnore]
        public long id { get; set; }
        public string name { get; set; }
        public byte level { get; set; }
        [JsonIgnore]
        public Person person { get; set; }
    }
}
