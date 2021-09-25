namespace AppneuronUnity.ProductModules.ChurnBlockerModule.WeboscketWorkers.DifficultyResult.UnityController
{
    using UnityEngine;
using AppneuronUnity.ProductModules.ChurnBlockerModule.WeboscketWorkers.DifficultyResult.Clients;

    public class DifficultyUnityController : MonoBehaviour
    {

        private async void Start()
        {
            await DifficultyClient.ListenServerManager();
        }
    }
}
