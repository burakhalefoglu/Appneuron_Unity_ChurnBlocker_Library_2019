namespace AppneuronUnity.Core.CoreModule.Components.LocationComponent.UnityManager
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System.Threading.Tasks;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.UnityManager;
    using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using AppneuronUnity.Core.Adapters.RestClientAdapter.Abstract;
using AppneuronUnity.Core.CoreModule.Components.LocationComponent.DataModel;
using AppneuronUnity.Core.CoreModule.Components.LocationComponent.UnityManager;

    internal class LocationUnityManager : ILocationUnityManager
    {
        private readonly IClientIdUnityManager _clientIdUnityManager;

        internal IRestClientServices _restClientService;

        private readonly IDataCreationClient _dataCreationClient;

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
            locationData.ProjectId = ChurnBlockerSingletonConfigs.Instance.GetProjectID();
            locationData.CustomerId = ChurnBlockerSingletonConfigs.Instance.GetCustomerID();
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
