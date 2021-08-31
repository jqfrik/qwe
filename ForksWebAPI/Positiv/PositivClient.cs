using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using ForksWebAPI.Common.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xNet;

namespace LiveForks.Admin.Providers.Positiv
{
    public  class PositivClient : IServiceForksClient
    {
        public PositivClient()
        {

        }
        public PositivClient(ILogger<PositivClient> log)
        {
            this._log = log;
        }
        private long Id = 1U;
        private readonly ILogger<PositivClient> _log;
        private string _apiUrl = "https://positivebet.com";
        private readonly CultureInfo _culture = CultureInfo.GetCultureInfo("ru-Ru");
        private Subscribe _activeSubscribe = new Subscribe()
        {
            State = ESubscribeState.End
        };
        private HttpRequest _req;
        private HtmlParser _parse;
        private string _token;
        private bool _isSubscribed;
        private string _login;
        private string _password;
        public int? _filterId;
        private LoggerHelp _logger;
        private int _betDataRequestCount;

        public bool IsLogin { get; private set; }

        public bool _isFirstLogin { get; set; } = true;

        public string LoginName
        {
            get
            {
                return this._login;
            }
        }

        public bool IsHaveExSubscribe { get; set; }

        public List<string> GetUrlFromPositive(Fork fork)
        {
            if (IsLogin)
            {
                List<string> bets = new List<string>();
                string urlFirstBetDirty = this._req.Get(_apiUrl + fork.OneBet.Url,null).ToString().Split(" ")[3].Substring("url=".Length);
                string urlFirstBetClean = urlFirstBetDirty.Substring(0, urlFirstBetDirty.Length - 1);
                string urlSecondBetDirty = this._req.Get(_apiUrl + fork.TwoBet.Url,null).ToString().Split(" ")[3].Substring("url=".Length);
                string urlSecondBetClean = urlSecondBetDirty.Substring(0, urlSecondBetDirty.Length - 1);
                if (string.IsNullOrWhiteSpace(urlFirstBetClean))
                {
                    throw new ArgumentNullException("Пришедший Url от первой Бк пуст");
                }
                if (string.IsNullOrWhiteSpace(urlSecondBetClean))
                {
                    throw new ArgumentNullException("Пришедший Url от второй Бк пуст");
                }
                bets.Add(urlFirstBetClean);
                bets.Add(urlSecondBetClean);
                return bets;
            }
            return null;
        }

        public void Login(string login, string password)
        {
            this._isFirstLogin = true;
            if (string.IsNullOrEmpty(login))
                throw new ArgumentNullException("login == null");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password == null");
           // this._logger = new LoggerHelp(true, login);
            this._login = login;
            this._password = password;
            this.CheckAndCreateRequest();
            string source1 = this._req.Get(this._apiUrl + "/ru/user/login", (RequestParams)null).ToString();
            if (this._parse == null)
                this._parse = new HtmlParser();
            IElement element1 = this._parse.Parse(source1).GetElementsByName("YII_CSRF_TOKEN").FirstOrDefault<IElement>();
            if (element1 == null)
                throw new ArgumentNullException("YII_CSRF_TOKEN == null");
            this._token = element1.Attributes["value"].Value;
            RequestParams reqParams = new RequestParams();
            reqParams["YII_CSRF_TOKEN"] = (object)this._token;
            reqParams["UserLogin[username]"] = (object)login;
            reqParams["UserLogin[password]"] = (object)password;
            reqParams["UserLogin[rememberMe]"] = (object)"0";
            //reqParams["UserLogin[rememberMe]"] = (object)"1";
            reqParams["yt0"] = (object)"";
            try
            {
                this._req.Post(this._apiUrl + "/ru/user/login", reqParams, false);
            }
            catch
            {
                if (this._req.Response.StatusCode != HttpStatusCode.Found)
                    throw;
            }
            string source2 = this._req.Response.ToString();
            if (source2.Contains("заблокирован."))
                throw new PositivException(EXceptionMessageType.UserBaned);
            IHtmlDocument htmlDocument = this._parse.Parse(source2);
            if (htmlDocument.GetElementsByName("YII_CSRF_TOKEN").FirstOrDefault<IElement>() != null && !this._req.Response.HasRedirect && !this._req.Response.Address.ToString().Contains("bets/index"))
                throw new PositivException(EXceptionMessageType.BadAuthData);
            if (!source2.Contains("/ru/user/logout"))
                throw new PositivException(EXceptionMessageType.BadAuthData);
            IElement element2 = htmlDocument.GetElementsByName("esa").FirstOrDefault<IElement>();
            if (element2 == null || !element2.HasAttribute("value"))
            {
                if (!this._req.Response.HasRedirect && !this._req.Response.Address.ToString().Contains("bets/index"))
                    throw new PositivException(EXceptionMessageType.ErrorCheckExSubscribe);
            }
            else if (element2.Attributes["value"].Value == "1" || element2.Attributes["value"].Value == "2")
                this.IsHaveExSubscribe = true;
            this.ParseFilterIdFromBetsPage(this._req.Get(this._apiUrl + "/ru/bets/index", (RequestParams)null).ToString());
            this._req.Get(string.Format("{0}/settings/filteractivate?fl[]={1}&akid=0&isAjax=true", (object)this._apiUrl, (object)this._filterId), (RequestParams)null).ToString();
            this.IsLogin = true;
            this._isFirstLogin = false;
            this._betDataRequestCount = 0;
        }

