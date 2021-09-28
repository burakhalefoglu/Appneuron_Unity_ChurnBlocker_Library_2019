namespace AppneuronUnity.Core.CoreModule.Components.AdvDataComponent.DataManager
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IAdvEventUnityManager" />.
    /// </summary>
    internal interface IAdvEventUnityManager
    {
        Task CheckAdvFileAndSendData();

        Task SendAdvEventData(string Tag,
            string levelName,
            int levelIndex,
            float GameSecond,
            string clientId);
    }
}
