using Appneuron.Core.DataModel.Concrete;
using Appneuron.Models;
using Appneuron.Services;
using AppneuronUnity.ChurnBlockerModule.Components.DifficultyComponent;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Assets.Appneuron.Core.DataAccess;
using Assets.Appneuron.Core.DataAccess.BinarySaving;
using Assets.Appneuron.Core.DataModel.Concrete;
using Ninject;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AppneuronUnity.Core.UnityManager
{
    public class SecurityUnityManager
    {
        private string filePath = ComponentsConfigService.TokenDataModel;
        private string fileName = ModelNames.TokenName;
        private string RequestPath = WebApiConfigService.AuthWebApiLink + WebApiConfigService.ClientTokenRequestName;

        public async Task Login()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Load(Assembly.GetExecutingAssembly());
                var _jwtDal = kernel.Get<IJwtDal>();
                var _restClientServices = kernel.Get<IRestClientServices>();

                var tokenmodel = await checkTokenOnfile(_jwtDal);
                if (tokenmodel.Token != "" && tokenmodel.Expiration > DateTime.Now)
                {
                    TokenSingletonModel.Instance.Token = tokenmodel.Token;
                    return;

                }

                string projectId = ChurnBlockerSingletonConfigService.Instance.GetProjectID();
                string customerId = ChurnBlockerSingletonConfigService.Instance.GetCustomerID();

                var JwtRequestModel = new JwtRequestModel
                {
                    ClientId = SystemInfo.deviceUniqueIdentifier,
                    DashboardKey = customerId,
                    ProjectId = projectId
                };

                var result = await _restClientServices.PostAsync<JwtResponseModel>
                    (RequestPath,
                    JwtRequestModel);
                if (result.Success)
                {
                    await SaveTokenOnfile(_jwtDal, result.Data.Data);
                    TokenSingletonModel.Instance.Token = result.Data.Data.Token;
                    await new DifficultyManager().AskDifficulty();

                }

            }
        }

        private async Task<TokenDataModel> checkTokenOnfile(IJwtDal _jwtDal)
        {
            var dataModel = await _jwtDal.SelectAsync(filePath + fileName);
            return dataModel;
        }

        private async Task SaveTokenOnfile(IJwtDal _jwtDal, TokenDataModel tokenDataModel)
        {
            TokenSingletonModel.Instance.Token = tokenDataModel.Token;
            await _jwtDal.InsertAsync(filePath + fileName, tokenDataModel);
        }

    }
}
