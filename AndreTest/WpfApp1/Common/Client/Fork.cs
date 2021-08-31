using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Common.Client
{
    public class Fork
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public long Id { get; set; }

        public TimeSpan Time { get; set; }

        public Decimal Profit { get; set; }

        public ESport Sport { get; set; }

        public EPositiveBetType PositiveBetType { get; set; }

        public string Bookmakers
        {
            get
            {
                return string.Format("{0}[{1}] - {2}[{3}]", (object)this.OneBet.Bookmaker.ToNormalString(), (object)this.OneBet.ForksCount, (object)this.TwoBet.Bookmaker.ToNormalString(), (object)this.TwoBet.ForksCount);
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
                return this.OneBet?.Liga;
            }
        }

        public Bet OneBet { get; set; }

        public Bet TwoBet { get; set; }

        public DateTime Created { get; }

        public string CridId { get; set; }
        public string EventLigaId { get; set; }

        public int UpdateCount { get; set; }

        public Fork(long id)
        {
            this.Created = DateTime.Now;
            this.Id = id;
        }

        public Fork()
        {
            this.Created = DateTime.Now;
        }

        public override string ToString()
        {
            return string.Format("{0}% {1} {2} - {3} {4} - {5} {6} {7}", (object)this.Profit, (object)this.Bookmakers, (object)this.OneBet.Coef, (object)this.TwoBet.Coef, (object)this.OneBet.BetValue, (object)this.TwoBet.BetValue, (object)this.Teams, (object)this.Other);
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

        public bool IsSwap { get; private set; }

        public bool IsBreak { get; set; }

        
    }
}
