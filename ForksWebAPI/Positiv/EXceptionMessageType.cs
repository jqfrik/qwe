using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveForks.Admin.Providers.Positiv
{
    [Flags]
    public enum EXceptionMessageType
    {
        NotLogin = 1,
        AlreadyLogin = 2,
        NonSubscribe = AlreadyLogin | NotLogin, // 0x00000003
        BetNotFound = 4,
        BadAuthData = BetNotFound | NotLogin, // 0x00000005
        NotFirstLogin = BetNotFound | AlreadyLogin, // 0x00000006
        Error8 = NotFirstLogin | NotLogin, // 0x00000007
        UserBaned = 8,
        ErrorCheckExSubscribe = UserBaned | NotLogin, // 0x00000009
        Unknown = 153, // 0x00000099
    }
}