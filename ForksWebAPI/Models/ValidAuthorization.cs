using ForksWebAPI.DATA;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForksWebAPI.Models
{
    public static  class ValidAuthorization
    {
        public static string GetJWT(HttpRequest request)
        {
            string hash = request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
            string res = hash.Trim('"')
;
            return res;
        }

        public static async Task<bool> IsValidToken(ForksDbContext dbContext, string token)
        {
            foreach (var item in dbContext.Users)
            {
                if (item.Hash == token)
                {
                    return true;
                }
            }

            return false;
        }

        public static async Task<bool> IsValidTokenAdmin(ForksDbContext dbContext, string token)
        {
            foreach (var item in dbContext.Users)
            {
                if (item.Hash == token && item.Roles == Roles.Admin)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

