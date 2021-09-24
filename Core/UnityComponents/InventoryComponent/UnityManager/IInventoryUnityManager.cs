namespace AppneuronUnity.Core.UnityComponents.InventoryComponent.UnityManager
{
    using System.Threading.Tasks;
using AppneuronUnity.Core.UnityComponents.InventoryComponent.DataModel;

    /// <summary>
    /// Defines the <see cref="IInventoryUnityManager" />.
    /// </summary>
    internal interface IInventoryUnityManager
    {
        /// <summary>
        /// The SendData.
        /// </summary>
        /// <param name="ınventoryDataModel">The ınventoryDataModel<see cref="InventoryDataModel"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SendData(InventoryDataModel ınventoryDataModel);

        /// <summary>
        /// The CheckFileExistAndSend.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CheckFileExistAndSend();
    }
}