        private void ParseFilterIdFromBetsPage(string htmlPageData)
        {
            IElement elementById = new HtmlParser().Parse(htmlPageData).GetElementById("ddFilters");
            if (elementById == null)
                throw new PositivException(EXceptionMessageType.BadAuthData);
            IElement element = elementById.Children.FirstOrDefault<IElement>();
            if (element == null)
                throw new ArgumentException("Ошибка. Фильтр отсутствует");
            if (element.TagName.ToLower() != "option")
                throw new ArgumentException("Ошибка.Выбран не <option> ! ");
            if (!element.HasAttribute("value"))
                throw new ArgumentException("Ошибка. Почему то нет фильтра!");
            int result;
            if (!int.TryParse(element.Attributes["value"].Value, NumberStyles.Any, (IFormatProvider)this._culture, out result))
                throw new ArgumentException("Ошибка. В фильтре не число!");
            this._filterId = new int?(result);
        }

        public void ReLogin()
        {
            if (this._isFirstLogin)
                throw new PositivException(EXceptionMessageType.NotFirstLogin);
            this._req.Cookies = new CookieDictionary(false);
            this.Login(this._login, this._password);
        }

        public List<BetData> GetBetsData(Fork fork)
        {
            List<BetData> betDataList = new List<BetData>();
            if (!this.IsLogin)
                throw new PositivException(EXceptionMessageType.NotLogin);


            RequestParams requestParams = new RequestParams();
            requestParams["b"] = (object)(int)fork.OneBet.Bookmaker;
            requestParams["e"] = (object)fork.OneBet.EvId;
            requestParams["bid"] = (object)fork.OneBet.OtherData.Replace("+", "%2B");

            requestParams["b2"] = (object)(int)fork.TwoBet.Bookmaker;
            requestParams["e2"] = (object)fork.TwoBet.EvId;
            requestParams["bid2"] = (object)fork.TwoBet.OtherData.Replace("+", "%2B");
            requestParams["k1"] = (object)fork.K1;
            requestParams["k2"] = (object)fork.K2;
            requestParams["elid"] = (object)fork.Elid;
            requestParams["crid"] = (object)fork.CridId;
            //Стартовый запрос
            string response = this._req.Post(this._apiUrl + "/bets/bet", requestParams, false).ToString();
            //Ошибка на стороне клиента
            //string response = this._req.Post(this._apiUrl + "/ru/bets", requestParams, false).ToString();
            //Возвращает мета тег в котором есть url для одной бк
            //string response = this._req.Get("https://positivebet.com" + $"/ru/bets/go/e/{fork.OneBet.EvId}/b/{(int)fork.OneBet.Bookmaker}/elid/{fork.Elid}/k/{fork.K1}").ToString();



            if (response.Contains("alreadyUsed"))
            {
                this.IsLogin = false;
                this._isSubscribed = false;
                throw new PositivException(EXceptionMessageType.AlreadyLogin);
            }
            if (response.Contains("bet is not found."))
                throw new PositivException(EXceptionMessageType.BetNotFound);
            if (response.Contains("error: 7"))
            {
                if (this._activeSubscribe.LocalCalcTimeLeft().TotalSeconds > 0.0)
                {
                    //this._logger?.SendError7(response, LoggerHelp.ConverToString(requestParams), this._betDataRequestCount);
                }
                throw new PositivException(EXceptionMessageType.NonSubscribe);
            }
            if (response.Contains("error: ") && response.Trim().Length < 20)
            {
                //this._logger?.SendErrorUnknown(response, LoggerHelp.ConverToString(requestParams));
                throw new PositivException("Ошибка PositiveBet на запрос информации по вилке: " + response);
            }
            BetData betData;
            try
            {
                betData = JsonConvert.DeserializeObject<BetData>(response);
            }
            catch (Exception ex)
            {
                //this._logger?.SendBadJsonResponse(response, LoggerHelp.ConverToString(requestParams));
                throw new PositivException("Ошибка разбора ответа от PositiveBet на запрос информации по вилке:" + Environment.NewLine + ex.Message + Environment.NewLine + response);
            }
            betDataList.Add(new BetData()
            {
                BetId = betData.BetId,
                EventId = betData.EventId,
                SportId = betData.SportId,
                Url = betData.Url,
                Bet1Name = betData.Bet1Name,
                EventTeams = fork.OneBet.Team
            });
            betDataList.Add(new BetData()
            {
                BetId = betData.BetId2,
                EventId = betData.EventId2,
                SportId = betData.SportId,
                Url = betData.Url2,
                Bet1Name = betData.Bet2Name,
                EventTeams = fork.TwoBet.Team
            });
            if (this.CheckEmptyUrl(betDataList[0]))
            {
                //this._logger?.SendEmptyUrl(response, LoggerHelp.ConverToString(requestParams));
                throw new PositivException("Для " + fork.OneBet.Bookmaker.ToNormalString() + " позитив не прислал URL");
            }
            if (this.CheckEmptyUrl(betDataList[1]))
            {
                //this._logger?.SendEmptyUrl(response, LoggerHelp.ConverToString(requestParams));
                throw new PositivException("Для " + fork.TwoBet.Bookmaker.ToNormalString() + " позитив не прислал URL");
            }
            ++this._betDataRequestCount;
            return betDataList;
        }

