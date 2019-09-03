using Newtonsoft.Json;


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
