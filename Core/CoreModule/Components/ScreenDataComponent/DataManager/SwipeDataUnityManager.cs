using AppneuronUnity.Core.BaseServices.WebsocketAdapter.WebsocketSharp;

namespace AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.DataManager
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
    using AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact;
    using AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.DataAccess;
    using Zenject;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager;
    using AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.DataModel;

    internal class SwipeDataUnityManager : ISwipeDataUnityManager
    {
        private Touch theTouch;

        private Vector2 touchStartPosition, touchEndPosition;

        private readonly IDataCreationClient _dataCreationClient;

        private readonly ISwipeScreenDataDal _swipeScreenDataDal;

        private readonly ICryptoServices _cryptoServices;

        private readonly IClientIdUnityManager _clientIdUnityManager;

        [Inject]
        private readonly CoreHelper coreHelper;

        ScreenSwipeDataModel swipeDataModel;

        public SwipeDataUnityManager(IDataCreationClient dataCreationClient,
            ISwipeScreenDataDal swipeScreenDataDal,
            ICryptoServices cryptoServices,
            IClientIdUnityManager clientIdUnityManager)
        {
            _dataCreationClient = dataCreationClient;
            _swipeScreenDataDal = swipeScreenDataDal;
            _cryptoServices = cryptoServices;
            _clientIdUnityManager = clientIdUnityManager;
            swipeDataModel = new ScreenSwipeDataModel();
        }

        public async Task SaveLocalData(ScreenSwipeDataModel swipeDataModel)
        {
            string fileName = _cryptoServices.GenerateStringName(6);
            await _swipeScreenDataDal.InsertAsync(fileName, swipeDataModel);
        }

        public async Task CheckSwipeDataFileAndSendData()
        {
            List<string> FolderNameList = coreHelper.GetSavedDataFilesNames<ScreenSwipeDataModel>();
            if (FolderNameList.Count == 0)
                return;
            foreach (var fileName in FolderNameList)
            {
                var dataModel = await _swipeScreenDataDal.SelectAsync(fileName);


                await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
                coreHelper.GetProjectInfo().ProjectId,
                dataModel, async (result) =>
                {
                    if (result)
                    {
                        await _swipeScreenDataDal.DeleteAsync(fileName);

                    }
                });
            }
        }

        public async Task CalculateSwipeDirection(SwipeModelDto swipeModelDto,
            string scenemName,
            int sceneIndex)
        {
            swipeDataModel.ClientId = await _clientIdUnityManager.GetPlayerIdAsync();
            swipeDataModel.ProjectId = coreHelper.GetProjectInfo().ProjectId;
            swipeDataModel.CustomerId = coreHelper.GetProjectInfo().CustomerId;
            swipeDataModel.LevelName = scenemName;
            swipeDataModel.LevelIndex = sceneIndex;

            if (swipeModelDto == null && swipeDataModel.SwipeDirection != 0)
            {

                await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
                coreHelper.GetProjectInfo().ProjectId,
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

                await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
                coreHelper.GetProjectInfo().ProjectId,
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

                await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
                coreHelper.GetProjectInfo().ProjectId,
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
                        SwipeDirection = y > 0 ? 3 : 4
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
