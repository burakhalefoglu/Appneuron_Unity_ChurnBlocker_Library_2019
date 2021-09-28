using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataAccess;
using AppneuronUnity.ProductModules.ChurnBlockerModule.WeboscketWorkers.DifficultyResult;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Workers.DifficultyResult.DataManager
{
    internal class DifficultyDataManager : IDifficultyDataManager
    {

        private readonly IDifficultyDal _difficultyDal;

        public DifficultyDataManager(IDifficultyDal difficultyDal)
        {
            _difficultyDal = difficultyDal;
        }

        public async Task<DifficultyInternalModel> GetDifficultyFromFile()
        {
            return await _difficultyDal.SelectAsync("DifficultyLevel");

        }

        public async Task SetDifficultytoFile(DifficultyInternalModel difficultyInternalModel)
        {

            await _difficultyDal.InsertAsync("DifficultyLevel", difficultyInternalModel);


        }
    }
}
