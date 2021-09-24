namespace AppneuronUnity.Core.UnityComponents.ScreenDataComponent.UnityService
{
    using System.Reflection;
    using UnityEngine;
using AppneuronUnity.Core.UnityComponents.ScreenDataComponent.ModelData;
using AppneuronUnity.Core.UnityComponents.ScreenDataComponent.UnityManager;
    using Appneuron.Zenject;

    /// <summary>
    /// Defines the <see cref="SwipeDataUnityService" />.
    /// </summary>
    public class SwipeDataUnityService : MonoBehaviour
    {
        /// <summary>
        /// Defines the swipeDataModel.
        /// </summary>
        internal SwipeDataModel swipeDataModel;

        [Inject]
        internal ISwipeDataUnityManager _swipeDataUnityManager;

        /// <summary>
        /// The Start.
        /// </summary>
        private void Start()
        {

            swipeDataModel = new SwipeDataModel();
            swipeDataModel.SwipeDirection = 0;
        }

        /// <summary>
        /// The Update.
        /// </summary>
        private async void Update()
        {
            var swipeModelDto = _swipeDataUnityManager.ListenTouchDirection();
            await _swipeDataUnityManager.CalculateSwipeDirection(swipeModelDto, swipeDataModel);
        }
    }
}
