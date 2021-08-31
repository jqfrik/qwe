using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace ForksWebAPI.Common.Client
{
  [DebuggerDisplay("EventId:{EventId} BetId:{BetId} SportId:{SportId} ")]
  [Serializable]
  public class BetData
  {
    [JsonProperty("euo")]
    public string Url { get; set; }

    [JsonProperty("euo2")]
    public string Url2 { get; set; }

    [JsonProperty("eido")]
    public string EventId { get; set; }

    [JsonProperty("eido2")]
    public string EventId2 { get; set; }

    [JsonProperty("bido")]
    public string BetId { get; set; }

    [JsonProperty("bido2")]
    public string BetId2 { get; set; }

    [JsonProperty("sid")]
    public string SportId { get; set; }

    [JsonProperty("bno")]
    public string Bet1Name { get; set; }

    [JsonProperty("bno2")]
    public string Bet2Name { get; set; }

    public string EventTeams { get; set; }
  }
}
