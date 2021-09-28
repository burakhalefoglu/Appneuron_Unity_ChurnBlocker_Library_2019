namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.Helper
{
    using System;
    using System.Threading.Tasks;
    using Zenject;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.WeboscketWorkers.DifficultyResult.Models;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.WeboscketWorkers.DifficultyResult;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataModel;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.ManuelFlowComponent.DataManager;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Workers.DifficultyResult.DataManager;

    internal class DifficultyHelper
    { 

        [Inject]
        private readonly DifficultyInternalModel difficultySingletonModel;

        [Inject]
        private IManuelFlowDataManager _manuelFlowDataManager;

        [Inject]
        private CharInfo charInfo;

        [Inject]
        private DifficultyInternalModel difficultyInternalModel;

        [Inject]
        private IDifficultyDataManager _difficultyDataManager;


        public async Task AskDifficultyLevelFromServer(DifficultyServerResultModel difficultyModel)
        {
            if (difficultyModel.CenterOfDifficultyLevel == 0)
            {
                await CalculateDifficultyManually();
                difficultyInternalModel.ServerResultModel = 0;
                return;
            }
            difficultyInternalModel.ServerResultModel = difficultyModel.CenterOfDifficultyLevel;
            difficultySingletonModel.CenterOfDifficultyLevel = difficultyModel.CenterOfDifficultyLevel;
            difficultySingletonModel.RangeCount = difficultyModel.RangeCount;
            await CalculateDifficulty();


        }

        public async Task CalculateDifficultyManually()
        {
            // TODO: oyun türlerine göre burası çeşitlenecek...
            int number = 1;
            switch (number)
            {
                case 1:
                    await FlowBaseDifficulty();
                    SendManuelFlowResult();
                    break;

                case 2:
                    break;

                default:
                    break;
            }
        }

        private void SendManuelFlowResult()
        {
            _manuelFlowDataManager.SendManuelFlowData(new ManuelFlowModel
            {
                DifficultyLevel = difficultySingletonModel.CurrentDifficultyLevel
            });
        }

        private async Task FlowBaseDifficulty()
        {
            var flow = 100 - charInfo.FinishHealth * 100 / charInfo.StartHealth;
            System.Random random = new System.Random();
            var TopFlow = random.Next(75, 98);
            var increaseAmount = random.Next(1, 4);

            var difficultyLevel = await _difficultyDataManager.GetDifficultyFromFile();
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


        private int GetCurrentDifficulty()
        {
            Random random = new Random();
            return random.Next(difficultySingletonModel.MinOfDifficultyLevelRange,
                                                                       difficultySingletonModel.MaxOfDifficultyLevelRange);
        }

        public  async Task CalculateDifficulty()
        {
            CalculateMaxDifficultyValue();
            CalculateMinDifficultyValue();

            int currentDifficultyLevel = difficultySingletonModel.CurrentDifficultyLevel;
            Random random = new Random();

            if (currentDifficultyLevel < difficultySingletonModel.MinOfDifficultyLevelRange)
            {
                difficultySingletonModel.CurrentDifficultyLevel += 2;
                await _difficultyDataManager.SetDifficultytoFile(difficultySingletonModel);
                return;
            }

            difficultySingletonModel.CurrentDifficultyLevel = random.Next(difficultySingletonModel.MinOfDifficultyLevelRange,
                                                                            difficultySingletonModel.MaxOfDifficultyLevelRange);

            await _difficultyDataManager.SetDifficultytoFile(difficultySingletonModel);
        }

        private void CalculateMinDifficultyValue()
        {
            difficultySingletonModel.MinOfDifficultyLevelRange = difficultySingletonModel.CenterOfDifficultyLevel - difficultySingletonModel.RangeCount;
            if (difficultySingletonModel.MinOfDifficultyLevelRange < 1)
                difficultySingletonModel.MinOfDifficultyLevelRange = 1;
            else if (difficultySingletonModel.MinOfDifficultyLevelRange > 8)
                difficultySingletonModel.MinOfDifficultyLevelRange = 8;
        }

        private void CalculateMaxDifficultyValue()
        {
            difficultySingletonModel.MaxOfDifficultyLevelRange = difficultySingletonModel.CenterOfDifficultyLevel + difficultySingletonModel.RangeCount;
            if (difficultySingletonModel.MaxOfDifficultyLevelRange > 10)
                difficultySingletonModel.MaxOfDifficultyLevelRange = 10;
            else if (difficultySingletonModel.MaxOfDifficultyLevelRange < 2)
                difficultySingletonModel.MaxOfDifficultyLevelRange = 2;
        }
    }
}
