namespace AppneuronUnity.Core.UnityComponents.BuyingDataComponent.UnityManager
{
    using AppneuronUnity.Core.CoreServices.CryptoServices.Absrtact;
    using AppneuronUnity.Core.CoreServices.MessageBrokers;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
using AppneuronUnity.Core.UnityComponents.BuyingDataComponent.DataAccess;
using AppneuronUnity.Core.UnityComponents.BuyingDataComponent.DataModel;
using AppneuronUnity.Core.UnityComponents.BuyingDataComponent.UnityManager;

    /// <summary>
    /// Defines the <see cref="BuyingEventManager" />.
    /// </summary>
    internal class BuyingEventManager : IBuyingEventManager
    {
        /// <summary>
        /// Defines the _buyingEventDal.
        /// </summary>
        private readonly IBuyingEventDal _buyingEventDal;

        /// <summary>
        /// Defines the _kafkaMessageBroker.
        /// </summary>
        private readonly IMessageBrokerService _kafkaMessageBroker;

        /// <summary>
        /// Defines the _cryptoServices.
        /// </summary>
        private readonly ICryptoServices _cryptoServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuyingEventManager"/> class.
        /// </summary>
        /// <param name="buyingEventDal">The buyingEventDal<see cref="IBuyingEventDal"/>.</param>
        /// <param name="kafkaMessageBroker">The kafkaMessageBroker<see cref="IMessageBrokerService"/>.</param>
        /// <param name="cryptoServices">The cryptoServices<see cref="ICryptoServices"/>.</param>
        public BuyingEventManager(IBuyingEventDal buyingEventDal,
            IMessageBrokerService kafkaMessageBroker,
            ICryptoServices cryptoServices)
        {
            _buyingEventDal = buyingEventDal;
            _kafkaMessageBroker = kafkaMessageBroker;
            _cryptoServices = cryptoServices;
        }

        /// <summary>
        /// The CheckAdvFileAndSendData.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CheckAdvFileAndSendData()
        {
            List<string> FolderList = ComponentsConfigs.GetSavedDataFilesNames(ComponentsConfigs
                                                                                      .SaveTypePath
                                                                                      .BuyingEventDataModel);

            foreach (var fileName in FolderList)
            {
                var dataModel = await _buyingEventDal.SelectAsync(ComponentsConfigs.BuyingEventDataPath + fileName);

                var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                if (result.Success)
                {
                    await _buyingEventDal.DeleteAsync(ComponentsConfigs.AdvEventDataPath + fileName);
                }
            }
        }

        /// <summary>
        /// The SendAdvEventData.
        /// </summary>
        /// <param name="Tag">The Tag<see cref="string"/>.</param>
        /// <param name="levelName">The levelName<see cref="string"/>.</param>
        /// <param name="GameSecond">The GameSecond<see cref="float"/>.</param>
        /// <param name="clientId">The clientId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendAdvEventData(string Tag,
            string levelName,
            int levelIndex,
            float GameSecond,
            string clientId)
        {
            BuyingEventDataModel dataModel = new BuyingEventDataModel
            {
                ClientId = clientId,
                ProjectId = ChurnBlockerSingletonConfigs.Instance.GetProjectID(),
                CustomerId = ChurnBlockerSingletonConfigs.Instance.GetCustomerID(),
                LevelName = levelName,
                LevelIndex = levelIndex,
                ProductType = Tag,
                InWhatMinutes = GameSecond,
                TrigerdTime = DateTime.Now
            };

            var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
            if (result.Success)
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            string filepath = ComponentsConfigs.AdvEventDataPath + fileName;

            await _buyingEventDal.InsertAsync(filepath, dataModel);
        }
    }
}
