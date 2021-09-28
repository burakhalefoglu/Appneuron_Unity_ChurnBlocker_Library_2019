namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataAccess
{
using AppneuronUnity.Core.ObjectBases.DataStorageHelper.Abstract;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.WeboscketWorkers.DifficultyResult;

    /// <summary>
    /// Defines the <see cref="IDifficultyDal" />.
    /// </summary>
    internal interface IDifficultyDal : IRepositoryService<DifficultyInternalModel>
    {
    }
} 
