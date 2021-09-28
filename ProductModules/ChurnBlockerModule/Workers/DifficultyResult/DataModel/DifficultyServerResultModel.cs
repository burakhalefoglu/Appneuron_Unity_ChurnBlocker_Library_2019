namespace AppneuronUnity.ProductModules.ChurnBlockerModule.WeboscketWorkers.DifficultyResult.Models
{
    using System;

    [Serializable]
    public class DifficultyServerResultModel
    {
        public int CenterOfDifficultyLevel { get; set; }
        public int RangeCount { get; set; }
    }
}
