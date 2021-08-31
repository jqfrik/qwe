using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1
{
    public class JsWorker
    {
        public ChromiumWebBrowser browser;
        //public IJavascriptCallback setSumStake;
        //public IJavascriptCallback doStake;
        public IJavascriptCallback loginTry;
        public IJavascriptCallback setStake;
        public IJavascriptCallback getBalance;
        public Dispatcher dispatcher;
        //private IJavascriptCallback _checkCouponLoading { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        public string BetValue { get; set; }
        public decimal SumStake { get; set; }
        public ListBox Logger { get; set; }
        public void setLoginTry(IJavascriptCallback loginTry)
        {
            this.loginTry = loginTry;
        }
        public void setGetBalance(IJavascriptCallback getBalance)
        {
            this.getBalance = getBalance;
            
        }
        public void ExecuteLoginTry()
        {
            Task.Factory.StartNew(async () => {
                await this.loginTry.ExecuteAsync();
            }).ContinueWith((e) => {

            });
        }
        public void SetStakeSetter(IJavascriptCallback setStake)
        {
            this.setStake = setStake;
            //AddMessageInLogger("SetCallback сработал");
        }
        public async Task GetBalance()
        {
            JavascriptResponse responseBalance = await this.getBalance.ExecuteAsync();
            AddMessageInLogger(responseBalance.Result as string);
        }
        [JavascriptIgnore]
        public void setCustomStake()
        {
            if(this.setStake == null)
            {
                throw new ArgumentException("setStake == null");
            }
            if (!this.setStake.CanExecute)
            {
                //AddMessageInLogger("setStake не может выполниться");
            }
            Task.Factory.StartNew(async () => {
                await this.setStake.ExecuteAsync();
            }).ContinueWith((e)=> { 
            
            });
            //JavascriptResponse resp1 = await this.setStake.ExecuteAsync();
        }
        public string executeJsCodeFromJs()
        {
            this.AddMessageInLogger("функция выплняется из js");
            return "succes";
        }

        public void AddMessageInLogger(string log)
        {
            dispatcher.Invoke(() =>
            {
                var lg = this.Logger;
                var items = lg.Items;
                items.Add(log);
            });
        }
        public async void LoadUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(url);
            }
            if (this.browser != null)
            {
                await this.browser.LoadUrlAsync(url);
            }
        }
        /*
        public void SetCallbacks(IJavascriptCallback setSumStake,IJavascriptCallback doStake)
        {
            this.setSumStake = setSumStake;
            this.doStake = doStake;
        }
        public async Task DoStake()
        {
            if(this.doStake == null)
            {
                throw new ArgumentNullException("doStake == null");
            }
            if (!this.doStake.CanExecute)
            {
                AddMessageInLogger("doStake не может выполниться");
            }
            JavascriptResponse jsResponse = await this.doStake.ExecuteAsync((object[])Array.Empty<object>());
            if (!jsResponse.Success)
            {
                AddMessageInLogger("doStake not success");
            }
        }
        public async Task SetSumStake()
        {
            if (this.setSumStake == null)
            {
                throw new ArgumentNullException("doStake == null");
            }
            if (!this.setSumStake.CanExecute)
            {
                AddMessageInLogger("doStake не может выполниться");
            }
            JavascriptResponse jsResponse = await this.setSumStake.ExecuteAsync((object[])Array.Empty<object>());
            if (!jsResponse.Success)
            {
                AddMessageInLogger("doStake not success");
            }
        }
        */


    }
}
