namespace AppneuronUnity.Core.UnityComponents.AdvDataComponent.UnityManager
{
    using AppneuronUnity.Core.CoreServices.CryptoServices.Absrtact;
    using AppneuronUnity.Core.CoreServices.MessageBrokers;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
using AppneuronUnity.Core.UnityComponents.AdvDataComponent.DataAccess;
using AppneuronUnity.Core.UnityComponents.AdvDataComponent.DataModel;
using AppneuronUnity.Core.UnityComponents.AdvDataComponent.UnityManager;

    /// <summary>
    /// Defines the <see cref="AdvEventUnityManager" />.
    /// </summary>
    internal class AdvEventUnityManager : IAdvEventUnityManager
    {
        /// <summary>
        /// Defines the _cryptoServices.
        /// </summary>
        private readonly ICryptoServices _cryptoServices;

        /// <summary>
        /// Defines the _kafkaMessageBroker.
        /// </summary>
        private readonly IMessageBrokerService _kafkaMessageBroker;

        /// <summary>
        /// Defines the _advEventDal.
        /// </summary>
        private readonly IAdvEventDal _advEventDal;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvEventUnityManager"/> class.
        /// </summary>
        /// <param name="cryptoServices">The cryptoServices<see cref="ICryptoServices"/>.</param>
        /// <param name="kafkaMessageBroker">The kafkaMessageBroker<see cref="IMessageBrokerService"/>.</param>
        /// <param name="advEventDal">The advEventDal<see cref="IAdvEventDal"/>.</param>
        public AdvEventUnityManager(ICryptoServices cryptoServices,
            IMessageBrokerService kafkaMessageBroker,
            IAdvEventDal advEventDal)
        {
            _cryptoServices = cryptoServices;
            _kafkaMessageBroker = kafkaMessageBroker;
            _advEventDal = advEventDal;
        }

        /// <summary>
        /// The CheckAdvFileAndSendData.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CheckAdvFileAndSendData()
        {
            List<string> FolderNameList = ComponentsConfigs.GetSavedDataFilesNames(ComponentsConfigs
                                                                                          .SaveTypePath
                                                                                          .AdvEventDataModel);
            foreach (var fileName in FolderNameList)
            {
                var dataModel = await _advEventDal.SelectAsync(ComponentsConfigs.AdvEventDataPath + fileName);

                var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                if (result.Success)
                {
                    await _advEventDal.DeleteAsync(ComponentsConfigs.AdvEventDataPath + fileName);
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
            AdvEventDataModel advEventDataModel = new AdvEventDataModel
            {
                ClientId = clientId,
                ProjectId = ChurnBlockerSingletonConfigs.Instance.GetProjectID(),
                CustomerId = ChurnBlockerSingletonConfigs.Instance.GetCustomerID(),
                LevelName = levelName,
                LevelIndex = levelIndex,
                AdvType = Tag,
                InMinutes = GameSecond,
                TrigerdTime = DateTime.Now
            };

            var result = await _kafkaMessageBroker.SendMessageAsync(advEventDataModel);
            if (result.Success)
            {
                return;
            }

            string fileName = _cryptoServices.GenerateStringName(6);
            string filepath = ComponentsConfigs.AdvEventDataPath + fileName;

            await _advEventDal.InsertAsync(filepath, advEventDataModel);
        }
    }
}
