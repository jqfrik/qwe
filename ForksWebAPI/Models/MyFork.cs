using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForksWebAPI.Models
{
    public class MyFork
    {
        [Key]
        public long Id { get; set; }

        [JsonProperty("Time")]
        public string Time { get; set; }
        [JsonProperty("%")]
        public string Percent { get; set; }
        [JsonProperty("Букмекер")]
        public string Bookmaker { get; set; }
        [JsonProperty("Событие")]
        public string Event { get; set; }
        [JsonProperty("Коэф")]
        public string Rate { get; set; }
        [JsonProperty("kef1")]
        public string Coef1 { get; set; }
        [JsonProperty("kef2")]
        public string Coef2 { get; set; }
        [JsonProperty("is_direct_1")]
        public string Is_Direct_1 { get; set; }
        [JsonProperty("is_direct_2")]
        public string Is_Direct_2 { get; set; }
        [JsonProperty("bet1")]
        public string Bet1 { get; set; }
        [JsonProperty("bet2")]
        public string Bet2 { get; set; }
        [JsonProperty("bet_id1")]
        public string Bet_Id1 { get; set; }
        [JsonProperty("bet_id2")]
        public string Bet_Id2 { get; set; }
        [JsonProperty("event_id1")]
        public string Event_Id1 { get; set; }
        [JsonProperty("event_id2")]
        public string Event_Id2 { get; set; }
        [JsonProperty("sound_id")]
        public string Sound_Id { get; set; }
        [JsonProperty("booker_id1")]
        public string Booker_Id1 { get; set; }
        [JsonProperty("booker_id2")]
        public string Booker_Id2 { get; set; }
        [JsonProperty("event_l_id")]
        public string Event_l_Id { get; set; }
        [JsonProperty("k1")]
        public string K1 { get; set; }
        [JsonProperty("k2")]
        public string K2 { get; set; }
        [JsonProperty("md")]
        public string Md { get; set; }
        [JsonProperty("noNameCell1")]
        public string NoNameCell1 { get; set; }
        [JsonProperty("noNameCell2")]
        public string NoNameCell2 { get; set; }
        [JsonProperty("Direction1")]
        public string Direction1 { get; set; }

        [JsonProperty("Direction2")]
        public string Direction2 { get; set; }

        [JsonProperty("Sport")]
        public string Sport { get; set; }

        [JsonProperty("Teams1")]

        public string Teams1 { get; set; }

        [JsonProperty("Teams2")]

        public string Teams2 { get; set; }

        [JsonProperty("League1")]

        public string League1 { get; set; }

        [JsonProperty("League2")]

        public string League2 { get; set; }

        [JsonProperty("Score1")]

        public string Score1 { get; set; }

        [JsonProperty("Score2")]

        public string Score2 { get; set; }

        [JsonProperty("ProcessOfMatchFirst")]

        public string ProcessOfMatchFirst { get; set; }

        [JsonProperty("ProcessOfMatchSecond")]

        public string ProcessOfMatchSecond { get; set; }

        [JsonProperty("BookmakerNameFirst")]
        public string BookmakerNameFirst { get; set; }

        [JsonProperty("BookmakerNameSecond")]

        public string BookmakerNameSecond { get; set; }

        [JsonProperty("ForksCountFirstBet")]
        public string ForksCountFirstBet { get; set; }

        [JsonProperty("ForksCountSecondBet")]
        public string ForksCountSecondBet { get; set; }
    }
}
