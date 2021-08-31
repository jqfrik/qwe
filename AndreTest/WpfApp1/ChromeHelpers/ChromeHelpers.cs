using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp1.ChromeHelpers
{
    public class ChromeHelpers
    {
        public static async Task<bool> SetProxy(ChromiumWebBrowser chromeBrowser, string schema, string host, string port)
        {
            return (bool) await Cef.UIThreadTaskFactory.StartNew(delegate {
                var rc = chromeBrowser.GetBrowser().GetHost().RequestContext;
                var v = new Dictionary<string, object>();
                v["mode"] = "fixed_servers";
                v["server"] = $"{schema}://{host}:{port}";
                string error;
                bool success = rc.SetPreference("proxy", v, out error);
                if (success)
                    return true;
                return false;
            });
        }
    }
}
