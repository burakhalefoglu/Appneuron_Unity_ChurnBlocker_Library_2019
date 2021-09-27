﻿namespace AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.UnityManager
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

    internal class ClickDataUnityManager : IClickDataUnityManager
    {

        private readonly IDataCreationClient _dataCreationClient;

        private readonly IClickDataDal _clickDataDal;

        private readonly ICryptoServices _cryptoServices;

        private readonly IClientIdUnityManager _clientIdUnityManager;

        private Touch theTouch;

        public ClickDataUnityManager(IDataCreationClient dataCreationClient,
            IClickDataDal clickDataDal,
            ICryptoServices cryptoServices,
            IClientIdUnityManager clientIdUnityManager)
        {
            _dataCreationClient = dataCreationClient;
            _clickDataDal = clickDataDal;
            _cryptoServices = cryptoServices;
            _clientIdUnityManager = clientIdUnityManager;
        }

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

        private static Vector2 FormatFingerPosition(Vector2 position)
        {
            //Screen.width   x
            //Screen.height  y
            return new Vector2(position.x / Screen.width, position.y / Screen.height);
        }

        public async Task SaveLocalData(ClickDataModel clickDataModel)
        {
            string fileName = _cryptoServices.GenerateStringName(6);
            string filepath = ComponentsConfigs.ClickDataModelPath + fileName;
            await _clickDataDal.InsertAsync(filepath, clickDataModel);
        }

        public async Task CheckAdvFileAndSendData()
        {
            List<string> FolderNameList = ComponentsConfigs.GetSavedDataFilesNames(
                ComponentsConfigs.SaveTypePath.ClickDataModel);
            foreach (var fileName in FolderNameList)
            {
                var dataModel = await _clickDataDal.SelectAsync(ComponentsConfigs.ClickDataModelPath + fileName);

                await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
                dataModel, async (result) =>
                {
                    if (result)
                    {
                        await _clickDataDal.DeleteAsync(ComponentsConfigs.ClickDataModelPath + fileName);

                    }
                });
            }
        }

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

                            await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
                            clickDataModel, async (result) =>
                            {
                                if (result)
                                {
                                    clickDataModelList.Remove(clickDataModel);
                                }
                                else
                                {
                                    await SaveLocalData(clickDataModel);
                                }
                            });
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