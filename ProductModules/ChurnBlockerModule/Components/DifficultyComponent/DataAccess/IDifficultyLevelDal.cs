namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataAccess
{
    using AppneuronUnity.Core.Helpers.DataStorageHelper.Abstract;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Workers.DifficultyResult.Models;

    /// <summary>
    /// Defines the <see cref="IDifficultyLevelDal" />.
    /// </summary>
    internal interface IDifficultyLevelDal : IRepositoryService<DifficultyModel>
    {
    }
}
