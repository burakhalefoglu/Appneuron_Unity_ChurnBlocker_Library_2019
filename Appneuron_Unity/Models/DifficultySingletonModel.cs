using System;
using System.Collections.Generic;
using System.Text;

namespace Appneuron.Models
{
    public sealed class DifficultySingletonModel
    {

        private static readonly DifficultySingletonModel instance = new DifficultySingletonModel();

        static DifficultySingletonModel()
        {
        }

        private DifficultySingletonModel()
        {
        }

        public static DifficultySingletonModel Instance
        {
            get
            {
                return instance;
            }
        }


        public int CurrentDifficultyLevel { get; set; }
        public int CenterOfDifficultyLevel { get; set; }
        public int MinOfDifficultyLevelRange { get; set; }
        public int MaxOfDifficultyLevelRange { get; set; }
        public int RangeCount { get; set; }

    }
}
