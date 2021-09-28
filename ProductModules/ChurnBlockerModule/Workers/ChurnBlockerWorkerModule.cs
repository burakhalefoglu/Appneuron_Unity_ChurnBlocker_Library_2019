namespace AppneuronUnity.ProductModules.ChurnBlockerModule.WeboscketWorkers.DifficultyResult.UnityController
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Workers.DifficultyResult;
    using UnityEngine;
    using Zenject;

    public class ChurnBlockerWorkerModule : MonoBehaviour
    {
        [Inject]
        IDifficultyResultUnityWorker difficultyResultUnityWorker;

        private async void Start()
        {
            await difficultyResultUnityWorker.GetDifficultyFromServer();
            await difficultyResultUnityWorker.StartListen();
        }
    }
}
