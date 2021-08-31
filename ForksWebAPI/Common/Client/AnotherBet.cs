using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForksWebAPI.Common.Client
{
    [Serializable]
    public class AnotherBet
    {
        [JsonProperty("booker_id")]
        public EBookmakers Bookmaker { get; set; }

        [JsonProperty("name")]
        public string BookmakerName { get; set; }

        [JsonProperty("coef")]
        public Decimal Coef { get; set; }

        [JsonProperty("moved")]
        public EDirection Direction { get; set; }

        [JsonProperty("event_name")]
        public string Teams { get; set; }

        [JsonProperty("league")]
        public string League { get; set; }

        [JsonProperty("bet")]
        public string BetValue { get; set; }

        [JsonProperty("bet_id")]
        public string OtherData { get; set; }

        [JsonProperty("event_id")]
        public string EvId { get; set; }

        [JsonProperty("val")]
        public double Parametr { get; set; }

        [JsonProperty("score")]
        public string Score { get; set; }

        [JsonProperty("k")]
        public string K { get; set; }

        public EAnotherBetNumber AnotherBetNumber { get; set; }

        [JsonProperty("eventUrl")]
        public string Url { get; set; }

        public string GetClearScore()
        {
            if (string.IsNullOrWhiteSpace(this.Score))
                return string.Empty;
            return this.Score.Replace("<b>", "").Replace("</b>,", " | ").Replace("</b>", " | ");
        }
    }
}
