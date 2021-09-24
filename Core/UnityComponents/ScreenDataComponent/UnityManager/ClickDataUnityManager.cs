namespace AppneuronUnity.Core.UnityComponents.ScreenDataComponent.UnityManager
{
    using AppneuronUnity.Core.CoreServices.CryptoServices.Absrtact;
    using AppneuronUnity.Core.CoreServices.MessageBrokers;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.SceneManagement;
using AppneuronUnity.Core.UnityComponents.ScreenDataComponent.DataAccess;
using AppneuronUnity.Core.UnityComponents.ScreenDataComponent.ModelData;
using AppneuronUnity.Core.UnityComponents.ScreenDataComponent.UnityManager;
using AppneuronUnity.Core.UnityWorkers.ClientIdWorker.UnityManager;

    /// <summary>
    /// Defines the <see cref="ClickDataUnityManager" />.
    /// </summary>
    internal class ClickDataUnityManager : IClickDataUnityManager
    {
        /// <summary>
        /// Defines the _kafkaMessageBroker.
        /// </summary>
        private readonly IMessageBrokerService _kafkaMessageBroker;

        /// <summary>
        /// Defines the _clickDataDal.
        /// </summary>
        private readonly IClickDataDal _clickDataDal;

        /// <summary>
        /// Defines the _cryptoServices.
        /// </summary>
        private readonly ICryptoServices _cryptoServices;


        /// <summary>
        /// Defines the _clientIdUnityManager.
        /// </summary>
        private readonly IClientIdUnityManager _clientIdUnityManager;

        /// <summary>
        /// Defines the theTouch.
        /// </summary>
        private Touch theTouch;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClickDataUnityManager"/> class.
        /// </summary>
        /// <param name="kafkaMessageBroker">The kafkaMessageBroker<see cref="IMessageBrokerService"/>.</param>
        /// <param name="clickDataDal">The clickDataDal<see cref="IClickDataDal"/>.</param>
        /// <param name="cryptoServices">The cryptoServices<see cref="ICryptoServices"/>.</param>
        public ClickDataUnityManager(IMessageBrokerService kafkaMessageBroker,
            IClickDataDal clickDataDal,
            ICryptoServices cryptoServices,
            IClientIdUnityManager clientIdUnityManager)
        {
            _kafkaMessageBroker = kafkaMessageBroker;
            _clickDataDal = clickDataDal;
            _cryptoServices = cryptoServices;
            _clientIdUnityManager = clientIdUnityManager;
        }

        /// <summary>
        /// The DetectDetaildRawTouchInformation.
        /// </summary>
        /// <returns>The <see cref="List{ClickDataDto}"/>.</returns>
        public List<ClickDataDto> DetectDetaildRawTouchInformation()
        {
            if (Input.touchCount > 0)
            {
                var clickDataDto = new List<ClickDataDto>();

                for (int i = 0; i < Input.touchCount; i++)
                {
                    theTouch = Input.GetTouch(i);
                    clickDataDto.Add(new ClickDataDto
                    {
                        LocX = FormatFingerPosition(theTouch.position).x,
                        LocY = FormatFingerPosition(theTouch.position).y,
                        FingerID = theTouch.fingerId,
                        TabCount = theTouch.tapCount
                    });
                }
                return clickDataDto;
            }
            return null;
        }

        /// <summary>
        /// The FormatFingerPosition.
        /// </summary>
        /// <param name="position">The position<see cref="Vector2"/>.</param>
        /// <returns>The <see cref="Vector2"/>.</returns>
        private static Vector2 FormatFingerPosition(Vector2 position)
        {
            //Screen.width   x
            //Screen.height  y
            return new Vector2(position.x / Screen.width, position.y / Screen.height);
        }

        /// <summary>
        /// The SaveLocalData.
        /// </summary>
        /// <param name="clickDataModel">The clickDataModel<see cref="ClickDataModel"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SaveLocalData(ClickDataModel clickDataModel)
        {
            string fileName = _cryptoServices.GenerateStringName(6);
            string filepath = ComponentsConfigs.ClickDataModelPath + fileName;
            await _clickDataDal.InsertAsync(filepath, clickDataModel);
        }

        /// <summary>
        /// The CheckAdvFileAndSendData.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CheckAdvFileAndSendData()
        {
            List<string> FolderNameList = ComponentsConfigs.GetSavedDataFilesNames(
                ComponentsConfigs.SaveTypePath.ClickDataModel);
            foreach (var fileName in FolderNameList)
            {
                var dataModel = await _clickDataDal.SelectAsync(ComponentsConfigs.ClickDataModelPath + fileName);

                var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                if (result.Success)
                {
                    await _clickDataDal.DeleteAsync(ComponentsConfigs.ClickDataModelPath + fileName);
                }
            }
        }

        /// <summary>
        /// The ClickCalculater.
        /// </summary>
        /// <param name="resultClickDtoList">The resultClickDtoList<see cref="List{ClickDataDto}"/>.</param>
        /// <param name="clickDataModelList">The clickDataModelList<see cref="List{ClickDataModel}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task ClickCalculater(List<ClickDataDto> resultClickDtoList,
            List<ClickDataModel> clickDataModelList)
        {
            await Task.Run(() =>
            {
                resultClickDtoList.ForEach(async r =>
                {
                    
                    var clickDataModel = clickDataModelList.Find(c => c.FingerID == r.FingerID);
                    if (clickDataModel != null)
                    {
                        if (clickDataModel.TabCount < r.TabCount)
                        {
                            clickDataModel.TabCount = r.TabCount;
                        }
                        else
                        {
                            clickDataModel.FinishLocX = r.LocX;
                            clickDataModel.FinishLocY = r.LocY;
                            clickDataModel.CreatedAt = r.CreatedAt;

                            clickDataModel.ClientId = _clientIdUnityManager.GetPlayerID();
                            clickDataModel.ProjectId = ChurnBlockerSingletonConfigs.Instance.GetProjectID();
                            clickDataModel.CustomerId = ChurnBlockerSingletonConfigs.Instance.GetCustomerID();
                            clickDataModel.LevelName = SceneManager.GetActiveScene().name;
                            clickDataModel.LevelIndex = SceneManager.GetActiveScene().buildIndex;
                            
                            var result = await _kafkaMessageBroker.SendMessageAsync(clickDataModel);
                            if (result.Success)
                            {
                                clickDataModelList.Remove(clickDataModel);
                            }
                            else
                            {
                                await SaveLocalData(clickDataModel);
                            }
                        }
                    }
                    else
                    {
                        clickDataModelList.Add(new ClickDataModel
                        {
                            TabCount = r.TabCount,
                            FingerID = r.FingerID,
                            StartLocX = r.LocX,
                            StartLocY = r.LocY
                        });
                    }
                });
            });
        }
    }
}
