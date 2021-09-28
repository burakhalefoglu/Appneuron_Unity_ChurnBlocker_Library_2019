namespace AppneuronUnity.Core.CoreModule.Components.LocationComponent.DataManager
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System.Threading.Tasks;
    using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
    using AppneuronUnity.Core.Adapters.RestClientAdapter.Abstract;
    using AppneuronUnity.Core.CoreModule.Components.LocationComponent.DataModel;
    using Zenject;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager;

    internal class LocationUnityManager : ILocationUnityManager
    {
        private readonly IClientIdUnityManager _clientIdUnityManager;

        private readonly IRestClientServices _restClientService;

        private readonly IDataCreationClient _dataCreationClient;

        [Inject]
        private readonly CoreHelper coreHelper;

        public LocationUnityManager(IDataCreationClient dataCreationClient,
            IRestClientServices restClientService,
            IClientIdUnityManager clientIdUnityManager)
        {
            _dataCreationClient = dataCreationClient;
            _restClientService = restClientService;
            _clientIdUnityManager = clientIdUnityManager;
        }

        public async Task SendMessage()
        {
            var resultLocation = await _restClientService.GetAsync<LocationModel>
            ("https://extreme-ip-lookup.com/json");
            var locationData = resultLocation.Data;
            locationData.ClientId = _clientIdUnityManager.GetPlayerID();
            locationData.ProjectId =coreHelper.GetProjectInfo().ProjectID;
            locationData.CustomerId = coreHelper.GetProjectInfo().CustomerID;
            await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
            locationData, async (result) =>
            {
                if (!result)
                {
                    //TODO: Send Log

                }
            });
        }
    }
}
