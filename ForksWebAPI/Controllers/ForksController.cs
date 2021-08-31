using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForksWebAPI.Common.Client;
using ForksWebAPI.DATA;
using ForksWebAPI.DATA.Models;
using ForksWebAPI.Models;
using LiveForks.Admin.Providers.Positiv;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForksWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ForksController : Controller
    {
        ForksDbContext _dbContext;
        //PositivClient _pos;

        public ForksController(ForksDbContext dbContext)
        {
            _dbContext = dbContext;
            //_pos = pos;
        }
        /*public ForksController(ForksDbContext dbContext)
        {
            _dbContext = dbContext;
        }*/
        /// <summary>
        /// КЛАДЁТ ВИЛКУ В ФОРМАТЕ ID(GUID) И VALUE(STRING) 
        /// </summary>
        /// <param name="forks"></param>
        [HttpPost]
        [Route("PutForks")]
        public void PutForks([FromBody]string forks)//показыает что привязка с помощью текста запроса
        {
            string res = ValidAuthorization.GetJWT(Request);

            if (!ValidAuthorization.IsValidTokenAdmin(_dbContext, res).Result)
            {
                BadRequest("No access");
            }


            if (forks != null)
            {
                try
                {
                    if (_dbContext.Forks.Any())//провека на существовании хотя бы одной вилки
                    {
                        var all = from c in _dbContext.Forks select c;
                        _dbContext.Forks.RemoveRange(all);
                        _dbContext.SaveChanges();
                    }

                    //Временно 
                    //_dbContext.Forks.Add(new ForkDB { Value = forks });
                    //_dbContext.SaveChanges();
                }
                catch (Exception)
                {
                }

            }

        }
        /// <summary>
        /// ПОЛУЧАЕТ ВСЕ ВИЛКИ 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetForks")]
        public async Task<List<Fork>> GetForks()
        {
            string token = ValidAuthorization.GetJWT(Request);

            if (!ValidAuthorization.IsValidToken(_dbContext, token).Result)
            {
                BadRequest("No access");
            }
            List<Fork> forks = new List<Fork>();
            try
            {
                //Какая-то магическая строка,позволяет взять все вилки с Bet
                var all = _dbContext.Forks.Include(e => e.OneBet).Include(e => e.TwoBet).ToList();

                forks = (from f in _dbContext.Forks select f).ToList();
                var oneFork = forks.FirstOrDefault();

                //List<BetData> positiv = _pos.GetBetsData(oneFork);
            }
            catch (Exception ex)
            {
                
            }

            if(forks == null)
            {
                return new List<Fork>();
               // return new List<Fork>();
            }else
            {
                return forks;
            }
        }
    }
}
