using ForksWebAPI.Common.Client;
using ForksWebAPI.DATA;
using ForksWebAPI.DATA.Models;
using LiveForks.Admin.Providers.Positiv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace ForksWebAPI.BackgroundTask
{

    public class ConsumeParserService : BackgroundService
    {
        private readonly ILogger<ConsumeParserService> _logger;
        private Timer timer { get; set; }
        public IServiceProvider Services { get; }
        private IServiceForksClient forksClient { get; set; }
        private ForksDbContext dbContext { get; set; } 
        public ConsumeParserService(IServiceProvider services,
            ILogger<ConsumeParserService> logger)
        {
            Services = services;
            _logger = logger;
            this.forksClient = Services.CreateScope().ServiceProvider.GetRequiredService<IServiceForksClient>();
            this.dbContext = Services.CreateScope().ServiceProvider.GetRequiredService<ForksDbContext>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
               "Background task is executing");
            timer = new Timer(async (o) => { try { await MainLogic(stoppingToken); } catch (Exception ex) { _logger.LogInformation(ex.Message); }  }, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }
        private async Task MainLogic(CancellationToken stoppingToken)
        {
                _logger.LogInformation(
                    "Background task is working.");
                using (var scope = Services.CreateScope())
                {
                if (!this.forksClient.IsLogin && this.forksClient._isFirstLogin)
                {
                    this.forksClient.Login("username-1000@mail.ru", "hujikolp01");
                }
                List<Fork> forks = null;
                try
                {
                    forks = this.forksClient.GetForks();
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Ошибка при получении вилок");
                }

                //Беру самую нижнюю вилку с позитива,получаю актуальные Url,вставляю в Fork,удаляю последнюю вилку из всего массива
                //добавляю модифицированную вилку
                Fork lastFork = forks[forks.Count - 1];
                List<string> urls = null;
                try
                {
                    urls = this.forksClient.GetUrlFromPositive(lastFork);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                }
                if(urls != null)
                {
                    lastFork.OneBet.Url = urls[0];
                    lastFork.TwoBet.Url = urls[1];
                    forks.RemoveAt(forks.Count - 1);
                    forks.Add(lastFork);
                }
                //List<BetData>
                /*foreach(var fork in forks)
                {
                    scopedPositivClientService.GetBetsData(fork);
                }*/

                /*if (dbContext.Forks.Any())
                {
                    var allForks = from i in dbContext.Forks select i;
                    dbContext.Forks.RemoveRange(allForks);
                    //var allBets = from i in dbContext.Bet select i;
                    //dbContext.Bet.RemoveRange(allBets);
                    dbContext.SaveChanges();
                }*/
                //Версию EntityFramework нужно подобрать(слишком высокая не подходит,слишком низкая не подходит)
                //dbContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT(Forks, RESEED, 0)");

                /*if(forks != null) {
                    dbContext.Forks.AddRange(forks);
                    foreach (var item in forks)
                    {
                        dbContext.Entry(item).State = EntityState.Detached;
                    }
                    dbContext.SaveChanges();
                }*/
                List<Fork> NoRepeatForks = new List<Fork>();
                if(forks != null) {
                    //Делал фильтрацию вилок по Id ,позитив сделал Id = 0 перестало работать
                    /*
                    foreach (var fork in forks)
                    {
                        //TODO:Поменять код чтобы каждая вилка обновлялась на новую,если дубликат,то заменить
                        var allForks = dbContext.Forks.Where(forkes => forkes.Id == fork.Id).ToList();
                        if (allForks.Count == 0)
                        {
                            NoRepeatForks.Add(fork);
                        }
                    }
                    int countForks = 0;
                    countForks = NoRepeatForks.Count == 0 ? 0 : NoRepeatForks.Count;
                    _logger.LogInformation(string.Format("Количество новых вилок: {0}", countForks));
                    */
                    //Добавление Forks
                    this.dbContext.Forks.AddRange(forks);
                    this.dbContext.SaveChanges();
                }
                }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Background task was stopped");

            await base.StopAsync(stoppingToken);
        }
    }
}
