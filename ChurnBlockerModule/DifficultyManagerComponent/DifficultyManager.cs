using Appneuron.DifficultyManagerComponent.DataAccess;
using Appneuron.Models;
using Appneuron.Services;
using Ninject;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Appneuron.DifficultyManagerComponent
{
    public static class DifficultyManager
    {
        private static readonly DifficultySingletonModel difficultySingletonModel = DifficultySingletonModel.Instance;
        private static IDifficultyLevelDal _difficultyLevelDal;
        private static string fileName = "DifficultyLevelData.data";


        public static void MakeConfing()
        {
            using (var kernel = new StandardKernel())
            {

                kernel.Load(Assembly.GetExecutingAssembly());
                _difficultyLevelDal = kernel.Get<IDifficultyLevelDal>();

            }

        }

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
            FlowBaseDifficulty();
        }


        private static async void FlowBaseDifficulty()
        {
            var flow = GetFlow();
            Random random = new Random();
            var TopFlow = random.Next(75, 98);
            var increaseAmount = random.Next(1, 4);


            var difficultyLevel = await _difficultyLevelDal.SelectAsync(ComponentsConfigService.DifficultyModel + fileName);
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
            return 100 - (CharInformation.CharFinishHealth * 100 / CharInformation.CharStarterHealth);
             
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
                await _difficultyLevelDal.InsertAsync(ComponentsConfigService.DifficultyModel + fileName, model);
                return;
            }

            difficultySingletonModel.CurrentDifficultyLevel = random.Next(difficultySingletonModel.MinOfDifficultyLevelRange,
                                                                            difficultySingletonModel.MaxOfDifficultyLevelRange);

            model.CenterOfDifficultyLevel = difficultySingletonModel.CurrentDifficultyLevel;
            await _difficultyLevelDal.InsertAsync(ComponentsConfigService.DifficultyModel + fileName, model);

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
