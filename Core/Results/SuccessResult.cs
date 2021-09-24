namespace AppneuronUnity.Core.Results
{
    /// <summary>
    /// Defines the <see cref="SuccessResult" />.
    /// </summary>
    internal class SuccessResult : Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessResult"/> class.
        /// </summary>
        public SuccessResult() : base(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessResult"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public SuccessResult(string message) : base(true, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessResult"/> class.
        /// </summary>
        /// <param name="StatuseCode">The StatuseCode<see cref="int"/>.</param>
        public SuccessResult(int StatuseCode) : base(true, StatuseCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessResult"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="StatuseCode">The StatuseCode<see cref="int"/>.</param>
        public SuccessResult(string message, int StatuseCode) : base(true, message, StatuseCode)
        {
        }
    }
}
