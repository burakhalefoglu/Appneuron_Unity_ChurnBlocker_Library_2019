namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataAccess
{
using AppneuronUnity.Core.ObjectBases.DataStorageHelper.Abstract;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataModel;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.WeboscketWorkers.DifficultyResult.Models;

    /// <summary>
    /// Defines the <see cref="IDifficultyDal" />.
    /// </summary>
    internal interface IManuelFlowDal : IRepositoryService<ManuelFlowModel>
    {
    }
} 
