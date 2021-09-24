namespace AppneuronUnity.Core.UnityWorkers.AuthWorker.UnityManager
{
    using AppneuronUnity.Core.CoreServices.RestClientServices.Abstract;
    using AppneuronUnity.Core.Models.Concrete.Jwt;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System;
    using System.Threading.Tasks;
    using UnityEngine;
using AppneuronUnity.Core.UnityWorkers.AuthWorker.DataAccess;
using AppneuronUnity.Core.UnityWorkers.AuthWorker.DataModel;
using AppneuronUnity.Core.UnityWorkers.AuthWorker.UnityManager;
using AppneuronUnity;

    /// <summary>
    /// Defines the <see cref="AuthUnityManager" />.
    /// </summary>
    internal class AuthUnityManager : IAuthUnityManager
    {
        /// <summary>
        /// Defines the filePath.
        /// </summary>
        private readonly string filePath = ComponentsConfigs.TokenDataModel;

        /// <summary>
        /// Defines the fileName.
        /// </summary>
        private readonly string fileName = Websettings.TokenName;

        /// <summary>
        /// Defines the RequestPath.
        /// </summary>
        private readonly string RequestPath = Appsettings.AuthWebApiLink + Appsettings.ClientTokenRequestName;

        /// <summary>
        /// Defines the _authDal.
        /// </summary>
        internal IAuthDal _authDal;

        /// <summary>
        /// The OnCheckLoacalData.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public delegate Task OnLogin();

        /// <summary>
        /// Defines the CheckLocalData.
        /// </summary>
        public static event OnLogin onLogin;

        /// <summary>
        /// Defines the _restClientServices.
        /// </summary>
        internal IRestClientServices _restClientServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthUnityManager"/> class.
        /// </summary>
        /// <param name="authDal">The authDal<see cref="IAuthDal"/>.</param>
        /// <param name="restClientServices">The restClientServices<see cref="IRestClientServices"/>.</param>
        public AuthUnityManager(IAuthDal authDal, IRestClientServices restClientServices)
        {
            _authDal = authDal;
            _restClientServices = restClientServices;
        }

        /// <summary>
        /// The Login.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task Login()
        {
            var tokenmodel = await checkTokenOnfile();
            if (tokenmodel.Token != "" && tokenmodel.Expiration > DateTime.Now)
            {
                TokenSingletonModel.Instance.Token = tokenmodel.Token;
                await onLogin();
                return;
            }

            string projectId = ChurnBlockerSingletonConfigs.Instance.GetProjectID();
            string customerId = ChurnBlockerSingletonConfigs.Instance.GetCustomerID();

            var JwtRequestModel = new AuthRequestModel
            {
                ClientId = SystemInfo.deviceUniqueIdentifier,
                CustomerId = customerId,
                ProjectId = projectId
            };

            var result = await _restClientServices.PostAsync<JwtResponseModel>
                (RequestPath,
                JwtRequestModel);
            if (result.Success)
            {
                await SaveTokenOnfile(result.Data.Data);
                TokenSingletonModel.Instance.Token = result.Data.Data.Token;
                await onLogin();
            }
        }

        /// <summary>
        /// The checkTokenOnfile.
        /// </summary>
        /// <returns>The <see cref="Task{TokenDataModel}"/>.</returns>
        public async Task<TokenDataModel> checkTokenOnfile()
        {
            var dataModel = await _authDal.SelectAsync(filePath + fileName);
            return dataModel;
        }

        /// <summary>
        /// The SaveTokenOnfile.
        /// </summary>
        /// <param name="tokenDataModel">The tokenDataModel<see cref="TokenDataModel"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SaveTokenOnfile(TokenDataModel tokenDataModel)
        {
            TokenSingletonModel.Instance.Token = tokenDataModel.Token;
            await _authDal.InsertAsync(filePath + fileName, tokenDataModel);
        }
    }
}
