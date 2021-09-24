namespace AppneuronUnity.Core.CoreServices.MessageBrokers.Kafka
{
    using AppneuronUnity.Core.CoreServices.RestClientServices.Abstract;
    using AppneuronUnity.Core.Libraries.LitJson;
    using AppneuronUnity.Core.Results;
    using Confluent.Kafka;
    using System;
    using System.Threading.Tasks;
    using UnityEngine;
    using Appneuron.Zenject;

    /// <summary>
    /// Defines the <see cref="KafkaMessageBrokerService" />.
    /// </summary>
    internal class KafkaMessageBrokerService : IMessageBrokerService
    {
        [Inject]
        private IRestClientServices _restClientServices;

        /// <summary>
        /// The GetMessageAsync.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <returns>The <see cref="Task{IDataResult{T}}"/>.</returns>
        public Task<IDataResult<T>> GetMessageAsync<T>() where T : class, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The SendMessageAsync.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="messageModel">The messageModel<see cref="T"/>.</param>
        /// <returns>The <see cref="Task{IResult}"/>.</returns>
        public async Task<IResult> SendMessageAsync<T>(T messageModel) where T :
         class, new()
        {

            var result = await _restClientServices.IsInternetConnectedAsync();

            if (!result.Success)
                //TODO: Collect log
                return new ErrorResult();
            try
            {
                var producerConfig = new ProducerConfig
                {
                    BootstrapServers = Appsettings.BootstrapServers,
                    Acks = Acks.All
                };
                

                var message = JsonMapper.ToJson(messageModel);
                var topicName = typeof(T).Name;
                using (var p = new ProducerBuilder<string, string>(producerConfig).Build())
                {
                    await p.ProduceAsync(topicName, new Message<string, string>
                    {
                        Key = SystemInfo.deviceUniqueIdentifier,
                        Value = message
                    });
                    return new SuccessResult();
                }
            }
            catch (ProduceException<Null, string> e)
            {
                //TODO: Collenct log
                return new ErrorResult();
            }
        }
    }
}
