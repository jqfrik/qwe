using ForksWebAPI.Common.Client;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveForks.Admin.Providers.Positiv
{
    public interface IServiceForksClient
    {
        bool _isFirstLogin { get; set; }

        bool IsLogin { get; }

        void Login(string login, string password);

        void ReLogin();

        List<BetData> GetBetsData(Fork fork);

        List<BetData> GetBetsDataWithoutSubscribe(Fork fork);

        List<Fork> GetForks();

        bool CheckSubscribe();

        void UpdateStakesSetting(DublicateList data);

        List<string> GetUrlFromPositive(Fork fork);
    }
}