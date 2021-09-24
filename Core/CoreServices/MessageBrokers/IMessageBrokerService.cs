namespace AppneuronUnity.Core.CoreServices.MessageBrokers
{
    using AppneuronUnity.Core.Results;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IMessageBrokerService" />.
    /// </summary>
    internal interface IMessageBrokerService
    {
        /// <summary>
        /// The SendMessageAsync.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="messageModel">The messageModel<see cref="T"/>.</param>
        /// <returns>The <see cref="Task{IResult}"/>.</returns>
        Task<IResult> SendMessageAsync<T>(T messageModel) where T :
         class, new();

        /// <summary>
        /// The GetMessageAsync.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <returns>The <see cref="Task{IDataResult{T}}"/>.</returns>
        Task<IDataResult<T>> GetMessageAsync<T>() where T :
         class, new();
    }
}
