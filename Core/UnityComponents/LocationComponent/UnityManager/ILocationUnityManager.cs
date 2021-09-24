namespace AppneuronUnity.Core.UnityComponents.LocationComponent.UnityManager
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ILocationUnityManager" />.
    /// </summary>
    internal interface ILocationUnityManager
    {
        /// <summary>
        /// The SendMessage.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SendMessage();
    }
}
