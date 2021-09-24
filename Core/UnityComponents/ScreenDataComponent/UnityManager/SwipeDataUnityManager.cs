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
    /// Defines the <see cref="SwipeDataUnityManager" />.
    /// </summary>
    internal class SwipeDataUnityManager : ISwipeDataUnityManager
    {
        /// <summary>
        /// Defines the theTouch.
        /// </summary>
        private Touch theTouch;

        /// <summary>
        /// Defines the touchStartPosition, touchEndPosition...
        /// </summary>
        private Vector2 touchStartPosition, touchEndPosition;

        /// <summary>
        /// Defines the _kafkaMessageBroker.
        /// </summary>
        private readonly IMessageBrokerService _kafkaMessageBroker;

        /// <summary>
        /// Defines the _swipeScreenDataDal.
        /// </summary>
        private readonly ISwipeScreenDataDal _swipeScreenDataDal;

        /// <summary>
        /// Defines the _cryptoServices.
        /// </summary>
        private readonly ICryptoServices _cryptoServices;

        /// <summary>
        /// Defines the _clientIdUnityManager.
        /// </summary>
        private readonly IClientIdUnityManager _clientIdUnityManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwipeDataUnityManager"/> class.
        /// </summary>
        /// <param name="kafkaMessageBroker">The kafkaMessageBroker<see cref="IMessageBrokerService"/>.</param>
        /// <param name="swipeScreenDataDal">The swipeScreenDataDal<see cref="ISwipeScreenDataDal"/>.</param>
        /// <param name="cryptoServices">The cryptoServices<see cref="ICryptoServices"/>.</param>
        public SwipeDataUnityManager(IMessageBrokerService kafkaMessageBroker,
            ISwipeScreenDataDal swipeScreenDataDal,
            ICryptoServices cryptoServices,
            IClientIdUnityManager clientIdUnityManager)
        {
            _kafkaMessageBroker = kafkaMessageBroker;
            _swipeScreenDataDal = swipeScreenDataDal;
            _cryptoServices = cryptoServices;
            _clientIdUnityManager = clientIdUnityManager;
        }

        /// <summary>
        /// The SaveLocalData.
        /// </summary>
        /// <param name="swipeDataModel">The swipeDataModel<see cref="SwipeDataModel"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SaveLocalData(SwipeDataModel swipeDataModel)
        {
            string fileName = _cryptoServices.GenerateStringName(6);
            string filepath = ComponentsConfigs.SwipeDataModelPath + fileName;
            await _swipeScreenDataDal.InsertAsync(filepath, swipeDataModel);
        }

        /// <summary>
        /// The CheckAdvFileAndSendData.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CheckAdvFileAndSendData()
        {
            List<string> FolderNameList = ComponentsConfigs.GetSavedDataFilesNames(
                ComponentsConfigs.SaveTypePath.SwipeDataModel);
            foreach (var fileName in FolderNameList)
            {
                var dataModel = await _swipeScreenDataDal.SelectAsync(
                    ComponentsConfigs.SwipeDataModelPath + fileName);

                var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                if (result.Success)
                {
                    await _swipeScreenDataDal.DeleteAsync(ComponentsConfigs.SwipeDataModelPath + fileName);
                }
            }
        }

        /// <summary>
        /// The CalculateSwipeDirection.
        /// </summary>
        /// <param name="swipeModelDto">The swipeModelDto<see cref="SwipeModelDto"/>.</param>
        /// <param name="swipeDataModel">The swipeDataModel<see cref="SwipeDataModel"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CalculateSwipeDirection(SwipeModelDto swipeModelDto,
            SwipeDataModel swipeDataModel)
        {
            swipeDataModel.ClientId = _clientIdUnityManager.GetPlayerID();
            swipeDataModel.ProjectId = ChurnBlockerSingletonConfigs.Instance.GetProjectID();
            swipeDataModel.CustomerId = ChurnBlockerSingletonConfigs.Instance.GetCustomerID();
            swipeDataModel.LevelName = SceneManager.GetActiveScene().name;
            swipeDataModel.LevelIndex = SceneManager.GetActiveScene().buildIndex;

            if (swipeModelDto == null && swipeDataModel.SwipeDirection != 0)
            {
                var result = await _kafkaMessageBroker.SendMessageAsync(swipeDataModel);
                if (!result.Success)
                {
                    await SaveLocalData(swipeDataModel);
                }
                swipeDataModel.SwipeDirection = 0;
                return;
            }

            if (swipeModelDto.SwipeDirection == 0 && swipeDataModel.SwipeDirection == 0)
            {
                swipeDataModel.StartLocX = swipeModelDto.LocX;
                swipeDataModel.StartLocY = swipeModelDto.LocY;
                swipeDataModel.CreatedAt = swipeModelDto.CreatedAt;
                return;
            }

            if (swipeModelDto.SwipeDirection != 0 && swipeDataModel.SwipeDirection == 0)
            {
                swipeDataModel.FinishLocX = swipeModelDto.LocX;
                swipeDataModel.FinishLocY = swipeModelDto.LocY;
                swipeDataModel.SwipeDirection = swipeModelDto.SwipeDirection;
                return;
            }

            if (swipeModelDto.SwipeDirection == 0 && swipeDataModel.SwipeDirection != 0)
            {
                var result = await _kafkaMessageBroker.SendMessageAsync(swipeDataModel);
                if (!result.Success)
                {
                    await SaveLocalData(swipeDataModel);
                }
                swipeDataModel.StartLocX = swipeModelDto.LocX;
                swipeDataModel.StartLocY = swipeModelDto.LocY;
                swipeDataModel.CreatedAt = swipeModelDto.CreatedAt;
                swipeDataModel.SwipeDirection = 0;
                return;
            }

            if (swipeModelDto.SwipeDirection == swipeDataModel.SwipeDirection)
            {
                swipeDataModel.FinishLocX = swipeModelDto.LocX;
                swipeDataModel.FinishLocX = swipeModelDto.LocY;
                return;
            }

            if (swipeModelDto.SwipeDirection != swipeDataModel.SwipeDirection)
            {
                var result = await _kafkaMessageBroker.SendMessageAsync(swipeDataModel);
                if (!result.Success)
                {
                    await SaveLocalData(swipeDataModel);
                }
                swipeDataModel.StartLocX = swipeModelDto.LocX;
                swipeDataModel.StartLocY = swipeModelDto.LocY;
                swipeDataModel.CreatedAt = swipeModelDto.CreatedAt;
                swipeDataModel.SwipeDirection = swipeModelDto.SwipeDirection;

                return;
            }
        }

        /// <summary>
        /// The ListenTouchDirection.
        /// </summary>
        /// <returns>The <see cref="SwipeModelDto"/>.</returns>
        public SwipeModelDto ListenTouchDirection()
        {
            if (Input.touchCount > 0)
            {
                theTouch = Input.GetTouch(0);

                if (theTouch.phase == TouchPhase.Began)
                {
                    touchStartPosition = theTouch.position;
                }
                else if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended)
                {
                    touchEndPosition = theTouch.position;

                    float x = touchEndPosition.x - touchStartPosition.x;
                    float y = touchEndPosition.y - touchStartPosition.y;

                    if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
                    {
                        return new SwipeModelDto
                        {
                            LocX = FormatFingerPosition(theTouch.position).x,
                            LocY = FormatFingerPosition(theTouch.position).y,
                            SwipeDirection = 0
                        };
                    }
                    else if (Mathf.Abs(x) > Mathf.Abs(y))
                    {
                        return new SwipeModelDto
                        {
                            LocX = FormatFingerPosition(theTouch.position).x,
                            LocY = FormatFingerPosition(theTouch.position).y,
                            SwipeDirection = x > 0 ? 1 : 2
                        };
                    }

                    return new SwipeModelDto
                    {
                        LocX = FormatFingerPosition(theTouch.position).x,
                        LocY = FormatFingerPosition(theTouch.position).y,
                        SwipeDirection = y > 0 ? 2 : 3
                    };
                }
            }
            return null;
        }

        /// <summary>
        /// The FormatFingerPosition.
        /// </summary>
        /// <param name="position">The position<see cref="Vector2"/>.</param>
        /// <returns>The <see cref="Vector2"/>.</returns>
        private Vector2 FormatFingerPosition(Vector2 position)
        {
            //Screen.width   x
            //Screen.height  y
            return new Vector2(position.x / Screen.width, position.y / Screen.height);
        }
    }
}
