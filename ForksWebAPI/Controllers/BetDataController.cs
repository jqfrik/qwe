using ForksWebAPI.Common.Client;
using ForksWebAPI.DATA;
using ForksWebAPI.DATA.Models;
using ForksWebAPI.DATA.Request;
using ForksWebAPI.DATA.Response;
using ForksWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForksWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class BetDataController : Controller
    {
        ForksDbContext _dbContext;

        public BetDataController(ForksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Данные о ставке запроса на размещение
        /// </summary>
        /// <param name="requestBet"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PutRequestBetData")]
        public async Task<IActionResult> PutRequestBetData([FromBody]string requestBet)
        {
            //ПОЛУЧАЕТ ТОКЕН ИЗ РЕКВЕСТА
            string token = ValidAuthorization.GetJWT(Request);
            //ЕСЛИ ТОКЕН НЕ ВАЛИДНЫЙ ВЕРНУТЬ BADREQUEST
            if (!ValidAuthorization.IsValidToken(_dbContext, token).Result)
            {
                BadRequest("No access");
            }
            /*ЕСЛИ ВСЁ ХОРОШО
             * ДЕСЕРИАЛИЗУЕМ ЗАПРОС В REQUESTBETDATAJSON
             * ПОТОМ ЕЩЁ ДЕСЕРИАЛИЗУЕМ В FORK (ЗНАЧЕНИЯ VALUE ИЗ REQUEST'A)
             * ПРОВЕРЯЕМ НА СУЩЕСТВОВАНИЕ В ТАБЛИЦЕ REQUESTBETDATAS
             * ЕСЛИ НЕ СУЩЕСТВУЕТ,СЕРИАЛИЗУЕМ ВИЛКУ
             * ДОБАВЛЯЕМ В ТАБЛИЦУ REQUESTBETDATAS ЭЛЕМЕНТ REQUESTBETDATA
            */
            try
            {
                var request = JsonConvert.DeserializeObject<RequestBetDataJSON>(requestBet);


                var forkClient =JsonConvert.DeserializeObject<Fork>(request.Value);

                var user = (from u in _dbContext.RequestBetDatas
                            where u.CridId == forkClient.CridId
                            select u).ToList();

                if (user.Count == 0)
                {
                    var forkClentJSON = JsonConvert.SerializeObject(forkClient);

                    RequestBetData betData = new RequestBetData()
                    { CridId = forkClient.CridId, Key = request.Key, Value = forkClentJSON, DateCreation = DateTime.Now };

                    _dbContext.RequestBetDatas.Add(betData);
                    _dbContext.SaveChangesAsync();



                    //var delBets = from d in _dbContext.RequestBetDatas
                    //              where d.DateCreation.AddMinutes(5) < DateTime.Now
                    //              select d;
                    //_dbContext.RequestBetDatas.RemoveRange(delBets);
                    //_dbContext.SaveChangesAsync();

                }

            }
            catch (Exception) { return NotFound(); }

            return Ok();

        }
        /*ПРИНИМАЕТ СТРОКУ RESPONSEBET
        ДЕСЕРИАЛИХУЕТ В RESPONSEBETDATAJSON
        ПРОВЕРЯЕТ СУЩЕСТВУЕТ ЛИ RESPONSE.cRIDID В RESPONSEBETDATAS
        ЕСЛИ СУЩЕСТВУЕТ НИЧЕГО НЕ ДЕЛАЕМ
        ИНАЧЕ СОЗДАЁМ НОВЫЙ ЭЛЕМЕНТ RESPONSEBETDATA СО ЗНАЧЕНИЯМИ ИЗ ТЕЛА ЗАПРОСА
        И ДОБАВЛЯЕМ В ТАБЛИЦУ RESPONSEBETDATAS
        */
        [HttpPost]
        [Route("PutResponseBetDatas")]
        public async Task<IActionResult> PutResponseBetDatas([FromBody]string responseBet)
        {
            string token = ValidAuthorization.GetJWT(Request);

            if (!ValidAuthorization.IsValidTokenAdmin(_dbContext, token).Result)
            {
                BadRequest("No access");
            }

            var response = JsonConvert.DeserializeObject<ResponseBetDataJSON>(responseBet);


            var datas = (from u in _dbContext.ResponseBetDatas select u).ToList();

            bool res = true;
            foreach (var item in datas)
            {
                if (item.CridId == response.CridId)
                {
                    res = false;
                }
            }

            if (res)
            {

                try
                {
                    var betData = new ResponseBetData()
                    { CridId = response.CridId, Key = response.Key, Value = response.Value, DateCreation = DateTime.Now };

                    _dbContext.ResponseBetDatas.Add(betData);
                    _dbContext.SaveChangesAsync();

                    //var delBets = from d in _dbContext.ResponseBetDatas
                    //              where d.DateCreation.AddMinutes(5) < DateTime.Now
                    //              select d;
                    //_dbContext.ResponseBetDatas.RemoveRange(delBets);
                    //_dbContext.SaveChangesAsync();
                }
                catch (Exception)
                {

                    NotFound();
                }


            }


            return Ok();

        }

        [HttpGet]
        [Route("GetRequestBetData")]
        public async Task<List<Fork>> GetRequestBetData()
        {

            string token = ValidAuthorization.GetJWT(Request);

            if (!ValidAuthorization.IsValidTokenAdmin(_dbContext, token).Result)
            {
                BadRequest("No access");
            }

            string res = string.Empty;
            List<Fork> ListForks = new List<Fork>();

            var forkRequest = (from b in _dbContext.RequestBetDatas select b).ToList();
            var forkResponse = (from b in _dbContext.ResponseBetDatas select b).ToList();

            if (forkRequest.Count > 0)
            {
                try
                {
                    bool r = true;
                    foreach (var item in forkRequest)
                    {
                        foreach (var item2 in forkResponse)
                        {
                            if (item.CridId == item2.CridId) { r = false; }
                        }
                        if (r)
                        {
                            ListForks.Add(JsonConvert.DeserializeObject<Fork>(item.Value));

                        }
                        else
                        {
                            r = true;
                        }

                    }
                }
                catch (Exception) { }

            }

            return ListForks;
        }

        /// <summary>
        /// Получает CridId
        /// По CridId находит в ResponseBetDatas item
        /// Десериализует item в BetData
        /// И отправляет список List<BetData> на клиент иначе возврат пустого массива
        /// </summary>
        /// <param name="cridId"></param>
        /// <returns></returns>
        [HttpGet("API/{cridId}")]
        [Route("GetResponseBetDatas")]
        public async Task<List<BetData>> GetResponseBetDatas([FromRoute]string cridId)
        {
            string token = ValidAuthorization.GetJWT(Request);

            if (!ValidAuthorization.IsValidToken(_dbContext, token).Result)
            {
                BadRequest("No access");
            }

            List<BetData> betDataListClient = new List<BetData>();

            try
            {
                var datas = (from b in _dbContext.ResponseBetDatas select b).ToList();
                foreach (var item in datas)
                {
                    if (item.CridId == cridId)
                    {
                        betDataListClient =JsonConvert.DeserializeObject<List<BetData>>(item.Value);
                        return betDataListClient;
                    }
                }
            }
            catch (Exception)
            {
            }
            return betDataListClient;
        }

    }
}
