using ForksWebAPI.Common.Client;
using ForksWebAPI.DATA.Request;
using ForksWebAPI.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ForksWebAPI.BackgroundTask
{
    public interface IParserPositiveService
    {

        bool _isRun { get; set; }
        List<Fork> ReceiveForksAndParse(CancellationToken cancellationToken);
        void Login();
    }
    public class ParserPositiveService : IParserPositiveService
    {

        public CookieContainer cookieContainer;

        private HttpClient client;

        public string _apiUrl = "https://positivebet.com/";

        public string _SessId;

        public bool _isRun { get; set; } = false;

        

        public ParserPositiveService()
        {
            cookieContainer = new CookieContainer();

            //cookieContainer.Add(new Uri(_apiUrl), new Cookie("_ga", "GA1.2.1391166531.1627804774"));
            //cookieContainer.Add(new Uri(_apiUrl), new Cookie("_gid", "GA1.2.1644841140.1627804774"));
            //cookieContainer.Add(new Uri(_apiUrl), new Cookie("_ym_uid", "1627804774165631029"));
            //cookieContainer.Add(new Uri(_apiUrl), new Cookie("_ym_d", "1627804774"));
            cookieContainer.Add(new Uri(_apiUrl), new Cookie("_ym_isad", "1"));
            //cookieContainer.Add(new Uri(_apiUrl), new Cookie("ddlPerPage_value", "30"));
            cookieContainer.Add(new Uri(_apiUrl), new Cookie("language", "ae0cc45c9e58fab6acdc21903ec78c3659e4a505s%3A2%3A%22ru%22%3B"));
            cookieContainer.Add(new Uri(_apiUrl), new Cookie("_gat", "1"));
            //cookieContainer.Add(new Uri(_apiUrl), new Cookie("PHPSESSID", "ronv741p4j8h3vdjj6umv09ik3"));
            cookieContainer.Add(new Uri(_apiUrl), new Cookie("YII_CSRF_TOKEN", "a14424d45eaa302247ef2458c1a49bdab9d262f7s%3A88%3A%22TH4zRHkwSUUwMDhWQX5aN2JONzkxWldDQW54Tkdjckzu_7PXNqICXfHiVjT9VnmhMAnGL36nnrXQu4FysGrdhQ%3D%3D%22%3B"));
            HttpClientHandler handler = new HttpClientHandler() { CookieContainer = cookieContainer,UseCookies = true, AllowAutoRedirect = true,};


            client = new HttpClient(handler);
            client.BaseAddress = new Uri(_apiUrl);
            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        }
        public void Login()
        {
            List<KeyValuePair<string, string>> formVariables = new List<KeyValuePair<string, string>>();
            formVariables.Add(new KeyValuePair<string, string>("YII_CSRF_TOKEN", "TH4zRHkwSUUwMDhWQX5aN2JONzkxWldDQW54Tkdjckzu_7PXNqICXfHiVjT9VnmhMAnGL36nnrXQu4FysGrdhQ=="));
            formVariables.Add(new KeyValuePair<string, string>("UserLogin[username]", "profsams@bk.ru"));
            formVariables.Add(new KeyValuePair<string, string>("UserLogin[password]", "medhoG-zodkyp-zecby2"));
            formVariables.Add(new KeyValuePair<string, string>("UserLogin[rememberMe]", "0"));
            formVariables.Add(new KeyValuePair<string, string>("yt0", ""));

            FormUrlEncodedContent formContent = new FormUrlEncodedContent(formVariables);
            
            try
            {
                var beforeCookie = cookieContainer.GetCookies(new Uri(_apiUrl));
                var response = client.PostAsync("ru/user/login", formContent).Result;


                var cooka = cookieContainer.GetCookies(new Uri(_apiUrl));
                
                HttpResponseMessage response2 = client.GetAsync("ru/bets/index").Result;
                string page = response2.Content.ReadAsStringAsync().Result;
                File.WriteAllText("нью.txt", page);
                
                foreach(Cookie item in cooka)
                {
                    if(item.Name == "PHPSESSID")
                    {
                        _SessId = item.Value;
                    }
                }
                

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    this._isRun = true;
                }
            }
            catch (Exception)
            {
                this._isRun = false;
            }
        }
        public List<Fork> ReceiveForksAndParse(CancellationToken cancellationToken)
        {
            List<KeyValuePair<string, string>> formVariables = new List<KeyValuePair<string, string>>();
            formVariables.Add(new KeyValuePair<string, string>("YII_CSRF_TOKEN", "TH4zRHkwSUUwMDhWQX5aN2JONzkxWldDQW54Tkdjckzu_7PXNqICXfHiVjT9VnmhMAnGL36nnrXQu4FysGrdhQ=="));
            formVariables.Add(new KeyValuePair<string, string>("UserLogin[username]", "profsams@bk.ru"));
            formVariables.Add(new KeyValuePair<string, string>("UserLogin[password]", "medhoG-zodkyp-zecby2"));
            formVariables.Add(new KeyValuePair<string, string>("UserLogin[rememberMe]", "0"));
            formVariables.Add(new KeyValuePair<string, string>("yt0", ""));

            FormUrlEncodedContent formContent = new FormUrlEncodedContent(formVariables);

            var response = client.PostAsync("ru/user/login", formContent).Result;

            HttpResponseMessage response2 = client.GetAsync("ru/bets/index?ajax=gridBets&au=0&crid=&fl%5B%5D=668559&isAjax=true&markNewEvent=false&perPage=30").Result;
            string page = response2.Content.ReadAsStringAsync().Result;
            File.WriteAllText("ещёстраница.txt", page);
            //Url без подписки
            //string requestPositive = "https://positivebet.com/ru/bets/index?ajax=gridBets&au=0&crid=&fl%5B%5D=741851&isAjax=true&markNewEvent=true&perPage=30";
            /*
            CookieContainer cont = new CookieContainer();
            var allCookies = cookiesSecond;
            cont.Add(cookiesSecond);
            HttpClientHandler handler = new HttpClientHandler() { CookieContainer = cont, UseCookies = true };
            HttpClient cl = new HttpClient(handler);
            cl.BaseAddress = new Uri("https://positivebet.com/");
            cl.DefaultRequestHeaders.UserAgent.Clear();
            cl.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36");
            cl.DefaultRequestHeaders.Accept.Clear();
            cl.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            CookieCollection col = cookieContainer.GetCookies(new Uri("https://positivebet.com/"));
            */
            cookieContainer.Add(new Uri(_apiUrl), new Cookie("PHPSESSID", _SessId));
            cookieContainer.GetCookies(new Uri(_apiUrl));

            HttpResponseMessage response3 = client.GetAsync("ru/bets/index").Result;
            string pag3 = response2.Content.ReadAsStringAsync().Result;
            //string page = cl.GetStringAsync("ru/bets/index").Result;
            File.WriteAllText("сейчас.txt", pag3);

            HtmlDocument doc = new HtmlDocument();

            doc.LoadHtml(page);

            var headers = doc.DocumentNode.SelectNodes("//*[@id='gridBets']/table/thead/tr/th");

            var stringsInTable = doc.DocumentNode.SelectNodes("//*[@id='gridBets']/table/tbody/tr");

            DataTable table = new DataTable();

            int countNbsp = 0;

            foreach (HtmlNode header in headers)
            {

                try
                {
                    table.Columns.Add(header.InnerText == "&nbsp;" && countNbsp == 0 ? "Time" : header.InnerText == "&nbsp;" ? "noNameCell" + countNbsp : header.InnerText);

                }
                catch (Exception ex)
                {
                    //Ячейки таблицы могут повторяться,нельзя добавлять повторяющиеся
                }
                if (header.InnerText == "&nbsp;")
                {
                    countNbsp++;
                }
            }
            table.Columns.Add("Direction1");
            table.Columns.Add("Direction2");

            table.Columns.Add("Sport");

            table.Columns.Add("Teams1");
            table.Columns.Add("Teams2");

            table.Columns.Add("League1");
            table.Columns.Add("League2");

            table.Columns.Add("Score1");
            table.Columns.Add("Score2");

            table.Columns.Add("ProcessOfMatchFirst");
            table.Columns.Add("ProcessOfMatchSecond");

            table.Columns.Add("BookmakerNameFirst");
            table.Columns.Add("BookmakerNameSecond");

            table.Columns.Add("ForksCountFirstBet");
            table.Columns.Add("ForksCountSecondBet");
            //table.Columns.Add("ForkId");
            int countTd = 1;
            foreach (var row in stringsInTable)
            {
                try
                {
                    var tdForDirection = row.SelectNodes("td")
                        .Where((Func<HtmlNode, bool>)(delegate (HtmlNode op) {
                            if (countTd == 6) { countTd++;  return true; }else { countTd++; return false; }
                    })).FirstOrDefault();
                    countTd = 1;
                    HtmlNodeCollection nobr = tdForDirection.SelectNodes("nobr");

                    var ForksCountFirstBk = nobr[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].InnerText;
                    var ForksCountSecondBk = nobr[1].ChildNodes[2].ChildNodes[0].ChildNodes[0].InnerText;
                    //arrowfreeze.png
                    Regex regex = new Regex(@"arrow\w*");

                    var Dir1Dirty = nobr[0].SelectNodes("a")[0].InnerHtml;
                    var Dir2Dirty = nobr[1].SelectNodes("a")[0].InnerHtml;

                    var dir1Res = regex.Matches(Dir1Dirty)[0];
                    var dir2Res = regex.Matches(Dir2Dirty)[0];

                    var tdForSport = row.SelectNodes("td")
                        .Where((Func<HtmlNode, bool>)(delegate (HtmlNode op) {
                            if (countTd == 1) { countTd++; return true; } else { countTd++; return false; }
                        })).FirstOrDefault();
                    countTd = 1;
                    var sport = tdForSport.SelectNodes("a")[2].SelectNodes("img")[0].Attributes["alt"].Value;

                    var tdForEvent = row.SelectNodes("td")
                        .Where((Func<HtmlNode, bool>)(delegate (HtmlNode op) {
                            if (countTd == 4) { countTd++; return true; } else { countTd++; return false; }
                        })).FirstOrDefault();
                    countTd = 1;
                    string firstTeam1 = null;
                    string secondTeam1 = null;
                    string firstTeam2 = null;
                    string secondTeam2 = null;
                    string nameOfLeagueFirst = null;
                    string scoreFirst = null;
                    string processOfMatchfirst = null;
                    string nameOfLeagueSecond = null;
                    string scoreSecond = null;
                    string processOfMatchSecond = null;

                    if (!tdForEvent.InnerText.Contains("Событие скрыто"))
                    {
                        //Название команд либо игроков для первой бк
                        string[] groupTeam1 = tdForEvent.ChildNodes[1].InnerText.Split(" vs ");
                        firstTeam1 = groupTeam1[0];
                        secondTeam1 = groupTeam1[1];

                        //Название команд либо игроков для второй бк
                        string[] groupTeam2 = tdForEvent.ChildNodes[5].InnerText.Split(" vs ");
                        firstTeam2 = groupTeam2[0];
                        secondTeam2 = groupTeam2[1];

                        HtmlNodeCollection GroupLeague = tdForEvent.SelectNodes("small");
                        //Две группы для бк(команды,счёт,статистика по таймам,сетам)
                        var GroupLeagueFirst = GroupLeague[0].ChildNodes;
                        var GroupLeagueSecond = GroupLeague[1].ChildNodes;


                         nameOfLeagueFirst = GroupLeagueFirst[0].InnerText;
                         scoreFirst = GroupLeagueFirst[1].InnerText;
                        //не использовал 
                        processOfMatchfirst = GroupLeagueFirst[2].InnerText.Trim();

                         nameOfLeagueSecond = GroupLeagueFirst[0].InnerText;
                         scoreSecond = GroupLeagueFirst[1].InnerText;
                        //не использовал
                         processOfMatchSecond = GroupLeagueFirst[2].InnerText.Trim();
                    }
                    
                    var tdForBookmakers = row.SelectNodes("td")
                        .Where((Func<HtmlNode, bool>)(delegate (HtmlNode op) {
                            if (countTd == 3) { countTd++; return true; } else { countTd++; return false; }
                        })).FirstOrDefault();

                    var firstBk = tdForBookmakers.ChildNodes[0].InnerText;
                    var secondBk = tdForBookmakers.ChildNodes[2].InnerText;
                    countTd = 1;

                    var result = row.SelectNodes("td")
                        .Select<HtmlNode, object>((Func<HtmlNode, object>)((td) => ConvertFromString(td.InnerText)))
                        .Append(dir1Res)
                        .Append(dir2Res)
                        .Append(sport)
                        .Append(firstTeam1 + secondTeam1)
                        .Append(firstTeam2 + secondTeam2)
                        .Append(nameOfLeagueFirst)
                        .Append(nameOfLeagueSecond)
                        .Append(scoreFirst)
                        .Append(scoreSecond)
                        .Append(processOfMatchfirst)
                        .Append(processOfMatchSecond)
                        .Append(firstBk)
                        .Append(secondBk)
                        .Append(ForksCountFirstBk)
                        .Append(ForksCountSecondBk)
                        .ToArray();
                    table.Rows.Add(result);
                }
                catch (Exception ex)
                {
                    //Ячейки таблицы могут повторяться,нельзя добавлять повторяющиеся
                }
                countTd = 1;
            }
            
            var JSONresult = JsonConvert.SerializeObject(table);

            List<MyFork> myForkes = JsonConvert.DeserializeObject<List<MyFork>>(JSONresult);
            List<Fork> normalForks = new List<Fork>();
            int ForkId = 1;
            foreach(var forkes in myForkes)
            {
                Fork fork = new Fork();

                fork.BetType = ConvertToBetType(forkes.Bet1,forkes.ProcessOfMatchFirst,forkes.ProcessOfMatchSecond);
                //fork.CridId = Convert.ToString(ForkId);
                //fork.Elid = Convert.ToString(ForkId);
                fork.ForkId = ForkId;
                fork.K1 = forkes.Coef1;
                fork.K2 = forkes.Coef2;
                fork.OneBet = ConvertToNormalBet(forkes,EBet.FirstBet,fork.BetType);
                fork.TwoBet = ConvertToNormalBet(forkes,EBet.SecondBet,fork.BetType);
                fork.Profit = Decimal.Parse(forkes.Percent.Replace(".",","));
                fork.Sport = ConvertToSport(forkes.Sport);
                fork.Time = TimeSpan.Parse(forkes.Time);
                fork.UpdateCount = 0;
                normalForks.Add(fork);
                ForkId++;
            }
            return  normalForks;
        }
        /// <summary>
        /// Goals WinPoints НЕ БЫЛО НА ПОЗИТИВЕ НАДО ПОСМОТРЕТЬ КАК ВЫГЛЯДИТ И ДОПИСАТЬ
        /// </summary>
        /// <param name="value"></param>
        /// <param name="processOfMatch"></param>
        /// <param name="processOfMatchSecond"></param>
        /// <returns></returns>
        public EBetType ConvertToBetType(string value,string processOfMatch,string processOfMatchSecond)
        {
            value = value.ToLower();

            StringBuilder scoreBuilder = new StringBuilder();

            string[] groupProcessOfMatch = processOfMatch.Split(",");
            //Последний счёт в матче
            string lastScoreInMatch = groupProcessOfMatch[0].Trim().Replace(")","");
            int currentGame;
            if (!string.IsNullOrWhiteSpace(lastScoreInMatch))
            {
                currentGame = int.Parse(lastScoreInMatch.Split(":")[0].Replace("(", "").Replace("(", "").Trim()) + int.Parse(lastScoreInMatch.Split(":")[1].Replace("(", "").Replace("(", "").Trim());
            }else
            {
                currentGame = 0;
            }
            //Гейм который идёт сейчас

            Regex winInSet = new Regex(@"п\d*\(\w* сет\)");
            Regex winInGame = new Regex(@"п\d*\(\d* гейм\)\(\d* сет\)");

            //Победы в сете

            if (winInSet.IsMatch(value))
            {
                return EBetType.WinInSet;
            }
            //Победы в гейме
            if (winInGame.IsMatch(value))
            {
                //нужно проверять текущий гейм который идёт и брать гейм из ставки
                //если текущий гейм совпадает со ставкой = WinInCurrentGame
                Regex game = new Regex(@"\(\d* гейм\)");

                string scoreInStake = game.Matches(value)[0].Value;
                Regex gameInStakeReg = new Regex(@"\(\d*");
                //гейм в ставке
                int gameInStake = int.Parse(gameInStakeReg.Matches(scoreInStake)[0].Value);
                //Совпадение по гейму в ставке и в очках = победа в текущем гейме
                //Иначе победа в гейме
                if(currentGame == gameInStake)
                {
                    return EBetType.WinInCurrentGame;
                }
                //Победы в гейме
                //П1(4 гейм)(2 сет)
                //П2(4 гейм)(2 сет)
                return EBetType.WinInGame;
            }

            //Форы
            if (value.Contains("ф1") || value.Contains("ф2"))
            {
                return EBetType.Fora;
            }

            //Тоталы + Сorner
            Regex individualTotal = new Regex(@"команда\d\sт[мб]");
            if (value.Contains("тм") || value.Contains("тб"))
            {
                //Индивидуальные тоталы
                if (individualTotal.IsMatch(value))
                {
                    return EBetType.IndTotal;
                }
                if (value.Contains("угл"))
                {
                    return EBetType.Corner;
                }
                return EBetType.Total;
            }
            //ПОБЕДЫ
            bool conditionWin = value.Length < 3 && value.Contains("п1") || value.Length < 3 && value.Contains("п2") || value.Length < 3 && value.Contains("x") || value.Length < 3 && value.Contains("1x") || value.Length < 3 && value.Contains("2") || value.Contains("команда") && value.Contains("гонка");
            if (conditionWin)
            {
                return EBetType.Win;
            }
            return EBetType.None;
        }
        public ESport ConvertToSport(string value)
        {
            value = value.ToLower();
            if (value.Contains("бадминтон")){
                return ESport.Badminton;
            }
            else if (value.Contains("баскетбол"))
            {
                return ESport.Basketball;
            }
            else if (value.Contains("бейсбол"))
            {
                return ESport.Baseball;
            }else if (value.Contains("волейбол"))
            {
                return ESport.VoleyBall;
            }
            else if (value.Contains("гандбол"))
            {
                return ESport.Handball;
            }else if(value.Contains("настольный теннис"))
            {
                return ESport.TableTennis;
            }
            else if (value.Contains("теннис"))
            {
                return ESport.Tennis;
            }
            else if (value.Contains("футбол"))
            {
                return ESport.Football;
            }
            else if (value.Contains("футзал"))
            {
                return ESport.Futsal;
            }else if (value.Contains("хоккей"))
            {
                return ESport.Hockey;
            }else
            {
                return ESport.None;
            }
        }
        public Bet ConvertToNormalBet(MyFork bet, EBet betType,EBetType betTypeFromFork)
        {
            switch (betType)
            {
                case EBet.FirstBet:

                    Bet bet1 = new Bet()
                    {
                        Coef = Convert.ToDecimal(bet.Coef1.Replace(".", ",")),
                        //Нужно разобраться Какой Id к какому букмекеру относится и написать
                        //ConvertBookmakerId
                        Bookmaker = ConvertBookmakerName(bet.BookmakerNameFirst),
                        EvId = Convert.ToString(bet.Id),
                        //Не знаю для чего это поле нужно
                        OtherData = "none",
                        BetValue = bet.Bet1,
                        Direction = ConvertToDirection(bet.Direction1),
                        //Нужно как-то обновлять это свойство(по идее это вообще переводится как количество 
                        //Вилок,но означает наверное другое(так как в Fork JsonProperty написано bet_id
                        ForksCount = int.Parse(bet.ForksCountFirstBet),
                        BetType = betTypeFromFork,
                        MatchData = " (" + GetLeague(bet.League1) + ") " + GetScore(bet.Score1)
                    };
                    //Нужно этот параметр то же переделывать как-то
                    bet1.Parametr = 0;
                    bet1.Team = ConvertToTeam(bet.Event,ETeam.FirstTeam);
                    bet1.Url = "https://positivebet.com" + String.Format("/ru/bets/go/e/{0}/b/{1}/elid/{2}/k/{3}", bet.Event_Id1, bet.Booker_Id1, bet.Event_l_Id, bet.K1);
                    return bet1;

                case EBet.SecondBet:

                    Bet bet2 = new Bet()
                    {
                        Coef = Convert.ToDecimal(bet.Coef2.Replace(".", ",")),
                        //Нужно разобраться Какой Id к какому букмекеру относится и написать
                        //ConvertBookmakerId
                        Bookmaker = ConvertBookmakerName(bet.BookmakerNameSecond),
                        EvId = Convert.ToString(bet.Id),
                        //Не знаю для чего это поле нужно
                        OtherData = "none",
                        BetValue = bet.Bet2,
                        Direction = ConvertToDirection(bet.Direction2),
                        //Нужно как-то обновлять это свойство(по идее это вообще переводится как количество 
                        //Вилок,но означает наверное другое(так как в Fork JsonProperty написано bet_id
                        ForksCount = int.Parse(bet.ForksCountSecondBet),
                        BetType = betTypeFromFork,
                        MatchData = " (" + GetLeague(bet.League1) + ") " + GetScore(bet.Score2)
                    };
                    //Нужно этот параметр то же переделывать как-то
                    bet2.Parametr = 0;
                    bet2.Team = ConvertToTeam(bet.Event, ETeam.SecondTeam);
                    bet2.Url = "https://positivebet.com" + String.Format("/ru/bets/go/e/{0}/b/{1}/elid/{2}/k/{3}", bet.Event_Id2,bet.Booker_Id2,bet.Event_l_Id,bet.K2);
                    return bet2;

                default:
                    return new Bet();
            }
        }

        public string GetScore(string score)
        {
            if (!string.IsNullOrWhiteSpace(score))
            {
                return score.Trim().Replace(":", " | ");
            }
            return String.Empty;
        }
        public string ConvertToTeam(string value,ETeam teamNumber)
        {
            string[] localValue = value.Split(" vs ",StringSplitOptions.RemoveEmptyEntries);
            if(localValue.Length > 1)
            {
                switch (teamNumber)
                {
                    case ETeam.FirstTeam:
                        return localValue[0];
                    case ETeam.SecondTeam:
                        return localValue[1];
                    default:
                        throw new Exception($"Некорректный {teamNumber}");
                }
            }else
            {
                return "";
            }
            
        }
        public EDirection ConvertToDirection(string direction)
        {
            if (direction.Contains("up"))
            {
                return EDirection.Up;
            }else if (direction.Contains("down"))
            {
                return EDirection.Down;
            }else if(direction.Contains("freeze"))
            {
                return EDirection.Freeze;
            }else
            {
                throw new Exception($"Не верный {direction}");
            }
        }
        public EBookmakers ConvertBookmakerName(string bookName)
        {
            switch (bookName.ToLower())
            {
                case "10bet":
                    return EBookmakers._10bet;
                case "188bet":
                    return EBookmakers._188bet;
                // case "18bet":
                //  return EBookmakers._18bet;
                case "1win":
                    return EBookmakers._1win;
                case "1xbet":
                    return EBookmakers._1xbet;
                //case "888.ru":
                //  return EBookmakers._888sport;
                case "888sport":
                    return EBookmakers._888sport;
                case "adjarabet":
                    return EBookmakers._adjarabet;
                case "baltbet":
                    return EBookmakers._baltbet;
                case "bet-at-home":
                    return EBookmakers._bet_at_home;
                case "bet365 mobile":
                    return EBookmakers._bet365Mobile;
                case "betcity":
                    return EBookmakers._betcity;
                case "betfair":
                    return EBookmakers._betfair;
                case "betfair sport":
                    return EBookmakers._betfair_sport;
                case "sportingbet":
                    return EBookmakers._sportingbet;
                case "dafabet mobile":
                    return EBookmakers._dafabetMobile;
                case "favbet":
                    return EBookmakers._favbet;
                //  case "GGBet":
                // return EBookmakers._ggbet;
                case "fonbet":
                    return EBookmakers._fonbet;
                case "ladbrokes":
                    return EBookmakers._ladbrokes;
                case "leon":
                    return EBookmakers._leon;
                case "liga stavok":
                    return EBookmakers._liga_stavok;
                // case "Loot.bet":
                //   return EBookmakers._loot.bet;
                case "marathon":
                    return EBookmakers._marathon;
                case "mostbet":
                    return EBookmakers._mostbet;
                case "olimp":
                    return EBookmakers._olimp;
                case "paddyLower":
                    return EBookmakers._paddypower;
                case "parimatch classic":
                    return EBookmakers._parimatchClassic;
                //case "Pin-up":
                //    return EBookmakers._pinapp;
                case "pinacle":
                    return EBookmakers._pinnacle;
                //case "Pokerstars":
                // return EBookmakers._;
                case "sbobet":
                    return EBookmakers._sbobet;
                case "tennisi":
                    return EBookmakers._tennisi;
                case "unibet":
                    return EBookmakers._unibet;
                case "vbet":
                    return EBookmakers._vbet;
                case "zenit":
                    return EBookmakers._zenit;
                case "william hill":
                    return EBookmakers._willhill;
                case "winlinebet":
                    return EBookmakers._winlinebet;
                case "377bet":
                    return EBookmakers._377bet;
                case "etoto":
                    return EBookmakers._etoto;
                case "comeon":
                    return EBookmakers._comeOn;
                case "netbet":
                    return EBookmakers._netBet;
                case "jenningsBet":
                    return EBookmakers._jenningsBet;
                case "mobilbet":
                    return EBookmakers._mobilbet;
                case "roadbet":
                    return EBookmakers._roadBet;
                case "1xstavka.ru":
                    return EBookmakers._1xstavkaRu;
                case "melbet":
                    return EBookmakers._melbet;
                case "1xbit":
                    return EBookmakers._1xbit;
                case "balbet.ru":
                    return EBookmakers._baltbetRu;

                case "betcity.ru":
                    return EBookmakers._betcityRu;
                case "bwinalt":
                    return EBookmakers._bwin;
                case "bwin.ru":
                    return EBookmakers._bwinRu;
                case "gamebookers":
                    return EBookmakers._gamebookers;
                case "partypoker":
                    return EBookmakers._partypoker;
                case "visabet":
                    return EBookmakers._vistabet;
                case "giocoDigitale":
                    return EBookmakers._giocoDigitale;
                case "dafabet":
                    return EBookmakers._dafabet;
                case "dafabetmobile":
                    return EBookmakers._dafabetMobile;
                case "12bet":
                    return EBookmakers._12bet;
                case "favtoto":
                    return EBookmakers._favtoto;
                case "fonbet.ru":
                    return EBookmakers._fonbetRu;
                case "betstat":
                    return EBookmakers._betStarts;
                case "ladbrokesau":
                    return EBookmakers._ladbrokesAu;
                case "leon.ru":
                    return EBookmakers._leonRu;
                case "leon.ru mobile":
                    return EBookmakers._leonRuMobile;
                case "oddsRing":
                    return EBookmakers._oddsRing;
                case "ligastavok.ru":
                    return EBookmakers._ligastavokRu;
                case "marathon.ru":
                    return EBookmakers._marathonRu;
                case "mostbet.ru":
                    return EBookmakers._mostbetRu;
                case "olimp.bet":
                    return EBookmakers._olimpBet;
                case "paddypower":
                    return EBookmakers._paddypower;
                case "parimach.ru":
                    return EBookmakers._parimatchRu;
                case "williamhillmobile":
                    return EBookmakers._williamHillMobile;
                case "zenit.win":
                    return EBookmakers._zenitWin;
                default:
                    return EBookmakers._None;
            }
        }
        //парсит лигу
        public string GetLeague(string league)
        {
            if (!string.IsNullOrWhiteSpace(league))
            {
                return league.Replace("(","").Replace(")","");
            }
            
            return String.Empty;
        }
        public object ConvertFromString(string text)
        {
            if(text.Contains("сек") || text.Contains("мин"))
            {
                text = text.Replace("сек", "").Trim();
                text = text.Replace("мин", "").Trim();
                return TimeSpan.FromSeconds(Int32.Parse(text));
            }
            return text;
        }
        public object ConvertBetType(Fork fork)
        {
            //Конвертер по типу ставку
            return fork;
        }
    }
}