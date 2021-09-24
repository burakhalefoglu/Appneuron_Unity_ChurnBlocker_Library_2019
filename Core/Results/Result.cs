namespace AppneuronUnity.Core.Results
{
    /// <summary>
    /// Defines the <see cref="Result" />.
    /// </summary>
    internal class Result : IResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="success">The success<see cref="bool"/>.</param>
        public Result(bool success)
        {
            Success = success;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="success">The success<see cref="bool"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        public Result(bool success, string message) : this(success)
        {
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="success">The success<see cref="bool"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="statuseCode">The statuseCode<see cref="int"/>.</param>
        public Result(bool success, string message, int statuseCode) : this(success, message)
        {
            StatuseCode = statuseCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="success">The success<see cref="bool"/>.</param>
        /// <param name="statuseCode">The statuseCode<see cref="int"/>.</param>
        public Result(bool success, int statuseCode) : this(success)
        {
            StatuseCode = statuseCode;
        }

        /// <summary>
        /// Gets a value indicating whether Success.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Gets the Message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the StatuseCode.
        /// </summary>
        public int StatuseCode { get; }
    }
}
