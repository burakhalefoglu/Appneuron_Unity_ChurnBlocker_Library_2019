namespace AppneuronUnity.Core.Results
{
    /// <summary>
    /// Defines the <see cref="IDataResult{T}" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    internal interface IDataResult<T> : IResult
    {
        /// <summary>
        /// Gets the Data.
        /// </summary>
        T Data { get; }
    }
}
