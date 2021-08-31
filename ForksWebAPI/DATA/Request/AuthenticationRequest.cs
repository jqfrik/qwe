using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForksWebAPI.DATA.Request
{
    public class AuthenticationRequest
    {
        public string Key { get; set; }

        public string Password { get; set; }
    }
}
