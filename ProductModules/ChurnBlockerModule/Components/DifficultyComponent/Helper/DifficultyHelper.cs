namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.Helper
{
    using AppneuronUnity.Core.CoreServices.MessageBrokers;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataAccess;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataModel;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Workers.DifficultyResult.Models;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Workers.DifficultyResult;
    using Zenject;

    /// <summary>
    /// Defines the <see cref="DifficultyHelper" />.
    /// </summary>
    internal static class DifficultyHelper
    {
        /// <summary>
        /// Defines the difficultySingletonModel.
        /// </summary>
        private static readonly DifficultySingletonModel difficultySingletonModel = DifficultySingletonModel.Instance;

        [Inject]
        private static IDifficultyLevelDal _difficultyLevelDal;

        private static IMessageBrokerService _messageBrokerService;
        /// <summary>
        /// Defines the fileName.
        /// </summary>
        private static string fileName = "DifficultyLevelData.data";

     

        /// <summary>
        /// The AskDifficultyLevelFromServer.
        /// </summary>
        /// <param name="difficultyModel">The difficultyModel<see cref="DifficultyModel"/>.</param>
        public static async void AskDifficultyLevelFromServer(DifficultyModel difficultyModel)
        {
            if (difficultyModel.CenterOfDifficultyLevel == 0)
            {
                CalculateDifficultyManually();
                return;
            }

            difficultySingletonModel.CenterOfDifficultyLevel = difficultyModel.CenterOfDifficultyLevel;
            difficultySingletonModel.RangeCount = difficultyModel.RangeCount;
            await CalculateDifficulty();
        }

        /// <summary>
        /// The CalculateDifficultyManually.
        /// </summary>
        private static void CalculateDifficultyManually()
        {
            // TODO: oyun türlerine göre burası çeşitlenecek...
            int number = 1;
            switch (number)
            {
                case 1:
                    FlowBaseDifficulty();
                    SendManuelFlowResult();
                    break;

                case 2:
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// The SendManuelFlowResult.
        /// </summary>
        private static void SendManuelFlowResult()
        {

                var result = _messageBrokerService.SendMessageAsync(new ManuelFlowModel
                {
                    DifficultyLevel = difficultySingletonModel.CurrentDifficultyLevel
                });
            
        }

        /// <summary>
        /// The FlowBaseDifficulty.
        /// </summary>
        private static async void FlowBaseDifficulty()
        {
            var flow = GetFlow();
            Random random = new Random();
            var TopFlow = random.Next(75, 98);
            var increaseAmount = random.Next(1, 4);

            var difficultyLevel = await _difficultyLevelDal.SelectAsync(ComponentsConfigs.DifficultyModel + fileName);
            difficultySingletonModel.CurrentDifficultyLevel = difficultyLevel.CenterOfDifficultyLevel;
            difficultySingletonModel.RangeCount = 2;

            if (flow >= TopFlow)
            {
                difficultySingletonModel.CurrentDifficultyLevel = GetCurrentDifficulty();
                return;
            }
            if (flow < TopFlow && flow >= 20)
            {
                difficultySingletonModel.CenterOfDifficultyLevel += increaseAmount;
                CalculateMaxDifficultyValue();
                CalculateMinDifficultyValue();
                difficultySingletonModel.CurrentDifficultyLevel = GetCurrentDifficulty();
                return;
            }
            difficultySingletonModel.CenterOfDifficultyLevel -= increaseAmount;
            CalculateMaxDifficultyValue();
            CalculateMinDifficultyValue();
            difficultySingletonModel.CurrentDifficultyLevel = GetCurrentDifficulty();
        }

        /// <summary>
        /// The GetCurrentDifficulty.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        private static int GetCurrentDifficulty()
        {
            Random random = new Random();
            return random.Next(difficultySingletonModel.MinOfDifficultyLevelRange,
                                                                       difficultySingletonModel.MaxOfDifficultyLevelRange);
        }

        /// <summary>
        /// The GetFlow.
        /// </summary>
        /// <returns>The <see cref="double"/>.</returns>
        private static double GetFlow()
        {
            //return 100 - CharInformation.CharFinishHealth * 100 / CharInformation.CharStarterHealth;
            return 0;
        }

        /// <summary>
        /// The CalculateDifficulty.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        private static async Task CalculateDifficulty()
        {
            CalculateMaxDifficultyValue();
            CalculateMinDifficultyValue();

            int currentDifficultyLevel = difficultySingletonModel.CurrentDifficultyLevel;
            Random random = new Random();
            DifficultyModel model = new DifficultyModel();

            if (currentDifficultyLevel < difficultySingletonModel.MinOfDifficultyLevelRange)
            {
                difficultySingletonModel.CurrentDifficultyLevel += 2;
                model.CenterOfDifficultyLevel = difficultySingletonModel.CurrentDifficultyLevel;
                await _difficultyLevelDal.InsertAsync(ComponentsConfigs.DifficultyModel + fileName, model);
                return;
            }

            difficultySingletonModel.CurrentDifficultyLevel = random.Next(difficultySingletonModel.MinOfDifficultyLevelRange,
                                                                            difficultySingletonModel.MaxOfDifficultyLevelRange);

            model.CenterOfDifficultyLevel = difficultySingletonModel.CurrentDifficultyLevel;
            await _difficultyLevelDal.InsertAsync(ComponentsConfigs.DifficultyModel + fileName, model);
        }

        /// <summary>
        /// The CalculateMinDifficultyValue.
        /// </summary>
        private static void CalculateMinDifficultyValue()
        {
            difficultySingletonModel.MinOfDifficultyLevelRange = difficultySingletonModel.CenterOfDifficultyLevel - difficultySingletonModel.RangeCount;
            if (difficultySingletonModel.MinOfDifficultyLevelRange < 1)
                difficultySingletonModel.MinOfDifficultyLevelRange = 1;
            else if (difficultySingletonModel.MinOfDifficultyLevelRange > 16)
                difficultySingletonModel.MinOfDifficultyLevelRange = 16;
        }

        /// <summary>
        /// The CalculateMaxDifficultyValue.
        /// </summary>
        private static void CalculateMaxDifficultyValue()
        {
            difficultySingletonModel.MaxOfDifficultyLevelRange = difficultySingletonModel.CenterOfDifficultyLevel + difficultySingletonModel.RangeCount;
            if (difficultySingletonModel.MaxOfDifficultyLevelRange > 20)
                difficultySingletonModel.MaxOfDifficultyLevelRange = 20;
            else if (difficultySingletonModel.MaxOfDifficultyLevelRange < 3)
                difficultySingletonModel.MaxOfDifficultyLevelRange = 3;
        }
    }
}
