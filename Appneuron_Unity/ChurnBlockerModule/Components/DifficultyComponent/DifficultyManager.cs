using Appneuron.Models;
using Appneuron.Services;
using AppneuronUnity.ChurnBlockerModule.Components.DifficultyComponent.FlowbaseDifficulty;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using Ninject;
using System.Reflection;
using System.Threading.Tasks;

namespace AppneuronUnity.ChurnBlockerModule.Components.DifficultyComponent
{
    public class DifficultyManager
    {
        public async Task AskDifficulty()
        {
            using (var kernel = new StandardKernel())
            {

                kernel.Load(Assembly.GetExecutingAssembly());
                var _restClientServices = kernel.Get<IRestClientServices>();
                var productId = AppneuronProductList.ChurnBlocker;
                var url = WebApiConfigService.ClientWebApiLink + WebApiConfigService.MlResultRequestName + "?productId=" + productId;

                DifficultyModel difficultyModel = new DifficultyModel();
                var result = await _restClientServices.GetAsync<DifficultyModel>(url);

                if (result.Data == null)
                {
                    difficultyModel.CenterOfDifficultyLevel = 0;
                    difficultyModel.RangeCount = 2;

                }
                else
                {
                    difficultyModel = result.Data;
                }

                DifficultyHelper.AskDifficultyLevelFromServer(difficultyModel);
            }
        }
    }
}
