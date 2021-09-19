using System;
using System.Collections.Generic;
using System.Text;

namespace Appneuron.Services
{
    public static class WebApiConfigService
    {
        public const string AuthWebApiLink = "http://localhost:52039/api/Auth/";
        public const string ClientWebApiLink = "https://localhost:44370/WebAPI/api/";
        public const string ClientTokenRequestName = "clienttoken";
        public const string MlResultRequestName = "MlResultModels/getbyproductId";

    }
}
