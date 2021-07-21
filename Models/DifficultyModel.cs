using System;
using System.Collections.Generic;
using System.Text;

namespace Appneuron.Models
{
    [Serializable]
    public class DifficultyModel
    {
        public int CenterOfDifficultyLevel { get; set; }
        public int RangeCount { get; set; }
    }
}
