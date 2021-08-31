using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForksWebAPI.DATA;
using ForksWebAPI.DATA.Response;
using ForksWebAPI.DATA.Request;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ForksWebAPI.Common.Client;
using ForksWebAPI.DATA.Models;
using ForksWebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForksWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AnotherBetController : Controller
    {

        ForksDbContext _dbContext;

        public AnotherBetController(ForksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("PutRequestAnotherBet")]
        public async Task<IActionResult> PutRequestAnotherBet([FromBody]string requestAnotherBet)
        {
            string token = ValidAuthorization.GetJWT(Request);

            if (!ValidAuthorization.IsValidToken(_dbContext, token).Result)
            {
                BadRequest("No access");
            }

            try
            {
                var request = JsonConvert.DeserializeObject<RequestAnotherBetJSON>(requestAnotherBet);

                var fork = JsonConvert.DeserializeObject<Fork>(request.Value);


                var anotherBets = (from u in _dbContext.RequestAnotherBets
                                   where u.CridId == fork.CridId
                                   select u).ToList();

                _dbContext.RequestAnotherBets.RemoveRange(anotherBets);
                _dbContext.SaveChanges();

                var forkClentJSON = JsonConvert.SerializeObject(fork);

                RequestAnotherBet betData = new RequestAnotherBet()
                {
                    CridId = fork.CridId,
                    Value = forkClentJSON,
                    DateCreation = DateTime.Now,
                    Key = request.Key,
                    AnotherBetNumber = request.AnotherBetNumber
                };

                _dbContext.RequestAnotherBets.Add(betData);
                _dbContext.SaveChanges();


                //var res = (from b in _dbContext.RequestAnotherBets select b).ToList();

            }
            catch (Exception)
            {

                return NotFound();
            }

            return Ok();
        }


        [HttpPost]
        [Route("PutResponseAnotherBet")]
        public async Task<IActionResult> PutResponseAnotherBet([FromBody]string responseAnotherBet)
        {
            string token = ValidAuthorization.GetJWT(Request);

            if (!ValidAuthorization.IsValidTokenAdmin(_dbContext, token).Result)
            {
                BadRequest("No access");
            }

            var response = JsonConvert.DeserializeObject<ResponseAnotherBetJSON>(responseAnotherBet);

            var data = (from u in _dbContext.ResponseAnotherBets
                        where u.CridId == response.CridId
                        select u).ToList();

            _dbContext.ResponseAnotherBets.RemoveRange(data);
            _dbContext.SaveChanges();

            var betData = new ResponseAnotherBet()
            {
                CridId = response.CridId,
                Value = response.Value,
                DateCreation = DateTime.Now,
                Key = response.Key,
                AnotherBetNumber = response.AnotherBetNumber
            };

            _dbContext.ResponseAnotherBets.Add(betData);
            _dbContext.SaveChangesAsync();


            return Ok();
        }


        [HttpGet]
        [Route("GetRequestAnotherBet")]
        public async Task<List<RequestAnotherBetJSON>> GetRequestAnotherBet()
        {

            string token = ValidAuthorization.GetJWT(Request);

            if (!ValidAuthorization.IsValidTokenAdmin(_dbContext, token).Result)
            {
                BadRequest("No access");
            }

            List<RequestAnotherBetJSON> ListForks = new List<RequestAnotherBetJSON>();

            var forkRequest = (from b in _dbContext.RequestAnotherBets select b).ToList();
           
            if (forkRequest.Count > 0)
            {
                try
                {
                    bool r = true;
                    foreach (var item in forkRequest)
                    {
                        RequestAnotherBetJSON anotherBetJSON = new RequestAnotherBetJSON();
                        anotherBetJSON.AnotherBetNumber = item.AnotherBetNumber;
                        anotherBetJSON.CridId = item.CridId;
                        anotherBetJSON.Key = item.Key;
                        anotherBetJSON.Value = item.Value;
                        ListForks.Add(anotherBetJSON);
                    }
                    _dbContext.RequestAnotherBets.RemoveRange(forkRequest);
                    _dbContext.SaveChangesAsync();
                }
                catch (Exception) { }

            }

            return ListForks;

        }


        [HttpGet("API/{cridId}")]
        [Route("GetResponseAnotherBet")]
        public async Task<List<AnotherBet>> GetResponseAnotherBet(string cridId)
        {
            string token = ValidAuthorization.GetJWT(Request);

            if (!ValidAuthorization.IsValidToken(_dbContext, token).Result)
            {
                BadRequest("No access");
            }

            List<AnotherBet> anotherBets = new List<AnotherBet>();

            try
            {
                var datas = (from b in _dbContext.ResponseAnotherBets where b.CridId == cridId select b).ToList().FirstOrDefault();
                if (datas != null)
                {
                    anotherBets = JsonConvert.DeserializeObject<List<AnotherBet>>(datas.Value);

                }

                _dbContext.ResponseAnotherBets.Remove(datas);
                _dbContext.SaveChangesAsync();

            }
            catch (Exception)
            {

            }

            return anotherBets;
        }


    }
}
