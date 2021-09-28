namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataAccess.BinarySaving
{
using AppneuronUnity.Core.ObjectBases.DataStorageHelper.Concrete.BinaryType;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataModel;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.WeboscketWorkers.DifficultyResult.Models;

    /// <summary>
    /// Defines the <see cref="BSDifficultyDal" />.
    /// </summary>
    internal class BSManuelFlowDal : BinaryTypeRepositoryBase<ManuelFlowModel>, IManuelFlowDal
    {
    }
}
