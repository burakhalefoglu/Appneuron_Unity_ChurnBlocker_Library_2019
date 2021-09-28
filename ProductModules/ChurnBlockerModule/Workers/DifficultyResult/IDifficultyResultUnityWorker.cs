using AppneuronUnity.ProductModules.ChurnBlockerModule.WeboscketWorkers.DifficultyResult.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Workers.DifficultyResult
{
    internal interface IDifficultyResultUnityWorker
    {
        Task StartListen();
        Task GetDifficultyFromServer();
    }
}
