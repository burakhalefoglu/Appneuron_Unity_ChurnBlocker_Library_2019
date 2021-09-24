namespace AppneuronUnity.Core.UnityComponents.ScreenDataComponent.UnityManager
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
using AppneuronUnity.Core.UnityComponents.ScreenDataComponent.ModelData;

    /// <summary>
    /// Defines the <see cref="IClickDataUnityManager" />.
    /// </summary>
    internal interface IClickDataUnityManager
    {
        /// <summary>
        /// The DetectDetaildRawTouchInformation.
        /// </summary>
        /// <returns>The <see cref="List{ClickDataDto}"/>.</returns>
        List<ClickDataDto> DetectDetaildRawTouchInformation();

        /// <summary>
        /// The SaveLocalData.
        /// </summary>
        /// <param name="clickDataModel">The clickDataModel<see cref="ClickDataModel"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SaveLocalData(ClickDataModel clickDataModel);

        /// <summary>
        /// The CheckAdvFileAndSendData.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CheckAdvFileAndSendData();

        /// <summary>
        /// The ClickCalculater.
        /// </summary>
        /// <param name="resultClickDtoList">The resultClickDtoList<see cref="List{ClickDataDto}"/>.</param>
        /// <param name="clickDataModelList">The clickDataModelList<see cref="List{ClickDataModel}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task ClickCalculater(List<ClickDataDto> resultClickDtoList,
            List<ClickDataModel> clickDataModelList);
    }
}
