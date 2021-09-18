using System;
using System.Collections.Generic;
using System.Text;

namespace Appneuron.Services
{
    public static class WebApiConfigService
    {
        public const string AuthWebApiLink = "http://localhost:52039/api/Auth/";
        public const string ClientWebApiLink = "https://localhost:44370/WebAPI/api/";

        public const string AdvEventsRequestName = "AdvEvents";
        public const string BuyingEventsRequestName = "BuyingEvents";
        public const string DailySessionDatasRequestName = "DailySessionDatas";
        public const string EveryLoginLevelDatasRequestName = "EveryLoginLevelDatas";
        public const string GameSessionEveryLoginDatasRequestName = "GameSessionEveryLoginDatas";
        public const string GeneralDatasRequestName = "GeneralDatas";
        public const string LevelBaseDieDatasRequestName = "LevelBaseDieDatas";
        public const string LevelBaseSessionDatasRequestName = "LevelBaseSessionDatas";
        public const string ClientTokenRequestName = "clienttoken";
        public const string MlResultRequestName = "MlResultModels/getbyproductId";

    }
}
