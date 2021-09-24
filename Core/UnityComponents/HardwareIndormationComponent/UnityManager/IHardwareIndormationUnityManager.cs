namespace AppneuronUnity.Core.UnityComponents.HardwareIndormationComponent.UnityManager
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