        private bool CheckEmptyUrl(BetData betData)
        {
            return string.IsNullOrWhiteSpace(betData.Url) || betData.Url == "no_data_found";
        }

        public List<BetData> GetBetsDataWithoutSubscribe(Fork fork)
        {
            this._req.AllowAutoRedirect = false;
            if (fork == null)
                throw new ArgumentNullException("fork==null");
            List<BetData> betDataList = new List<BetData>();
            Thread.Sleep(100);
            BetData betData1 = new BetData();
            string resp = this._req.Get(this._apiUrl + fork.OneBet.Url, (RequestParams)null).ToString();
            File.WriteAllText("resp12.html", resp);
            IElement element1 = this._parse.Parse(this._req.Get(this._apiUrl + fork.OneBet.Url, (RequestParams)null).ToString()).QuerySelector("meta");
            if (element1 != null)
            {
                string str = element1.Attributes["content"].Value.Replace("0; url=", "");
                betData1.Url = str;
            }
            betData1.EventTeams = fork.OneBet.Team;
            betDataList.Add(betData1);
            Thread.Sleep(300);
            BetData betData2 = new BetData();
            IElement element2 = this._parse.Parse(this._req.Get(this._apiUrl + fork.TwoBet.Url, (RequestParams)null).ToString()).QuerySelector("meta");
            if (element2 != null)
            {
                string str = element2.Attributes["content"].Value.Replace("0; url=", "");
                betData2.Url = str;
            }
            betDataList.Add(betData2);
            betData2.EventTeams = fork.TwoBet.Team;
            Thread.Sleep(200);
            return betDataList;
        }

        public List<BetData> GetBetsDataSmart(Fork fork)
        {
            if (this._isSubscribed)
                return this.GetBetsData(fork);
            return this.GetBetsDataWithoutSubscribe(fork);
        }

