
using AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact;
using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Components.OfferBehavior.DataAccess;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Components.OfferBehavior.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppneuronUnity.Core.BaseServices.WebsocketAdapter.WebsocketSharp;
using Zenject;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.Components.OfferBehavior.DataManager
{
    internal class OfferBehaviorManager : IOfferBehaviorManager
    {
        private readonly IClientIdUnityManager _clientIdUnityManager;

        private readonly IDataCreationClient _dataCreationClient;

        private readonly ICryptoServices _cryptoServices;

        private readonly IOfferBehaviorDal _offerBehaviorDal;

        [Inject]
        private readonly CoreHelper coreHelper;

        public OfferBehaviorManager(IClientIdUnityManager clientIdUnityManager,
            IDataCreationClient dataCreationClient,
            ICryptoServices cryptoServices,
            IOfferBehaviorDal offerBehaviorDal)
        {
            _clientIdUnityManager = clientIdUnityManager;
            _dataCreationClient = dataCreationClient;
            _cryptoServices = cryptoServices;
            _offerBehaviorDal = offerBehaviorDal;
        }

        public async Task CheckFileAndSendDataAsync()
        {
            List<string> FolderList = coreHelper.GetSavedDataFilesNames<OfferBehaviorModel>();
            if (FolderList.Count == 0)
                return;
            foreach (var fileName in FolderList)
            {
                var dataModel = await _offerBehaviorDal.SelectAsync(fileName);

                await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
                coreHelper.GetProjectInfo().ProjectId,
                dataModel, async (result) =>
                {
                    if (result)
                    {
                        await _offerBehaviorDal.DeleteAsync(fileName);

                    }
                });
            }
        }

        public async Task<bool> OfferIdIsInValidOnLocalAsync(int OfferId)
        {
            List<string> FolderList = coreHelper.GetSavedDataFilesNames<OfferBehaviorModel>();
            if (FolderList.Count == 0)
                return true;
            foreach (var fileName in FolderList)
            {
                var dataModel = await _offerBehaviorDal.SelectAsync(fileName);
                if(dataModel.OfferId>= OfferId)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task SendAdvEventDataAsync(int OfferId,
            bool isBuyOffer)
        {
            var isBought = isBuyOffer == true ? 1 : 0;
            OfferBehaviorModel dataModel = new OfferBehaviorModel
            {
                ClientId = await _clientIdUnityManager.GetPlayerIdAsync(),
                ProjectId = coreHelper.GetProjectInfo().ProjectId,
                CustomerId = coreHelper.GetProjectInfo().CustomerId,
                OfferId = OfferId,
                IsBuyOffer = isBought
            };

            await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
            coreHelper.GetProjectInfo().ProjectId,
            dataModel, async (result) =>
            {
                    await CheckFileAndSendDataAsync();
                    string fileName = _cryptoServices.GenerateStringName(6);
                    await _offerBehaviorDal.InsertAsync(fileName, dataModel);
            });

        }
    }
}