﻿using System;
using UnityEngine;
using Zenject;
using System.Threading.Tasks;
using AppneuronUnity.Core.AuthModule.AuthComponent.DataManager;
using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager;

namespace AppneuronUnity.Core.AuthModule
{
    public class AuthModule : MonoBehaviour
    {
        [Inject]
        private IAuthUnityManager _authUnityManager { get; set; }

        [Inject]
        private IClientIdUnityManager _clientIdUnityManager { get; set; }

        private async void Start()
        {
            await LateStart(3);
        }
        private async Task LateStart(float waitTime)
        {
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            await _authUnityManager.Login();
        }
    }
}