        public List<Fork> GetForks()
        {
            List<Fork> forkList = new List<Fork>();
            try
            {
                this._req.ConnectTimeout = 2000;
                this._req.ReadWriteTimeout = 2000;
                this._req.KeepAliveTimeout = 2000;
                this._req.AllowAutoRedirect = true;
                if (!this.IsLogin)
                    throw new PositivException(EXceptionMessageType.NotLogin);
                this._req.AddHeader("X-Requested-With", "XMLHttpRequest");
                this._req.AddHeader("Accept", "*/*");
                this._req.EnableEncodingContent = true;
                //  this._req.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                this._req.AddHeader("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
                string address = string.Format("{0}/ru/bets/index?ajax=gridBets&au=false&crid=&fl%5B%5D={1}&isAjax=true&markNewEvent=false&perPage=30", (object)this._apiUrl, (object)this._filterId);
                if (!this._filterId.HasValue)
                    address = this._apiUrl + "/ru/bets/index?markNewEvent=false&perPage=30&crid=&ajax=gridBets";
                string source = this._req.Get(address, (RequestParams)null).ToString();
                if (source.Contains("alreadyUsed"))
                {
                    this.IsLogin = false;
                    this._isSubscribed = false;
                    throw new PositivException(EXceptionMessageType.AlreadyLogin);
                }
                if (source.Contains("Нет результатов."))
                    return forkList;
                IHtmlCollection<IElement> children = this._parse.Parse(source)?.Body?.Children[0]?.Children[0]?.Children[1].Children;
                if (children == null)
                    throw new ArgumentException("Не удалось получить таблицу с вилками");
                foreach (IElement element in (IEnumerable<IElement>)children)
                {
                    if (!string.IsNullOrWhiteSpace(element.Id))
                    {
                        //long id = long.Parse(element.Id, (IFormatProvider)this._culture);
                        //Была проверка,если Id = 0,то пропускаем вилку
                        //Теперь на позитиве все Id равн
                        if(!element.Children[3].TextContent.Contains("Событие скрыто") && !element.Children[3].TextContent.Contains("Event has been hidden"))
                        {
                            Fork fork = new Fork(Id)
                            {
                                Profit = Decimal.Parse(element.Children[1].Children[0].TextContent.Replace(".", ","), (IFormatProvider)this._culture)
                            };
                            try
                            {
                                fork.Sport = EnumsHelper.SportParse(element.Children[0].Children[2].Attributes["title"].Value);
                            }
                            catch
                            {
                                continue;
                            }
                            string[] strArray = element.Children[0].Children[3].TextContent.Split(' ');
                            int minutes = 0;
                            int seconds;
                            if (strArray.Length == 4)
                            {
                                minutes = int.Parse(strArray[0], (IFormatProvider)this._culture);
                                seconds = int.Parse(strArray[2], (IFormatProvider)this._culture);
                            }
                            else
                            {
                                if (strArray.Length != 2)
                                    throw new ArgumentException(string.Format("{0} can not parse. dt.Length={1}", (object)"Time", (object)strArray.Length));
                                seconds = int.Parse(strArray[0], (IFormatProvider)this._culture);
                            }
                            //fork.BetType = 
                            fork.Time = new TimeSpan(0, 0, minutes, seconds);
                            fork.OneBet = new Bet();
                            fork.OneBet.Bookmaker = (EBookmakers)Enum.Parse(typeof(EBookmakers), element.Children[17].TextContent, true);
                            fork.OneBet.EvId = element.Children[14].TextContent;
                            fork.OneBet.OtherData = element.Children[20].TextContent;
                            fork.OneBet.BetValue = element.Children[10].TextContent;
                            foreach (IElement child in (IEnumerable<IElement>)element.Children[5].Children[0].Children)
                            {
                                if (child.Children.Any<IElement>())
                                {
                                    if (child.Children.First<IElement>().TagName == "SUB")
                                    {
                                        try
                                        {
                                            fork.OneBet.ForksCount = string.IsNullOrWhiteSpace(child.TextContent.Trim()) ? 1 : int.Parse(child.TextContent.Trim(), (IFormatProvider)this._culture);
                                        }
                                        catch (Exception ex)
                                        {
                                            continue;
                                        }
                                    }
                                }
                                if (child.HasAttribute("data-original-title") && child.Attributes["data-original-title"].Value == "Инициатор")
                                    fork.OneBet.IsInitiator = true;
                                if (child.HasAttribute("data-original-title") && child.Attributes["data-original-title"].Value == "Движение коэффициента" && (child.HasChildNodes && child.Children[0].HasAttribute("alt")))
                                {
                                    string direct = child.Children[0].Attributes["alt"].Value;
                                    fork.OneBet.Direction = this.ParseBetDirection(direct);
                                }
                            }
                            fork.OneBet.Coef = Decimal.Parse(element.Children[6].TextContent.Replace(".", ","), (IFormatProvider)this._culture);
                            fork.OneBet.Team = element.Children[3].Children[1].TextContent;
                            fork.OneBet.Url = element.Children[3].Children[1].Attributes["href"]?.Value;
                            fork.OneBet.MatchData = element.Children[3].Children[2].TextContent;
                            fork.TwoBet = new Bet();
                            fork.TwoBet.Bookmaker = (EBookmakers)Enum.Parse(typeof(EBookmakers), element.Children[18].TextContent);
                            fork.TwoBet.EvId = element.Children[15].TextContent;
                            fork.TwoBet.OtherData = element.Children[21].TextContent;
                            fork.TwoBet.BetValue = element.Children[11].TextContent;
                            foreach (IElement child in (IEnumerable<IElement>)element.Children[5].Children[2].Children)
                            {
                                if (child.Children.Any<IElement>())
                                {
                                    if (child.Children.First<IElement>().TagName == "SUB")
                                    {
                                        try
                                        {
                                            fork.TwoBet.ForksCount = string.IsNullOrWhiteSpace(child.TextContent.Trim()) ? 1 : int.Parse(child.TextContent.Trim(), (IFormatProvider)this._culture);
                                        }
                                        catch (Exception ex)
                                        {
                                            continue;
                                        }
                                    }
                                }
                                if (child.HasAttribute("data-original-title") && child.Attributes["data-original-title"].Value == "Инициатор")
                                    fork.TwoBet.IsInitiator = true;
                                if (child.HasAttribute("data-original-title") && child.Attributes["data-original-title"].Value == "Движение коэффициента" && (child.HasChildNodes && child.Children[0].HasAttribute("alt")))
                                {
                                    string direct = child.Children[0].Attributes["alt"].Value;
                                    fork.TwoBet.Direction = this.ParseBetDirection(direct);
                                }
                            }
                            fork.TwoBet.Coef = Decimal.Parse(element.Children[7].TextContent.Replace(".", ","), (IFormatProvider)this._culture);
                            fork.TwoBet.Team = element.Children[3].Children[5].TextContent;
                            fork.TwoBet.Url = element.Children[3].Children[5].Attributes["href"]?.Value;
                            fork.TwoBet.MatchData = element.Children[3].Children[6].TextContent;
                            fork.Elid = element.Children[3].Children[1].Attributes["href"]?.Value.Split("/")[9].Trim();
                            //fork.K1 = element.Children[element.Children.Length - 2].TextContent.Trim();
                            //fork.K2 = element.Children[element.Children.Length - 1].TextContent.Trim();
                            //fork.CridId = fork.K1 + fork.K2;
                            fork.K1 = element.Children[3].Children[1].Attributes["href"]?.Value.Split("/")[(int)element.Children[3].Children[1].Attributes["href"]?.Value.Split("/").Length - 1].Trim();
                            fork.K2 = element.Children[3].Children[5].Attributes["href"]?.Value.Split("/")[(int)element.Children[3].Children[5].Attributes["href"]?.Value.Split("/").Length - 1].Trim();
                            fork.CridId = fork.K1 + fork.K2;
                            fork.OneBet.Sport = fork.TwoBet.Sport = fork.Sport;
                            forkList.Add(fork);
                            this.Id += 1U;
                        }
                    }
                }
            }catch(Exception ex)
            {
                _log.LogInformation("Исключение в GetForks ", ex.Message);
            }
            
            return forkList;
        }

