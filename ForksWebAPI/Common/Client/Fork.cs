
using ForksWebAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace ForksWebAPI.Common.Client
{
  [DebuggerDisplay("id:{Id} p:{Profit}% s:{Sport} t:{Time} bt:{BetType}")]
  [Serializable]
  public class Fork
  {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    
    public long Id { get; set; }

    public int ForkId { get; set; }

    public int UpdateCount { get; set; }

    public TimeSpan Time { get; set; }

    public Decimal Profit { get; set; }

    public ESport Sport { get; set; }

    public EBetType BetType { get; set; }

    public string Bookmakers { 
            get {
                return string.Format("{0}[{1}] - {2}[{3}]", (object)this.OneBet?.Bookmaker.ToNormalString(), (object)this.OneBet?.ForksCount, (object)this.TwoBet?.Bookmaker.ToNormalString(), (object)this.TwoBet?.ForksCount);
            } 
        }
    public string Teams
    {
      get
      {
        return this.OneBet?.Team;
      }
    }

    public string Other
    {
      get
      {
        return this.OneBet?.MatchData;
      }
    }

    public Bet OneBet { get; set; }

    public Bet TwoBet { get; set; }

    public DateTime Creted { get;}

    public string CridId { get; set; }

    public string K1 { get; set; }

    public string K2 { get; set; }

    public string Elid { get; set; }

    public Fork(long id)
    {
            this.Creted = DateTime.Now;
      this.Id = id;
    }

    public Fork()
    {
            this.Creted = DateTime.Now;
        }

    public override string ToString()
    {
      return string.Format("{0}% {1} {2} - {3} {4} - {5} {6} {7}", (object) this.Profit, (object) this.Bookmakers, (object) this.OneBet.Coef, (object) this.TwoBet.Coef, (object) this.OneBet.BetValue, (object) this.TwoBet.BetValue, (object) this.Teams, (object) this.Other);
    }

    public override int GetHashCode()
    {
      return this.Id.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      long? id1 = (obj as Fork)?.Id;
      long id2 = this.Id;
      return id1.GetValueOrDefault() == id2 & id1.HasValue;
    }

    public void SwapBet()
    {
      Bet oneBet = this.OneBet;
      this.OneBet = this.TwoBet;
      this.TwoBet = oneBet;
    }

    public void ReplaceBet(AnotherBet anotherBet)
    {
      Bet normalBet = this.ConvertToNormalBet(anotherBet);
      if (anotherBet.AnotherBetNumber == EAnotherBetNumber.One)
      {
        normalBet.BetType = this.OneBet.BetType;
        this.K1 = anotherBet.K;
        this.OneBet = normalBet;
      }
      else
      {
        normalBet.BetType = this.TwoBet.BetType;
        this.K2 = anotherBet.K;
        this.TwoBet = normalBet;
      }
      ++this.Id;
    }

    public Bet ConvertToNormalBet(AnotherBet bet)
    {
      Bet bet1 = new Bet()
      {
        Coef = bet.Coef,
        Bookmaker = bet.Bookmaker,
        EvId = bet.EvId,
        OtherData = bet.OtherData,
        BetValue = bet.BetValue,
        Direction = bet.Direction,
        ForksCount = 0,
        MatchData = " (" + bet.League + ") " + bet.GetClearScore()
      };
      bet1.Direction = bet.Direction;
      bet1.Parametr = bet.Parametr;
      bet1.Team = bet.Teams;
      bet1.Url = bet.Url;
      return bet1;
    }
    }
}