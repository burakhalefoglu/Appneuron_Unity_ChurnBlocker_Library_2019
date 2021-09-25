namespace AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.UnityService
{
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using Appneuron.Zenject;
using AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.ModelData;
using AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.UnityManager;

    /// <summary>
    /// Defines the <see cref="ClickDataUnityService" />.
    /// </summary>
    public class ClickDataUnityService : MonoBehaviour
    {
        [Inject]
        internal IClickDataUnityManager _clickDataUnityManager;

        /// <summary>
        /// Defines the clickDataModelList.
        /// </summary>
        internal List<ClickDataModel> clickDataModelList;

        /// <summary>
        /// The Start.
        /// </summary>
        private void Start()
        {

            clickDataModelList = new List<ClickDataModel>();
        }

        /// <summary>
        /// The Update.
        /// </summary>
        private async void Update()
        {
            var resultClickDtoList = _clickDataUnityManager.DetectDetaildRawTouchInformation();
            if (resultClickDtoList != null)
            {
                await _clickDataUnityManager.ClickCalculater(resultClickDtoList,
                    clickDataModelList);
            }
        }
    }
}
