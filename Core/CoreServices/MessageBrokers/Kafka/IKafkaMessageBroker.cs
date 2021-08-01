using Assets.Appneuron.Core.CoreServices.ResultService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Core.CoreServices.MessageBrockers.Kafka
{
    public interface IKafkaMessageBroker
    {
        Task<IResult> SendMessageAsync<T>(T messageModel) where T :
         class, new();


        Task<IDataResult<T>> getMessageAsync<T>() where T :
         class, new();

    }
}
