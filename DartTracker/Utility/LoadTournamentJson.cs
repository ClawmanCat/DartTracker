using DartTracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartTracker.Utility
{
    public class LoadTournamentJson
    {
        public static Tournament LoadTournament(string jsonString)
        {
            
            Tournament tournament = JsonConvert.DeserializeObject<Tournament>(jsonString, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
            });
            return tournament;
        }
        public static Tournament LoadJson()
        {
            string jsonString = File.ReadAllText("..\\..\\..\\tournament.json");
            return LoadTournament(jsonString);
        }
    }
}
