namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.UnityManager
{
    using AppneuronUnity.Core.CoreServices.RestClientServices.Abstract;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.Helper;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Workers.DifficultyResult.Models;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="DifficultyManager" />.
    /// </summary>
    internal class DifficultyManager : IDifficultyManager
    {
        /// <summary>
        /// Defines the _restClientServices.
        /// </summary>
        private readonly IRestClientServices _restClientServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="DifficultyManager"/> class.
        /// </summary>
        /// <param name="restClientServices">The restClientServices<see cref="IRestClientServices"/>.</param>
        public DifficultyManager(IRestClientServices restClientServices)
        {
            _restClientServices = restClientServices;
        }

        /// <summary>
        /// The AskDifficulty.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task AskDifficulty()
        {
            var productId = 0/* AppneuronProductList.ChurnBlocker*/;
            var url = Appsettings.ClientWebApiLink + Appsettings.MlResultRequestName + "?productId=" + productId;

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
