namespace AppneuronUnity.Core.Results
{
    /// <summary>
    /// Defines the <see cref="ErrorResult" />.
    /// </summary>
    internal class ErrorResult : Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResult"/> class.
        /// </summary>
        public ErrorResult() : base(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResult"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public ErrorResult(string message) : base(false, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResult"/> class.
        /// </summary>
        /// <param name="StatuseCode">The StatuseCode<see cref="int"/>.</param>
        public ErrorResult(int StatuseCode) : base(false, StatuseCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResult"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="StatuseCode">The StatuseCode<see cref="int"/>.</param>
        public ErrorResult(string message, int StatuseCode) : base(false, message, StatuseCode)
        {
        }
    }
}
