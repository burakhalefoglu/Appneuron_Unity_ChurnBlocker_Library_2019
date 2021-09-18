using Appneuron.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appneuron.Services
{
    public static class ClientDifficultyService
    {
        private static readonly DifficultySingletonModel difficultySingletonModel = DifficultySingletonModel.Instance;

        public static int getDifficuly()
        {
            return difficultySingletonModel.CenterOfDifficultyLevel;
        }
    }
}
