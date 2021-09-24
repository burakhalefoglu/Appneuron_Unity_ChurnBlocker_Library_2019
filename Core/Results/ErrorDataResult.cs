namespace AppneuronUnity.Core.Results
{
    /// <summary>
    /// Defines the <see cref="ErrorDataResult{T}" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    internal class ErrorDataResult<T> : DataResult<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDataResult{T}"/> class.
        /// </summary>
        /// <param name="data">The data<see cref="T"/>.</param>
        public ErrorDataResult(T data) : base(data, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDataResult{T}"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public ErrorDataResult(string message) : base(default, false, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDataResult{T}"/> class.
        /// </summary>
        /// <param name="data">The data<see cref="T"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        public ErrorDataResult(T data, string message) : base(data, false, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDataResult{T}"/> class.
        /// </summary>
        /// <param name="data">The data<see cref="T"/>.</param>
        /// <param name="statuseCode">The statuseCode<see cref="int"/>.</param>
        public ErrorDataResult(T data, int statuseCode) : base(data, false, statuseCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDataResult{T}"/> class.
        /// </summary>
        /// <param name="data">The data<see cref="T"/>.</param>
        /// <param name="statuseCode">The statuseCode<see cref="int"/>.</param>
        public ErrorDataResult(int statuseCode) : base(default, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDataResult{T}"/> class.
        /// </summary>
        /// <param name="data">The data<see cref="T"/>.</param>
        /// <param name="statuseCode">The statuseCode<see cref="int"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        public ErrorDataResult(T data, int statuseCode, string message) : base(data, false, statuseCode, message)
        {
        }
    }
}
