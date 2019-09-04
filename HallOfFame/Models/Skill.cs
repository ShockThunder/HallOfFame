using Newtonsoft.Json;


namespace HallOfFame.Models
{
    public class Skill
    {
        /// <summary>
        /// Primary key to store skill objects in database
        /// </summary>
        [JsonIgnore]
        public long id { get; set; }
        public string name { get; set; }
        public byte level { get; set; }

        /// <summary>
        /// Navigation property to set relationship with concrete person
        /// </summary>
        [JsonIgnore]
        public Person person { get; set; }
    }
}
