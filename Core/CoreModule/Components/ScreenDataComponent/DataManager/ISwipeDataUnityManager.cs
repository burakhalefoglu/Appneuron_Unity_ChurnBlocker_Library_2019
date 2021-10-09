namespace AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.DataManager
{
    using System.Threading.Tasks;
using AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.DataModel;

    /// <summary>
    /// Defines the <see cref="ISwipeDataUnityManager" />.
    /// </summary>
    internal interface ISwipeDataUnityManager
    {
        Task SaveLocalData(ScreenSwipeDataModel swipeDataModel);
        Task CheckSwipeDataFileAndSendData();
        Task CalculateSwipeDirection(SwipeModelDto swipeModelDto,
            string sceneName, int sceneIndex);
        SwipeModelDto ListenTouchDirection();
    }
}
