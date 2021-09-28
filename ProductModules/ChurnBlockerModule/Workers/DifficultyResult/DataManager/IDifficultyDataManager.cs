using AppneuronUnity.ProductModules.ChurnBlockerModule.WeboscketWorkers.DifficultyResult;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Workers.DifficultyResult.DataManager
{
    interface IDifficultyDataManager
    {
        Task<DifficultyInternalModel> GetDifficultyFromFile();
        Task SetDifficultytoFile(DifficultyInternalModel difficultyInternalModel);
    }
}
