using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace ForksWebAPI.Common.Client
{
  [DebuggerDisplay("b:{Bookmaker} c:{Coef} bt:{BetType} Fc:{ForksCount}")]
  [Serializable]
  [ComplexType]
   public class Bet
  {
    [Key]
    public long Id { get; set; }
    public EBookmakers Bookmaker { get; set; }

    public Decimal Coef { get; set; }

    public EDirection Direction { get; set; }

    public EBetType BetType { get; set; }

    public ESport Sport { get; set; }

    public double Parametr { get; set; }

    public string BetValue { get; set; }

    public int ForksCount { get; set; }

    public string EvId { get; set; }

    public string OtherData { get; set; }

    public string Team { get; set; }

    public string MatchData { get; set; }

    public string Url { get; set; }

    public bool IsReq { get; set; }

    public bool IsInitiator { get; set; }
  }
}
