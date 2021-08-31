using CefSharp;
using CefSharp.Internals;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WpfApp1.Common.Client;
using WpfApp1.Common.Server;
using WpfApp1.Shared;

namespace WpfApp1.ViewModel
{
    public class StartBotViewModel
    {
        public ListBox Box;
        public TextBlock bk;
        public TextBlock stake;
        public TextBlock sport;
        public TextBlock eventT;
        public TextBlock score;
        private Dispatcher dispatcher;
        private Window w1;
        private Window w2;
        public ChromiumWebBrowser web1;
        public ChromiumWebBrowser web2;
        public JsWorker worker;
        public JsWorker secondWorker;
        public string jsCodeLigaStavok = File.ReadAllText("../../JsCode/LigaStavok.js");
        public string jsCodeMarathon = File.ReadAllText("../../JsCode/Marathon.js");

        public bool botIsStart = false;

        public ICommand StartBrowserCom { get; set; }
        public ICommand LoginMarathonCom { get; set; }
        public ICommand SetStakeInMarathonCom { get; set; }
        public ICommand DevtoolsCom { get; set; }
        public ICommand BotStartCom { get; set; }

        ApiServer apiServer;

        public StartBotViewModel(ListBox Box,Dispatcher dispatcher,TextBlock Bk, TextBlock Stake, TextBlock Sport, TextBlock Event, TextBlock Score)
        {
            this.bk = Bk;
            this.stake = Stake;
            this.sport = Sport;
            this.eventT = Event;
            this.score =  Score;

        StartBrowserCom = new RealCommand(this.StartAllBrowsers, () => CanExecute);
            LoginMarathonCom = new RealCommand(this.LoginTry, () => CanExecute);
            SetStakeInMarathonCom = new RealCommand(this.SetStake, () => CanExecute);
            DevtoolsCom = new RealCommand(this.Devtools, () => CanExecute);
            BotStartCom = new RealCommand(this.BotStartExec, () => CanExecute);
            apiServer = new ApiServer();
            worker = new JsWorker();
            secondWorker = new JsWorker();
            this.dispatcher = dispatcher;
            this.Box = Box;
        }
        public bool CanExecute
        {
            get
            {
                // check if executing is allowed, i.e., validate, check if a process is running, etc. 
                return true;
            }
        }
        public void Devtools()
        {
            secondWorker.browser.ShowDevTools();
        }
        public void SetStake()
        {
             secondWorker.setCustomStake();
        }
        public void LoginTry()
        {
                secondWorker.ExecuteLoginTry();
            //worker.ExecuteLoginTry();
        }
        public  void StartAllBrowsers()
        {
            Cef.EnableHighDPISupport();
            CefSettings s = new CefSettings();
            Cef.Initialize(s);
            web1 = new ChromiumWebBrowser("https://www.ligastavok.ru/");
            web2 = new ChromiumWebBrowser("https://www.marathonbet.ru/su/?cpcids=all&liveTab=popular");

            CefSharpSettings.WcfEnabled = true;
            web1.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            web2.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;

            worker.dispatcher = this.dispatcher;
            secondWorker.dispatcher = this.dispatcher;

            worker.Logger = Box;
            secondWorker.Logger = Box;

            worker.browser = web1;
            secondWorker.browser = web2;

            web1.RequestHandler = new RequestHandlerRelease(jsCodeLigaStavok);
            web2.RequestHandler = new RequestHandlerRelease(jsCodeMarathon);


            web1.JavascriptObjectRepository.ResolveObject += (swe, e) =>
            {
                if (e.ObjectName == JavascriptObjectRepository.LegacyObjects)
                {
                    //ADD YOUR LEGACY
                    e.ObjectRepository.Register("worker", worker, isAsync: false);
                }
            };
            web2.JavascriptObjectRepository.ResolveObject += (swe, e) =>
            {
                if (e.ObjectName == JavascriptObjectRepository.LegacyObjects)
                {
                    //ADD YOUR LEGACY
                    e.ObjectRepository.Register("worker", secondWorker, isAsync: false);
                }
            };
            web1.ExecuteScriptAsyncWhenPageLoaded(jsCodeLigaStavok);
            web2.ExecuteScriptAsyncWhenPageLoaded(jsCodeMarathon);


            w1 = new Window();
            w1.Content = web1;
            w1.Show();

            w2 = new Window();
            w2.Content = web2;
            w2.Show();

            web1.IsBrowserInitializedChanged += (sender, args) =>
            {
                if (web1.IsBrowserInitialized)
                {
                      //web1.ShowDevTools();
                }
            };
            web2.IsBrowserInitializedChanged += (sender, args) =>
            {
                if (web2.IsBrowserInitialized)
                {
                    //Рабочая установка прокси(можно проверить через яндексометр)
                    //await ChromeHelpers.ChromeHelpers.SetProxy(web2, "SOCKS5", "46.101.102.106", "4444");
                    web2.ShowDevTools();
                }
            };
        }
        public void BotStartExec()
        {//при клике на кнопку запуск,запускаются 2 браузера с двумя бк
            botIsStart = !botIsStart;
            StartAllBrowsers();

            Task login = new Task(async ()=> {
                await Task.Delay(5000);
                LoginTry();
            });
            login.Start();
            //а здесь должна быть зацикленность чтобы вилка приходила,попадала в воркеры,как значение и происходила ставка
            //А потом по новой получение вилок 
            Task update = new Task(async () => {
                await Task.Delay(25000);
                while (botIsStart)
                {
                    secondWorker.AddMessageInLogger("Получаем вилки");
                    List<Fork> forks = null;
                    do
                    {
                        forks = GetForks();
                    } while (forks.Count == 0);
                    secondWorker.AddMessageInLogger($"Получили вилки в количестве {forks.Count}") ;
                    Fork lastFork = forks[forks.Count - 1];
                    InitInfoAboutLastFork(lastFork);
                    InitializeWorkers(lastFork);
                    secondWorker.AddMessageInLogger("Инициализировали воркеры");
                    await Task.Delay(5000);
                    secondWorker.LoadUrl(secondWorker.Url);
                    secondWorker.AddMessageInLogger($"Загрузили Url {secondWorker.Url}");
                    await Task.Delay(10000);
                    SetStake();
                    secondWorker.AddMessageInLogger("Сделали ставку");
                    await Task.Delay(5000);
                }
            });
            update.Start();
            update.Wait();
        }
        public void InitInfoAboutLastFork(Fork fork)
        {
            this.dispatcher.Invoke(() =>
            {
                this.bk.Text = fork.Bookmakers;
                this.stake.Text = fork.TwoBet.BetName;
                this.sport.Text = fork.TwoBet.Sport.ToString();
                this.eventT.Text = fork.TwoBet.PositiveBetType.ToString();
                this.score.Text = fork.TwoBet.Score;
            });
        }
        public void InitializeWorkers(Fork fork)
        {
            Bet actFork = null;
            if (fork.OneBet.Url.ToLower().Contains("marathon"))
            {
                actFork = fork.OneBet;
            }
            if (fork.TwoBet.Url.ToLower().Contains("marathon"))
            {
                actFork = fork.TwoBet;
            }
            
            worker.BetValue = fork.OneBet.BetValue;
            worker.SumStake = 20;
            worker.Url = fork.OneBet.Url;
            
            //secondWorker
            secondWorker.BetValue = actFork.BetValue;
            secondWorker.SumStake = 20;
            secondWorker.Url = actFork.Url.Replace("com","ru");
            
        }
        public List<Fork> GetForks()
        {
            return apiServer.GetForks();
        }
    }
}
