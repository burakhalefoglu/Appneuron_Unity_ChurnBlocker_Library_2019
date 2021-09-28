namespace AppneuronUnity.Core.CoreModule.Components.HardwareIndormationComponent.DataManager
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IHardwareIndormationUnityManager" />.
    /// </summary>
    internal interface IHardwareIndormationUnityManager
    {
        /// <summary>
        /// The SendData.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SendData();
    }
}
