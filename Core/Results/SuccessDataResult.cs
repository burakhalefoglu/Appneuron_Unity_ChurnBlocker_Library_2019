namespace AppneuronUnity.Core.Results
{
    /// <summary>
    /// Defines the <see cref="SuccessDataResult{T}" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    internal class SuccessDataResult<T> : DataResult<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessDataResult{T}"/> class.
        /// </summary>
        /// <param name="data">The data<see cref="T"/>.</param>
        public SuccessDataResult(T data) : base(data, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessDataResult{T}"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public SuccessDataResult(string message) : base(default, true, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessDataResult{T}"/> class.
        /// </summary>
        /// <param name="data">The data<see cref="T"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        public SuccessDataResult(T data, string message) : base(data, true, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessDataResult{T}"/> class.
        /// </summary>
        /// <param name="data">The data<see cref="T"/>.</param>
        /// <param name="statuseCode">The statuseCode<see cref="int"/>.</param>
        public SuccessDataResult(T data, int statuseCode) : base(data, true, statuseCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessDataResult{T}"/> class.
        /// </summary>
        /// <param name="data">The data<see cref="T"/>.</param>
        /// <param name="statuseCode">The statuseCode<see cref="int"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        public SuccessDataResult(T data, int statuseCode, string message) : base(data, true, statuseCode, message)
        {
        }
    }
}
