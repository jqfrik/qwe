using ForksWebAPI.Common.Client;
using ForksWebAPI.DATA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForksWebAPI.DATA
{
    public interface IEntity
    {
        long Id { get; }
    }
    public class ForksDbContext : DbContext
    {
        public ForksDbContext(DbContextOptions opts) : base(opts)
        {
        }

        public DbSet<RequestAnotherBet> RequestAnotherBets { get; set; }
        public DbSet<ResponseAnotherBet> ResponseAnotherBets { get; set; }

        public DbSet<RequestBetData> RequestBetDatas { get; set; }
        public DbSet<ResponseBetData> ResponseBetDatas { get; set; }

        public DbSet<Bet> Bet { get; set; }

        public DbSet<Fork> Forks { get; set; }
        public DbSet<UserDB> Users { get; set; }

    }
}
