namespace AppneuronUnity.Core.Results
{
    /// <summary>
    /// Defines the <see cref="IResult" />.
    /// </summary>
    internal interface IResult
    {
        /// <summary>
        /// Gets the StatuseCode.
        /// </summary>
        int StatuseCode { get; }

        /// <summary>
        /// Gets the Success
        /// Gets a value indicating whether Success...
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// Gets the Message.
        /// </summary>
        string Message { get; }
    }
}
