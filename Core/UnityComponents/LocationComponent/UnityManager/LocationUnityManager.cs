namespace AppneuronUnity.Core.UnityComponents.LocationComponent.UnityManager
{
    using AppneuronUnity.Core.CoreServices.MessageBrokers;
    using AppneuronUnity.Core.CoreServices.RestClientServices.Abstract;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System.Threading.Tasks;
using AppneuronUnity.Core.UnityComponents.LocationComponent.DataModel;
using AppneuronUnity.Core.UnityComponents.LocationComponent.UnityManager;
using AppneuronUnity.Core.UnityWorkers.ClientIdWorker.UnityManager;

    /// <summary>
    /// Defines the <see cref="LocationUnityManager" />.
    /// </summary>
    internal class LocationUnityManager : ILocationUnityManager
    {
        /// <summary>
        /// Defines the _kafkaMessageBroker.
        /// </summary>
        internal IMessageBrokerService _kafkaMessageBroker;

        /// <summary>
        /// Defines the _restClientService.
        /// </summary>
        internal IRestClientServices _restClientService;

        /// <summary>
        /// Defines the _kafkaMessageBroker.
        /// </summary>
        private readonly IClientIdUnityManager _clientIdUnityManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationUnityManager"/> class.
        /// </summary>
        /// <param name="kafkaMessageBroker">The kafkaMessageBroker<see cref="IMessageBrokerService"/>.</param>
        /// <param name="restClientService">The restClientService<see cref="IRestClientServices"/>.</param>
        public LocationUnityManager(IMessageBrokerService kafkaMessageBroker,
            IRestClientServices restClientService,
            IClientIdUnityManager clientIdUnityManager)
        {
            _kafkaMessageBroker = kafkaMessageBroker;
            _restClientService = restClientService;
            _clientIdUnityManager = clientIdUnityManager;
        }

        /// <summary>
        /// The SendMessage.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendMessage()
        {
            var resultLocation = await _restClientService.GetAsync<LocationModel>
            ("https://extreme-ip-lookup.com/json");
            var locationData = resultLocation.Data;
            locationData.ClientId = _clientIdUnityManager.GetPlayerID();
            locationData.ProjectId = ChurnBlockerSingletonConfigs.Instance.GetProjectID();
            locationData.CustomerId = ChurnBlockerSingletonConfigs.Instance.GetCustomerID();
            var resultKafka = await _kafkaMessageBroker.SendMessageAsync(locationData);

            if (!resultKafka.Success)
            {
                //TODO: Send Log
            }
        }
    }
}
