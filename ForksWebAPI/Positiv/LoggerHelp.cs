using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using xNet;

namespace LiveForks.Admin.Providers.Positiv
{
    internal class LoggerHelp
    {
        private string _serverLogs = "http://193.124.66.224:6060";
        private bool _isRealSend;
        private string _login;
        private WebClient _client;

        public LoggerHelp(bool isRealSend, string login)
        {
            this._isRealSend = isRealSend;
            this._login = login;
            this._client = new WebClient();
            this._client.Encoding = Encoding.UTF8;
        }

        public void SendError7(string response, string queryData, int betDataRequestCount)
        {
            if (!this._isRealSend)
                return;
            this.SendData(new Uri(this._serverLogs + "/error7"), "Ошибка 'Нет подписки' Update", this.GetDefaultMessage(response, queryData) + string.Format("<b>Good request before now:</b>{0}<br>", (object)betDataRequestCount));
        }

        public void SendEmptyUrl(string response, string queryData)
        {
            if (!this._isRealSend)
                return;
            this.SendData(new Uri(this._serverLogs + "/badJson"), "Ошибка 'Нет URL'", this.GetDefaultMessage(response, queryData));
        }

        public void SendBadJsonResponse(string response, string queryData)
        {
            if (!this._isRealSend)
                return;
            this.SendData(new Uri(this._serverLogs + "/badJson"), "Ошибка 'Невалидный ответ'", this.GetDefaultMessage(response, queryData));
        }

        public void SendErrorUnknown(string response, string queryData)
        {
            if (!this._isRealSend)
                return;
            this.SendData(new Uri(this._serverLogs + "/badJson"), "Ошибка 'error: X'", this.GetDefaultMessage(response, queryData));
        }

        private string GetDefaultMessage(string response, string queryData)
        {
            string str = this.CreateMessage() + "<b>Request data:</b> " + queryData + "<br>";
            return response != null ? (response.Length <= 1000 ? str + "<b>Server Response:</b> " + response + "<br>" : str + "<b>Server Response:</b> " + response.Substring(0, 1000) + "<br>") : str + "<b>Server Response:</b> null";
        }

        private string CreateMessage()
        {
            return this.AddTimeNow("<b>Userlogin:</b> " + this._login + "<br>");
        }

        private string AddTimeNow(string message)
        {
            string str1 = message;
            string str2 = string.Format("<b>Time:</b> {0:G} МСК <br>", (object)DateTime.UtcNow.AddHours(3.0));
            return message = str1 + str2;
        }

        private void SendData(Uri url, string title, string message)
        {
            this.SendData(url, new LogRequest()
            {
                Title = title,
                Body = message
            });
        }

        private void SendData(Uri url, LogRequest request)
        {
            this._client.UploadStringTaskAsync(url, JsonConvert.SerializeObject((object)request));
        }

        public static string ConverToString(RequestParams req)
        {
            string empty = string.Empty;
            for (int index = 0; index < req.Count; ++index)
            {
                KeyValuePair<string, string> keyValuePair = req[index];
                string key = keyValuePair.Key;
                keyValuePair = req[index];
                string str1 = keyValuePair.Value;
                string str2 = key + "=" + str1;
                if (index != 0)
                    str2 = "&" + str2;
                empty += str2;
            }
            return empty;
        }
    }
}
