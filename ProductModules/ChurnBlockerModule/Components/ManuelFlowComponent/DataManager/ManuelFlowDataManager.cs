using AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact;
using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataAccess;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataModel;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppneuronUnity.Core.BaseServices.WebsocketAdapter.WebsocketSharp;
using Zenject;

namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.ManuelFlowComponent.DataManager
{
    internal class ManuelFlowDataManager: IManuelFlowDataManager
    {
        private readonly ICryptoServices _cryptoServices;

        private readonly IManuelFlowDal _manuelFlowDal;

        private readonly IClientIdUnityManager _clientIdUnityManager;

        private readonly IDataCreationClient _dataCreationClient;

        [Inject]
        private readonly CoreHelper coreHelper;

        public ManuelFlowDataManager(ICryptoServices cryptoServices,
            IManuelFlowDal manuelFlowDal,
            IClientIdUnityManager clientIdUnityManager,
            IDataCreationClient dataCreationClient)
        {
            _cryptoServices = cryptoServices;
            _manuelFlowDal = manuelFlowDal;
            _clientIdUnityManager = clientIdUnityManager;
            _dataCreationClient = dataCreationClient;
        }

        public async Task CheckAdvFileAndSendData()
        {
            List<string> FolderNameList = coreHelper.GetSavedDataFilesNames<ManuelFlowModel>();
            if (FolderNameList.Count == 0)
                return;
            foreach (var fileName in FolderNameList)
            {
                var dataModel = await _manuelFlowDal.SelectAsync(fileName);

                await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
                coreHelper.GetProjectInfo().ProjectId,
                dataModel, async (result) =>
                {
                    if (result)
                    {
                        await _manuelFlowDal.DeleteAsync(fileName);
                    }

                });

            }
        }

        public async Task SendManuelFlowData(ManuelFlowModel manuelFlowModel)
        {
            manuelFlowModel.ClientId = await _clientIdUnityManager.GetPlayerIdAsync();
            manuelFlowModel.CustomerId = coreHelper.GetProjectInfo().CustomerId;
            manuelFlowModel.ProjectId = coreHelper.GetProjectInfo().ProjectId;

            await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
            coreHelper.GetProjectInfo().ProjectId,
            manuelFlowModel, async (result) =>
            {

                if (!result)
                {
                    string fileName = _cryptoServices.GenerateStringName(6);
                    await _manuelFlowDal.InsertAsync(fileName, manuelFlowModel);
                }


            });

        }
    }
}
