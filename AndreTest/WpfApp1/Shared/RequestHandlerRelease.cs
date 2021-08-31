using CefSharp;
using CefSharp.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Shared
{
    public class RequestHandlerRelease : RequestHandler
    {
        public string JsCode;
        public RequestHandlerRelease(string JsCode)
        {
            this.JsCode = JsCode;
        }
        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            return new ResourceRequestHandlerRelease(JsCode);
        }
    }
}
