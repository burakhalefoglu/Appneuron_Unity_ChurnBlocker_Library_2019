namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataAccess
{
using AppneuronUnity.Core.ObjectBases.DataStorageHelper.Abstract;
using AppneuronUnity.ProductModules.ChurnBlockerModule.WeboscketWorkers.DifficultyResult.Models;

    /// <summary>
    /// Defines the <see cref="IDifficultyLevelDal" />.
    /// </summary>
    internal interface IDifficultyLevelDal : IRepositoryService<DifficultyModel>
    {
    }
}
