using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForksWebAPI.DATA;
using ForksWebAPI.DATA.Models;
using ForksWebAPI.DATA.Request;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForksWebAPI.Controllers
{

    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {

        ForksDbContext _dbContext;
        public AuthenticationController(ForksDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /*
         ПОСТ-ЗАПРОС
         KEY И PASSWORD ПЕРЕДАЁТСЯ В JSON ФОРМАТЕ
         ВАЛИДИРУЕТ ПО КЛЮЧУ И ПАРОЛЮ
        СТАВИТ ISACTIVE В TRUE
        ОБНОВЛЯЕТ СТАТУС ПОЛЬЗОВАТЕЛ В БД
        СОХРАНЯЕТ ИЗМЕНЕНИЯ
        ВОЗВРАЩАЕТ HASH ПОЛЬЗОВАТЕЛЯ
         */
       [HttpPost]
        [Route("Post")]
        public async Task<ActionResult<string>> Post([FromBody]AuthenticationRequest authRequest)
        {
            var user = _dbContext.Users.Where(u => u.Key == authRequest.Key).ToList().FirstOrDefault();
            if (user == null)
            {
                return BadRequest("не верный логин");
            }
            if (user.Password != authRequest.Password)
            {
                return BadRequest("не верный пароль");
            }
            //if (user.IsActive == true)
            //{
            //    return BadRequest("Пользователь уже авторизован");
            //}

            var hash = user.Hash;

             user.IsActive = true;
            _dbContext.Users.Update(user);
            _dbContext.SaveChangesAsync();

            return hash;
        }


    }
}
