﻿using System.IO;

namespace WebApi.Models
{
    public class CharacterInfo
    {
        public int Health { get; set; }
        public int Damage { get; set; }
        public string Type { get; set; }
        public int Speed { get; set; }
        public string Team { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public CharacterInfo(string team, string name)
        {
            using (StreamReader r = new StreamReader("./characters/" + team + "/" + name + ".json"))
            {
                var json = r.ReadToEnd();
                var obj = Newtonsoft.Json.Linq.JObject.Parse(json);

                this.Health = (int)obj.SelectToken("health");
                this.Damage = (int)obj.SelectToken("damage");
                this.Type = (string)obj.SelectToken("type");
                this.Speed = (int)obj.SelectToken("speed");
                this.Description = (string)obj.SelectToken("description");
            }

            this.Name = name;
            this.Team = team;

            if (this.Description == null)
                this.Description = name;
        }
    }
}