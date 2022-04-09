using AppneuronUnity.Core.BaseServices.WebsocketAdapter.WebsocketSharp;

namespace AppneuronUnity.Core.CoreModule.Components.AdvDataComponent.DataManager
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
    using AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact;
    using AppneuronUnity.Core.CoreModule.Components.AdvDataComponent.DataAccess;
    using AppneuronUnity.Core.CoreModule.Components.AdvDataComponent.DataModel;
    using Zenject;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager;

    internal class AdvEventUnityManager : IAdvEventUnityManager
    {
        private readonly ICryptoServices _cryptoServices;

        private readonly IAdvEventDal _advEventDal;

        private readonly IClientIdUnityManager _clientIdUnityManager;

        private readonly IDataCreationClient _dataCreationClient;

        [Inject]
        private readonly CoreHelper coreHelper;
        public AdvEventUnityManager(ICryptoServices cryptoServices,
            IAdvEventDal advEventDal,
            IDataCreationClient dataCreationClient,
            IClientIdUnityManager clientIdUnityManager)
        {
            _cryptoServices = cryptoServices;
            _advEventDal = advEventDal;
            _dataCreationClient = dataCreationClient;
            _clientIdUnityManager = clientIdUnityManager;
        }

        public async Task CheckAdvFileAndSendData()
        {
            List<string> FolderNameList = coreHelper.GetSavedDataFilesNames<AdvEventDataModel>();
            if (FolderNameList.Count == 0)
                return;
            foreach (var fileName in FolderNameList)
            {
                var dataModel = await _advEventDal.SelectAsync(fileName);

                await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
                    coreHelper.GetProjectInfo().ProjectId,
                dataModel, async (result) =>
                {
                    if (result)
                    {
                        await _advEventDal.DeleteAsync(fileName);
                    }

                });

            }
        }

        public async Task SendAdvEventData(string Tag,
            string levelName,
            int levelIndex,
            float GameSecond,
            long clientId)
        {
            AdvEventDataModel advEventDataModel = new AdvEventDataModel
            {
                ClientId = clientId,
                ProjectId = coreHelper.GetProjectInfo().ProjectId,
                CustomerId = coreHelper.GetProjectInfo().CustomerId,
                LevelName = levelName,
                LevelIndex = levelIndex,
                AdvType = Tag,
                InMinutes = GameSecond,
                TrigerdTime = DateTime.Now
            };

            await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
            coreHelper.GetProjectInfo().ProjectId,
            advEventDataModel, async (result) =>
            {

                if (!result)
                {
                    string fileName = _cryptoServices.GenerateStringName(6);
                    await _advEventDal.InsertAsync(fileName, advEventDataModel);
                }


            });

        }
    }
}
