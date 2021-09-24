namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataAccess.BinarySaving
{
    using AppneuronUnity.Core.Helpers.DataStorageHelper.Concrete.BinaryType;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Workers.DifficultyResult.Models;

    /// <summary>
    /// Defines the <see cref="BSDifficultyLevelDal" />.
    /// </summary>
    internal class BSDifficultyLevelDal : BinaryTypeRepositoryBase<DifficultyModel>, IDifficultyLevelDal
    {
    }
}
