using System;
using System.Collections.Generic;
using System.Text;

namespace AppneuronUnity.ProductModules.ChurnPrediction.WebSocketWorkers.ChurnMLResult.Model
{
    internal class MlResultModel
    {
        public bool OneDayChurn { get; set; }
        public bool ThreeDayChurn { get; set; }
        public bool SevenDayChurn { get; set; }
    }
}
