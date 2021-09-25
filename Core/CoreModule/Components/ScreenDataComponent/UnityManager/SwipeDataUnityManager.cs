namespace AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.UnityManager
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.UnityManager;
    using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact;
using AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.DataAccess;
using AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.ModelData;
using AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.UnityManager;

    internal class SwipeDataUnityManager : ISwipeDataUnityManager
    {
        private Touch theTouch;

        private Vector2 touchStartPosition, touchEndPosition;

        private readonly IDataCreationClient _dataCreationClient;

        private readonly ISwipeScreenDataDal _swipeScreenDataDal;

        private readonly ICryptoServices _cryptoServices;

        private readonly IClientIdUnityManager _clientIdUnityManager;

        public SwipeDataUnityManager(IDataCreationClient dataCreationClient,
            ISwipeScreenDataDal swipeScreenDataDal,
            ICryptoServices cryptoServices,
            IClientIdUnityManager clientIdUnityManager)
        {
            _dataCreationClient = dataCreationClient;
            _swipeScreenDataDal = swipeScreenDataDal;
            _cryptoServices = cryptoServices;
            _clientIdUnityManager = clientIdUnityManager;
        }

        public async Task SaveLocalData(SwipeDataModel swipeDataModel)
        {
            string fileName = _cryptoServices.GenerateStringName(6);
            string filepath = ComponentsConfigs.SwipeDataModelPath + fileName;
            await _swipeScreenDataDal.InsertAsync(filepath, swipeDataModel);
        }

        public async Task CheckAdvFileAndSendData()
        {
            List<string> FolderNameList = ComponentsConfigs.GetSavedDataFilesNames(
                ComponentsConfigs.SaveTypePath.SwipeDataModel);
            foreach (var fileName in FolderNameList)
            {
                var dataModel = await _swipeScreenDataDal.SelectAsync(
                    ComponentsConfigs.SwipeDataModelPath + fileName);


                await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
                dataModel, async (result) =>
                {
                    if (result)
                    {
                        await _swipeScreenDataDal.DeleteAsync(ComponentsConfigs.SwipeDataModelPath + fileName);

                    }
                });
            }
        }

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

                await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
                swipeDataModel, async (result) =>
                {
                    if (!result)
                    {
                        await SaveLocalData(swipeDataModel);

                    }
                });
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

                await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
                swipeDataModel, async (result) =>
                {
                    if (!result)
                    {
                        await SaveLocalData(swipeDataModel);

                    }
                });
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

                await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
                swipeDataModel, async (result) =>
                {
                    if (!result)
                    {
                        await SaveLocalData(swipeDataModel);

                    }
                }); 
                swipeDataModel.StartLocX = swipeModelDto.LocX;
                swipeDataModel.StartLocY = swipeModelDto.LocY;
                swipeDataModel.CreatedAt = swipeModelDto.CreatedAt;
                swipeDataModel.SwipeDirection = swipeModelDto.SwipeDirection;

                return;
            }
        }

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

        private Vector2 FormatFingerPosition(Vector2 position)
        {
            //Screen.width   x
            //Screen.height  y
            return new Vector2(position.x / Screen.width, position.y / Screen.height);
        }
    }
}