        private EDirection ParseBetDirection(string direct)
        {
            if (string.IsNullOrWhiteSpace(direct))
                return EDirection.Freeze;
            string lower = direct.Trim().ToLower();
            if (lower == "up")
                return EDirection.Up;
            return lower == "down" ? EDirection.Down : EDirection.Freeze;
        }

        public bool CheckSubscribe()
        {
            if (!this.IsLogin)
                throw new PositivException(EXceptionMessageType.NotLogin);
            this._isSubscribed = this._parse.Parse(this._req.Get(this._apiUrl + "/ru/subscription/index", (RequestParams)null).ToString()).QuerySelector("div#content > div.alert.alert-success") != null;
            return this._isSubscribed;
        }

        public PositiveSubscribeInfo GetSubscribes()
        {
            PositiveSubscribeInfo positiveSubscribeInfo = new PositiveSubscribeInfo();
            positiveSubscribeInfo.Subscribes = new List<Subscribe>();
            if (!this.IsLogin)
                throw new PositivException(EXceptionMessageType.NotLogin);
            string source = this._req.Get(this._apiUrl + "/ru/options/update?tab=es", (RequestParams)null).ToString();
            bool flag = false;
            IHtmlDocument htmlDocument = this._parse.Parse(source);
            IElement element1 = htmlDocument.QuerySelector("#es .alert.alert-success");
            string[] strArray1;
            if (element1 == null)
            {
                strArray1 = (string[])null;
            }
            else
            {
                string textContent = element1.TextContent;
                if (textContent == null)
                    strArray1 = (string[])null;
                else
                    strArray1 = textContent.Trim().Replace(".", "").Split(' ');
            }
            string[] strArray2 = strArray1;
            if (strArray2 != null && strArray2.Length > 3)
            {
                int length = strArray2.Length;
                DateTimeOffset dateTimeOffset = DateTimeOffset.Parse(strArray2[length - 2] + " " + strArray2[length - 1] + "+03:00", (IFormatProvider)this._culture);
                positiveSubscribeInfo.SubscribeEndTime = dateTimeOffset.ToUniversalTime();
                flag = true;
                positiveSubscribeInfo.IsActive = true;
                this._isSubscribed = true;
            }
            if (this.LoginName.ToLower().Contains("theforks_support"))
            {
                positiveSubscribeInfo.Subscribes.Add(new Subscribe()
                {
                    Name = "ручная",
                    FreezeCount = 0,
                    IsCurrent = true,
                    PayDateTime = DateTime.Now,
                    State = ESubscribeState.Active
                });
                return positiveSubscribeInfo;
            }
            IHtmlCollection<IElement> htmlCollection = htmlDocument.QuerySelectorAll("#es #gridSubscriptionHistory tr[class]");
            if (htmlCollection.Length == 0)
                return positiveSubscribeInfo;
            foreach (IElement element2 in (IEnumerable<IElement>)htmlCollection)
            {
                Subscribe subscribe1 = new Subscribe();
                subscribe1.Name = element2.Children[0].TextContent;
                DateTimeOffset dateTimeOffset1 = DateTimeOffset.Parse(element2.Children[3].TextContent + "+03:00", (IFormatProvider)this._culture);
                Subscribe subscribe2 = subscribe1;
                DateTimeOffset dateTimeOffset2 = dateTimeOffset1.ToUniversalTime();
                DateTime dateTime1 = dateTimeOffset2.DateTime;
                subscribe2.PayDateTime = dateTime1;
                if (string.IsNullOrWhiteSpace(element2.Children[1].TextContent.Trim()) && string.IsNullOrWhiteSpace(element2.Children[2].TextContent.Trim()))
                {
                    subscribe1.State = ESubscribeState.New;
                }
                else
                {
                    if (element2.Children[2].TextContent.Contains("Приостановлена"))
                    {
                        subscribe1.IsCurrent = true;
                        subscribe1.State = ESubscribeState.Freeze;
                        positiveSubscribeInfo.IsFreeze = true;
                        IElement element3 = htmlDocument.QuerySelector("#es .alert.alert-info");
                        string[] strArray3;
                        if (element3 == null)
                        {
                            strArray3 = (string[])null;
                        }
                        else
                        {
                            string textContent = element3.TextContent;
                            if (textContent == null)
                                strArray3 = (string[])null;
                            else
                                strArray3 = textContent.Trim().Replace(".", "").Replace(" day ", ":").Split(' ');
                        }
                        string[] strArray4 = strArray3;
                        positiveSubscribeInfo.EndFreezeTime = strArray4 == null || strArray4.Length <= 3 ? TimeSpan.FromSeconds(-1.0) : TimeSpan.Parse(((IEnumerable<string>)strArray4).Last<string>() ?? "", (IFormatProvider)this._culture);
                    }
                    else
                    {
                        Subscribe subscribe3 = subscribe1;
                        dateTimeOffset2 = DateTimeOffset.Parse(element2.Children[2].TextContent + "+03:00", (IFormatProvider)this._culture);
                        dateTimeOffset2 = dateTimeOffset2.ToUniversalTime();
                        DateTime dateTime2 = dateTimeOffset2.DateTime;
                        subscribe3.EndTime = dateTime2;
                        Subscribe subscribe4 = subscribe1;
                        DateTimeOffset endTime = (DateTimeOffset)subscribe1.EndTime;
                        dateTimeOffset2 = DateTimeOffset.Now;
                        DateTimeOffset universalTime = dateTimeOffset2.ToUniversalTime();
                        TimeSpan timeSpan = endTime - universalTime;
                        subscribe4.LeftTime = timeSpan;
                        if (element2.Children[1].TextContent.Contains("Текущая") || subscribe1.LeftTime.TotalSeconds > 0.0)
                        {
                            subscribe1.IsCurrent = true;
                            subscribe1.State = ESubscribeState.Active;
                            this._activeSubscribe = subscribe1;
                            flag = true;
                            positiveSubscribeInfo.IsActive = true;
                        }
                        else
                            subscribe1.State = ESubscribeState.End;
                    }
                    try
                    {
                        string s = htmlDocument.GetElementById("yw3")?.Children?[4]?.ChildNodes?[1]?.TextContent?.Trim();
                        if (s == null)
                            throw new NullReferenceException("Нет данных о количестве заморозок!");
                        subscribe1.FreezeCount = int.Parse(s, (IFormatProvider)this._culture);
                    }
                    catch (Exception ex)
                    {
                        subscribe1.FreezeCount = -1;
                    }
                }
                positiveSubscribeInfo.Subscribes.Add(subscribe1);
            }
            this._isSubscribed = flag;
            return positiveSubscribeInfo;
        }

