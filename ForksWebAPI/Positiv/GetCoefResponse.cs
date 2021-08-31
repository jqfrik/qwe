using ForksWebAPI.Common.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveForks.Admin.Providers.Positiv
{
    internal sealed class GetCoefResponse
    {
        [JsonProperty("coefs1")]
        public List<AnotherBet> Coefs1 { get; set; }

        [JsonProperty("coefs2")]
        public List<AnotherBet> Coefs2 { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
