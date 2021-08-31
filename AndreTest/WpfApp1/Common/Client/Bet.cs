using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Common.Client
{
    [DebuggerDisplay("b:{Bookmaker} c:{Coef} bt:{PositiveBetType} Fc:{ForksCount}")]
    [Serializable]
    public class Bet
    {
        public EBookmakers Bookmaker { get; set; }

        public Decimal Coef { get; set; }

        public EDirection Direction { get; set; }

        public EPositiveBetType PositiveBetType { get; set; }

        public EBetType BetType { get; set; }

        public Decimal Parameter { get; set; }

        public string BetValue { get; set; }

        public int ForksCount { get; set; }

        public string EvId { get; set; }

        public string BetId { get; set; }

        public string BetName { get; set; }

        public string PositiveBetId { get; set; }

        public string Team { get; set; }

        public string Liga { get; set; }

        public string Score { get; set; }

        public string Url { get; set; }

        public bool IsReq { get; set; }

        public bool IsInitiator { get; set; }

        public string Duration { get; set; }

        public bool IsBreak { get; set; }

        public string Period { get; set; }

        public string K { get; set; }

        public string PositiveEventId { get; set; }

        public ESport Sport { get; set; }
        public string OtherData { get; set; }
        /*
        public BetData GetBetData()
        {
            return new BetData()
            {
                SportId = ((int)(this.Sport - 100)).ToString()
            };
        }
        */

        public bool IsRu { get; set; }
    }
}
