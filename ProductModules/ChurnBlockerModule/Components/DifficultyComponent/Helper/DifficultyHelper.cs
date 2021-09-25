namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.Helper
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataAccess;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System;
    using System.Threading.Tasks;
    using Appneuron.Zenject;
using AppneuronUnity.ProductModules.ChurnBlockerModule.WeboscketWorkers.DifficultyResult.Models;
using AppneuronUnity.ProductModules.ChurnBlockerModule.WeboscketWorkers.DifficultyResult;

    internal static class DifficultyHelper
    {
        private static readonly DifficultySingletonModel difficultySingletonModel = DifficultySingletonModel.Instance;

        [Inject]
        private static IDifficultyLevelDal _difficultyLevelDal;

        private static string fileName = "DifficultyLevelData.data";

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

        private static void SendManuelFlowResult()
        {

            //var result = _messageBrokerService.SendMessageAsync(new ManuelFlowModel
            //{
            //    DifficultyLevel = difficultySingletonModel.CurrentDifficultyLevel
            //});

        }

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

        private static int GetCurrentDifficulty()
        {
            Random random = new Random();
            return random.Next(difficultySingletonModel.MinOfDifficultyLevelRange,
                                                                       difficultySingletonModel.MaxOfDifficultyLevelRange);
        }

        private static double GetFlow()
        {
            //return 100 - CharInformation.CharFinishHealth * 100 / CharInformation.CharStarterHealth;
            return 0;
        }

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

        private static void CalculateMinDifficultyValue()
        {
            difficultySingletonModel.MinOfDifficultyLevelRange = difficultySingletonModel.CenterOfDifficultyLevel - difficultySingletonModel.RangeCount;
            if (difficultySingletonModel.MinOfDifficultyLevelRange < 1)
                difficultySingletonModel.MinOfDifficultyLevelRange = 1;
            else if (difficultySingletonModel.MinOfDifficultyLevelRange > 16)
                difficultySingletonModel.MinOfDifficultyLevelRange = 16;
        }

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