        public bool FreezeSubscribe()
        {
            bool flag = false;
            if (!this.IsLogin)
                throw new PositivException(EXceptionMessageType.NotLogin);
            try
            {
                RequestParams reqParams = new RequestParams();
                reqParams["YII_CSRF_TOKEN"] = (object)this._token;
                reqParams["yt0"] = (object)"";
                reqParams["confirmation"] = (object)"yes";
                reqParams["stid"] = (object)"3";
                reqParams["akid"] = (object)"0";
                this._req.AllowAutoRedirect = false;
                HttpResponse httpResponse = this._req.Post(this._apiUrl + "/ru/subscription/confirmationfreeze", reqParams, false);
                if (httpResponse.HasRedirect)
                {
                    if (httpResponse.RedirectAddress.ToString().Contains("ru/subscription/history"))
                        flag = true;
                }
            }
            catch (HttpException ex)
            {
                if (ex.HttpStatusCode != HttpStatusCode.InternalServerError)
                    throw;
            }
            finally
            {
                this._req.AllowAutoRedirect = true;
            }
            return flag;
        }

        public bool UnFreezeSubscribe()
        {
            bool flag = false;
            if (!this.IsLogin)
                throw new PositivException(EXceptionMessageType.NotLogin);
            try
            {
                RequestParams reqParams = new RequestParams();
                reqParams["YII_CSRF_TOKEN"] = (object)this._token;
                reqParams["yt0"] = (object)"";
                reqParams["confirmation"] = (object)"yes";
                reqParams["stid"] = (object)"3";
                reqParams["akid"] = (object)"0";
                this._req.AllowAutoRedirect = false;
                HttpResponse httpResponse = this._req.Post(this._apiUrl + "/ru/subscription/confirmationunfreeze", reqParams, false);
                if (httpResponse.HasRedirect)
                {
                    if (httpResponse.RedirectAddress.ToString().Contains("ru/subscription/history"))
                        flag = true;
                }
            }
            catch (HttpException ex)
            {
                if (ex.HttpStatusCode != HttpStatusCode.InternalServerError)
                    throw;
            }
            finally
            {
                this._req.AllowAutoRedirect = true;
            }
            return flag;
        }

        private TimeSpan GetSubscribeEndTime()
        {
            TimeSpan zero = TimeSpan.Zero;
            if (!this.IsLogin)
                throw new PositivException(EXceptionMessageType.NotLogin);
            IElement element = this._parse.Parse(this._req.Get(this._apiUrl + "/ru/subscription/index", (RequestParams)null).ToString()).QuerySelector(".alert.alert-success");
            string[] strArray1;
            if (element == null)
            {
                strArray1 = (string[])null;
            }
            else
            {
                string textContent = element.TextContent;
                if (textContent == null)
                    strArray1 = (string[])null;
                else
                    strArray1 = textContent.Trim().Replace(".", "").Split(' ');
            }
            string[] strArray2 = strArray1;
            if (strArray2 == null || strArray2.Length <= 3)
                return zero;
            int length = strArray2.Length;
            return DateTimeOffset.Parse(strArray2[length - 2] + " " + strArray2[length - 1] + "+03:00", (IFormatProvider)this._culture).ToUniversalTime() - (DateTimeOffset)DateTime.Now.ToUniversalTime();
        }

