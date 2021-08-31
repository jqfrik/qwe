using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Common.Client;

namespace WpfApp1.Common
{
    public static class EnumsHelper
    {
        public static string ToNormalString(this EBookmakers book)
        {
            return book.ToString().Replace("_", "");
        }
    }
}
