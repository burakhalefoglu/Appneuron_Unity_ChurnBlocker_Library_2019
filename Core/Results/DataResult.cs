namespace AppneuronUnity.Core.Results
{
    /// <summary>
    /// Defines the <see cref="DataResult{T}" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    internal class DataResult<T> : IDataResult<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataResult{T}"/> class.
        /// </summary>
        /// <param name="data">The data<see cref="T"/>.</param>
        /// <param name="success">The success<see cref="bool"/>.</param>
        public DataResult(T data, bool success)
        {
            Data = data;
            Success = success;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataResult{T}"/> class.
        /// </summary>
        /// <param name="data">The data<see cref="T"/>.</param>
        /// <param name="success">The success<see cref="bool"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        public DataResult(T data, bool success, string message) : this(data, success)
        {
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataResult{T}"/> class.
        /// </summary>
        /// <param name="data">The data<see cref="T"/>.</param>
        /// <param name="success">The success<see cref="bool"/>.</param>
        /// <param name="statuseCode">The statuseCode<see cref="int"/>.</param>
        public DataResult(T data, bool success, int statuseCode) : this(data, success)
        {
            StatuseCode = statuseCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataResult{T}"/> class.
        /// </summary>
        /// <param name="data">The data<see cref="T"/>.</param>
        /// <param name="success">The success<see cref="bool"/>.</param>
        /// <param name="statuseCode">The statuseCode<see cref="int"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        public DataResult(T data, bool success, int statuseCode, string message) : this(data, success, message)
        {
            StatuseCode = statuseCode;
        }

        /// <summary>
        /// Gets the Data.
        /// </summary>
        public T Data { get; }

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
