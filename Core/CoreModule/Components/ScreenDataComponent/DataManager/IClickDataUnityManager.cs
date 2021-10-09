namespace AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.DataManager
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
using AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.DataModel;

    /// <summary>
    /// Defines the <see cref="IClickDataUnityManager" />.
    /// </summary>
    internal interface IClickDataUnityManager
    {
        List<ClickDataDto> DetectDetaildRawTouchInformation();

        Task SaveLocalData(ScreenClickDataModel clickDataModel);

        Task CheckClickDataFileAndSendData();

        Task ClickCalculater(List<ClickDataDto> resultClickDtoList);
    }
}