        public Dictionary<EBookmakers, string> GetMirrors()
        {
            if (!this.IsLogin)
                throw new PositivException(EXceptionMessageType.NotLogin);
            IHtmlDocument htmlDocument = this._parse.Parse(this._req.Get(this._apiUrl + "/ru/options/update", (RequestParams)null).ToString());
            IHtmlCollection<IElement> htmlCollection = htmlDocument.QuerySelectorAll("input.input-large");
            Dictionary<EBookmakers, string> dictionary = new Dictionary<EBookmakers, string>();
            foreach (IElement element in (IEnumerable<IElement>)htmlCollection)
            {
                string str = element.Id.Replace("t_url", "");
                EBookmakers key = (EBookmakers)Enum.Parse(typeof(EBookmakers), str);
                IElement elementById = htmlDocument.GetElementById("[" + str + "]hidden_url");
                dictionary.Add(key, elementById.TextContent.Trim());
            }
            return dictionary;
        }

        public void UpdateStakesSetting(DublicateList data)
        {
            if (data == null)
                throw new ArgumentException("data==null");
            this._req.AddHeader("Origin", "null");
            this._req.AddHeader("Upgrade-Insecure-Requests", "1");
            this._req.AddHeader("Cache-Control", "max-age=0");
            this._req.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            this._req.AddHeader("Accept-Encoding", "gzip, deflate");
            RequestParams reqParams = new RequestParams()
            {
                ["Settings[name]"] = (object)"%D0%9F%D0%BE%20%D1%83%D0%BC%D0%BE%D0%BB%D1%87%D0%B0%D0%BD%D0%B8%D1%8E"
            };
            reqParams["YII_CSRF_TOKEN"] = (object)this._token;
            foreach (KeyValuePair<string, string> keyValuePair in (List<KeyValuePair<string, string>>)data)
                reqParams[keyValuePair.Key] = (object)keyValuePair.Value;
            reqParams["yt43"] = (object)"";
            try
            {
                this._req.AllowAutoRedirect = false;
                HttpResponse httpResponse = this._req.Post(new Uri(this._apiUrl + "/ru/settings/update"), reqParams, true);
                if (!httpResponse.HasRedirect)
                    throw new ArgumentException("Ошибка сохранения настроек фильтров на PositiveBet. Отсутствует переадресация");
                int result;
                if (!int.TryParse(((IEnumerable<string>)httpResponse.Location.Split('/')).Last<string>(), NumberStyles.Any, (IFormatProvider)this._culture, out result))
                    return;
                this._filterId = new int?(result);
            }
            catch (NetException ex)
            {
                throw new ArgumentException("Ошибка сохранения настроек фильтров на PositiveBet. " + ex.Message, (Exception)ex);
            }
            finally
            {
                this._req.AllowAutoRedirect = true;
            }
        }

        public void UpdateBookmakersSetting(DublicateList data)
        {
            if (data == null)
                throw new ArgumentException("data==null");
            this._req.AddHeader("Origin", "null");
            this._req.AddHeader("Upgrade-Insecure-Requests", "1");
            this._req.AddHeader("Cache-Control", "max-age=0");
            this._req.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            this._req.AddHeader("Accept-Encoding", "gzip, deflate");
            RequestParams reqParams = new RequestParams()
            {
                ["YII_CSRF_TOKEN"] = (object)this._token
            };
            foreach (KeyValuePair<string, string> keyValuePair in (List<KeyValuePair<string, string>>)data)
                reqParams[keyValuePair.Key] = (object)keyValuePair.Value;
            reqParams["yt0"] = (object)"";
            try
            {
                this._req.AllowAutoRedirect = false;
                HttpResponse httpResponse = this._req.Post(new Uri(this._apiUrl + "/ru/options/update"), reqParams, true);
                if (!httpResponse.HasRedirect)
                    throw new ArgumentException("Ошибка сохранения настроек 'Букмекеры' на PositiveBet. Отсутствует переадресация");
                if (!httpResponse.Location.ToLower().Contains("/options/update"))
                    throw new ArgumentException("Ошибка сохранения настроек 'Букмекеры' на PositiveBet");
            }
            catch (NetException ex)
            {
                throw new ArgumentException("Ошибка сохранения настроек 'Букмекеры' на PositiveBet. " + ex.Message, (Exception)ex);
            }
            finally
            {
                this._req.AllowAutoRedirect = true;
            }
        }

        public Dictionary<string, string> GetCookies()
        {
            return this._req.Cookies.ToDictionary<KeyValuePair<string, string>, string, string>((Func<KeyValuePair<string, string>, string>)(x => x.Key), (Func<KeyValuePair<string, string>, string>)(y => y.Value));
        }

        private void CheckAndCreateRequest()
        {
            if (this._req != null)
                return;
            this._req = new HttpRequest();
            this._req.CharacterSet = Encoding.UTF8;
            this._req.Cookies = new CookieDictionary(false);
            this._req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36";
        }

