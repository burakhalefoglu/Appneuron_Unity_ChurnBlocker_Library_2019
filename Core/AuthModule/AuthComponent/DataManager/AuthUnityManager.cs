namespace AppneuronUnity.Core.AuthModule.AuthComponent.DataManager
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System;
    using System.Threading.Tasks;
    using UnityEngine;
    using AppneuronUnity;
    using AppneuronUnity.Core.AuthModule.AuthComponent.DataAccess;
    using AppneuronUnity.Core.AuthModule.AuthComponent.DataModel;
    using AppneuronUnity.Core.Adapters.RestClientAdapter.Abstract;
    using AppneuronUnity.Core.AuthModule.AuthComponent.DataModel.Jwt;
    using Zenject;

    internal class AuthUnityManager : IAuthUnityManager
    {
        private readonly string fileName = Appsettings.TokenName;

        private readonly string RequestPath = Appsettings.AuthWebApiLink + Appsettings.ClientTokenRequestName;

        internal IAuthDal _authDal;

        public delegate Task OnLogin();

        public static event OnLogin onLogin;

        internal IRestClientServices _restClientServices;

        [Inject]
        private readonly CoreHelper coreHelper;

        public AuthUnityManager(IAuthDal authDal, IRestClientServices restClientServices)
        {
            _authDal = authDal;
            _restClientServices = restClientServices;
        }

        public async Task Login()
        {
            var tokenmodel = await checkTokenOnfile();
            if (tokenmodel == null)
                return;
            if (tokenmodel.Token != "" && tokenmodel.Expiration > DateTime.Now)
            {
                TokenSingletonModel.Instance.Token = tokenmodel.Token;
                await onLogin();
                return;
            }

            string projectId = coreHelper.GetProjectInfo().ProjectID;
            string customerId = coreHelper.GetProjectInfo().CustomerID;

            var JwtRequestModel = new AuthRequestModel
            {
                ClientId = SystemInfo.deviceUniqueIdentifier,
                CustomerId = customerId,
                ProjectId = projectId
            };

            var result = await _restClientServices.PostAsync<JwtResponseModel>
                (RequestPath,
                JwtRequestModel);
            if (result == null)
                return;

            if (result.Success)
            {
                await SaveTokenOnfile(result.Data.Data);
                TokenSingletonModel.Instance.Token = result.Data.Data.Token;
                await onLogin();
            }
        }

        public async Task<TokenDataModel> checkTokenOnfile()
        {
            var dataModel = await _authDal.SelectAsync(fileName + fileName);
            return dataModel;
        }

        public async Task SaveTokenOnfile(TokenDataModel tokenDataModel)
        {
            TokenSingletonModel.Instance.Token = tokenDataModel.Token;
            await _authDal.InsertAsync(fileName, tokenDataModel);
        }
    }
}
