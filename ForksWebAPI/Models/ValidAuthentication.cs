using ForksWebAPI.DATA;
using ForksWebAPI.DATA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForksWebAPI.Models
{
        public interface IValidAuthentication
        {
            Task<bool> IsValidToken(string token);
            Task<bool> IsValidTokenAdmin(string token);
        }

    public class ValidAuthentication : IValidAuthentication
    {
        private readonly ForksDbContext _dbContext;
        private readonly List<UserDB> _userDB;

        public ValidAuthentication(ForksDbContext dbContext)
        {
            _dbContext = dbContext;

            _userDB = (from u in _dbContext.Users select u).ToList();
        }

        public async Task<bool> IsValidToken(string token)
        {
            foreach (var item in _userDB)
            {
                if (item.Hash == token)
                {
                    return true ;
                }
            }

            return false;
        }

        public async Task<bool> IsValidTokenAdmin(string token)
        {
            foreach (var item in _userDB)
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
