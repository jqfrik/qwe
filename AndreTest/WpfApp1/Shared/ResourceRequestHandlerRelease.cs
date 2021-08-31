using CefSharp;
using CefSharp.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Shared
{
    public class ResourceRequestHandlerRelease: ResourceRequestHandler
    {
        private string JsCode;
        public ResourceRequestHandlerRelease(string JsCode)
        {
            this.JsCode = JsCode;
        }
        protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            if (!string.IsNullOrWhiteSpace(JsCode))
            {
                browser.MainFrame.ExecuteJavaScriptAsync(JsCode, request.Url, 1);
            }
            return base.OnBeforeResourceLoad(chromiumWebBrowser, browser, frame, request, callback);
        }
    }
}
