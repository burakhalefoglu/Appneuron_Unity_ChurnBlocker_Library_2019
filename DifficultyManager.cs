using Appneuron.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appneuron
{
    public class DifficultyManager
    {
        private DifficultySingletonModel difficultySingletonModel = DifficultySingletonModel.Instance;

        public void GetDifficultyLevelFromServer(DifficultyModel difficultyModel)
        {
            if (difficultyModel.CenterOfDifficultyLevel == 0)
            {
                CalculateDifficultyManually();
                return;
            }

            difficultySingletonModel.CenterOfDifficultyLevel = difficultyModel.CenterOfDifficultyLevel;
            difficultySingletonModel.RangeCount = difficultyModel.RangeCount;
            CalculateDifficulty();
        }

        private void CalculateDifficultyManually()
        {
            //flow bazlı,
            //harcama bazlı,
            //veya diğer yöntemlerle burada manuel hesap yapacak!!!!

        }

        private void CalculateDifficulty()
        {

            CalculateMaxDifficultyValue();
            CalculateMinDifficultyValue();

            int currentDifficultyLevel = difficultySingletonModel.CurrentDifficultyLevel;
            Random random = new Random();

            if (currentDifficultyLevel < difficultySingletonModel.MinOfDifficultyLevelRange)
            {
                difficultySingletonModel.CurrentDifficultyLevel += 2;
                return;
            }
            difficultySingletonModel.CurrentDifficultyLevel = random.Next(difficultySingletonModel.MinOfDifficultyLevelRange,
                                                                            difficultySingletonModel.MaxOfDifficultyLevelRange);
        }


        private void CalculateMinDifficultyValue()
        {
            difficultySingletonModel.MinOfDifficultyLevelRange = difficultySingletonModel.CenterOfDifficultyLevel - difficultySingletonModel.RangeCount;
            if (difficultySingletonModel.MinOfDifficultyLevelRange < 1)
                difficultySingletonModel.MinOfDifficultyLevelRange = 1;
            
            else if (difficultySingletonModel.MinOfDifficultyLevelRange > 16)
                difficultySingletonModel.MinOfDifficultyLevelRange = 16;
        }

        private void CalculateMaxDifficultyValue()
        {
            difficultySingletonModel.MaxOfDifficultyLevelRange = difficultySingletonModel.CenterOfDifficultyLevel + difficultySingletonModel.RangeCount;
            if (difficultySingletonModel.MaxOfDifficultyLevelRange > 20)
                difficultySingletonModel.MaxOfDifficultyLevelRange = 20;

            else if (difficultySingletonModel.MaxOfDifficultyLevelRange < 3)
                difficultySingletonModel.MaxOfDifficultyLevelRange = 3;
        }
    }

}
