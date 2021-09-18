using System;
using System.Collections.Generic;
using System.Text;

namespace Appneuron.Core.CoreServices.MessageBrockers.Kafka.Model
{
    public class PartitionModel
    {
        public string TopicName { get; set; }
        public int PartitionCount { get; set; }
    }
}