        public void HideFork(Fork fork, EForkHideRule hideRule)
        {
            string str1 = "/ru/bets/rowshide?";
            string str2;
            switch (hideRule)
            {
                case EForkHideRule.ThisForks:
                    str2 = str1 + string.Format("row_id={0}", (object)fork.Id);
                    break;
                case EForkHideRule.AllBkAllFork:
                    str2 = str1 + "event_l_id=" + fork.Elid;
                    break;
                case EForkHideRule.OnlyOneBkAllForks:
                    str2 = str1 + "event_id1=" + fork.OneBet.EvId;
                    break;
                case EForkHideRule.OnlyTwoBkAllForks:
                    str2 = str1 + "event_id1=" + fork.TwoBet.EvId;
                    break;
                case EForkHideRule.TwoBkAllForks:
                    str2 = str1 + "event_id1=" + fork.OneBet.EvId + "&event_id2=" + fork.TwoBet.EvId;
                    break;
                case EForkHideRule.OnlyOneBkOnlyThisStake:
                    str2 = str1 + "bet_id1=" + fork.OneBet.OtherData;
                    break;
                case EForkHideRule.OnlyTwoOnlyThisStake:
                    str2 = str1 + "bet_id1=" + fork.TwoBet.OtherData;
                    break;
                case EForkHideRule.TwoBkOnlyThisStake:
                    str2 = str1 + "bet_id1=" + fork.OneBet.OtherData + "bet_id2=" + fork.TwoBet.OtherData;
                    break;
                case EForkHideRule.ShowAllHidenForks:
                    str2 = "/ru/bets/rowsshow";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(hideRule), (object)hideRule, (string)null);
            }
            string address = this._apiUrl + str2 + "&isAjax=true";
            if (hideRule == EForkHideRule.ShowAllHidenForks)
                address = this._apiUrl + str2;
            this._req.Get(address, (RequestParams)null);
        }

        public List<AnotherBet> GetAnotherBets1(Fork fork)
        {
            if (!this.IsLogin)
                throw new PositivException(EXceptionMessageType.NotLogin);
            if (fork == null)
                return new List<AnotherBet>();
            GetCoefResponse getCoefResponse = JsonConvert.DeserializeObject<GetCoefResponse>(this._req.Get(this._apiUrl + "/ru/bets/refreshOdd?bid1=" + fork.OneBet.OtherData + "&eid1=" + fork.OneBet.EvId + "&elid=" + fork.Elid + "&crid=" + fork.CridId + "&k1=" + fork.K1 + "&k2=" + fork.K2 + "&gac=1&isAjax=true", (RequestParams)null).ToString());
            if (getCoefResponse.Coefs1 == null)
                getCoefResponse.Coefs1 = new List<AnotherBet>();
            foreach (AnotherBet anotherBet in getCoefResponse.Coefs1)
                anotherBet.AnotherBetNumber = EAnotherBetNumber.One;
            return getCoefResponse.Coefs1;
        }

        public List<AnotherBet> GetAnotherBets2(Fork fork)
        {
            if (!this.IsLogin)
                throw new PositivException(EXceptionMessageType.NotLogin);
            if (fork == null)
                return new List<AnotherBet>();
            GetCoefResponse getCoefResponse = JsonConvert.DeserializeObject<GetCoefResponse>(this._req.Get(this._apiUrl + "/ru/bets/refreshOdd?bid2=" + fork.TwoBet.OtherData + "&&eid2=" + fork.TwoBet.EvId + "&elid=" + fork.Elid + "&crid=" + fork.CridId + "&k1=" + fork.K1 + "&k2=" + fork.K2 + "&gac=1&isAjax=true", (RequestParams)null).ToString());
            if (getCoefResponse.Coefs2 == null)
                getCoefResponse.Coefs2 = new List<AnotherBet>();
            foreach (AnotherBet anotherBet in getCoefResponse.Coefs2)
                anotherBet.AnotherBetNumber = EAnotherBetNumber.Two;
            return getCoefResponse.Coefs2;
        }

        public bool ChangePassword(string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("newPassword не может быть пустым");
            if (!this.IsLogin)
                throw new PositivException(EXceptionMessageType.NotLogin);
            this._req.AllowAutoRedirect = false;
            RequestParams reqParams = new RequestParams()
            {
                ["YII_CSRF_TOKEN"] = (object)this._token,
                ["UserChangePassword[oldPassword]"] = (object)this._password,
                ["UserChangePassword[password]"] = (object)newPassword,
                ["UserChangePassword[verifyPassword]"] = (object)newPassword,
                ["yt0"] = (object)"Сохранить"
            };
            try
            {
                this._req.Post(this._apiUrl + "/user/profile/changepassword", reqParams, true).ToString();
                return this._req.Response.HasRedirect && !(this._req.Response.RedirectAddress == (Uri)null) && this._req.Response.RedirectAddress.ToString().Contains("user/profile/profile");
            }
            finally
            {
                this._req.AllowAutoRedirect = true;
            }
        }

        public void Dispose()
        {
            this._req.Dispose();
        }
    }
}
