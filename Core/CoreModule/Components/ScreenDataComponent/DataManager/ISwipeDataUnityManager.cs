namespace AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.UnityManager
{
    using System.Threading.Tasks;
using AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.ModelData;

    /// <summary>
    /// Defines the <see cref="ISwipeDataUnityManager" />.
    /// </summary>
    internal interface ISwipeDataUnityManager
    {
        /// <summary>
        /// The SaveLocalData.
        /// </summary>
        /// <param name="swipeDataModel">The swipeDataModel<see cref="SwipeDataModel"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SaveLocalData(SwipeDataModel swipeDataModel);

        /// <summary>
        /// The CheckAdvFileAndSendData.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CheckAdvFileAndSendData();

        /// <summary>
        /// The CalculateSwipeDirection.
        /// </summary>
        /// <param name="swipeModelDto">The swipeModelDto<see cref="SwipeModelDto"/>.</param>
        /// <param name="swipeDataModel">The swipeDataModel<see cref="SwipeDataModel"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CalculateSwipeDirection(SwipeModelDto swipeModelDto,
            SwipeDataModel swipeDataModel);

        /// <summary>
        /// The ListenTouchDirection.
        /// </summary>
        /// <returns>The <see cref="SwipeModelDto"/>.</returns>
        SwipeModelDto ListenTouchDirection();
    }
}
