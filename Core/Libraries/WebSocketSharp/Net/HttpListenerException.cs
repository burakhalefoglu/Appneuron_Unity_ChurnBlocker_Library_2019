/*
 * HttpListenerException.cs
 *
 * This code is derived from System.Net.HttpListenerException.cs of Mono
 * (http://www.mono-project.com).
 *
 * The MIT License
 *
 * Copyright (c) 2005 Novell, Inc. (http://www.novell.com)
 * Copyright (c) 2012-2014 sta.blockhead
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

/*
 * Authors:
 * - Gonzalo Paniagua Javier <gonzalo@novell.com>
 */

namespace AppneuronUnity.Core.Libraries.WebSocketSharp.Net
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;

    /// <summary>
    /// The exception that is thrown when a <see cref="HttpListener"/> gets an error
    /// processing an HTTP request.
    /// </summary>
    [Serializable]
    public class HttpListenerException : Win32Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpListenerException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serializationInfo<see cref="SerializationInfo"/>.</param>
        /// <param name="streamingContext">The streamingContext<see cref="StreamingContext"/>.</param>
        protected HttpListenerException(
      SerializationInfo serializationInfo, StreamingContext streamingContext)
      : base(serializationInfo, streamingContext)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpListenerException"/> class.
        /// </summary>
        public HttpListenerException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpListenerException"/> class.
        /// </summary>
        /// <param name="errorCode">The errorCode<see cref="int"/>.</param>
        public HttpListenerException(int errorCode)
      : base(errorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpListenerException"/> class.
        /// </summary>
        /// <param name="errorCode">The errorCode<see cref="int"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        public HttpListenerException(int errorCode, string message)
      : base(errorCode, message)
        {
        }

        /// <summary>
        /// Gets the error code that identifies the error that occurred....
        /// </summary>
        public override int ErrorCode
        {
            get
            {
                return NativeErrorCode;
            }
        }
    }
}
