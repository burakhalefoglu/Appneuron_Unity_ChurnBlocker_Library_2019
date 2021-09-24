/*
 * CloseEventArgs.cs
 *
 * The MIT License
 *
 * Copyright (c) 2012-2019 sta.blockhead
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

namespace AppneuronUnity.Core.Libraries.WebSocket
{
    using System;

    /// <summary>
    /// Represents the event data for the <see cref="WebSocket.OnClose"/> event.
    /// </summary>
    public class CloseEventArgs : EventArgs
    {
        /// <summary>
        /// Defines the _clean.
        /// </summary>
        private bool _clean;

        /// <summary>
        /// Defines the _payloadData.
        /// </summary>
        private PayloadData _payloadData;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloseEventArgs"/> class.
        /// </summary>
        /// <param name="payloadData">The payloadData<see cref="PayloadData"/>.</param>
        /// <param name="clean">The clean<see cref="bool"/>.</param>
        internal CloseEventArgs(PayloadData payloadData, bool clean)
        {
            _payloadData = payloadData;
            _clean = clean;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloseEventArgs"/> class.
        /// </summary>
        /// <param name="code">The code<see cref="ushort"/>.</param>
        /// <param name="reason">The reason<see cref="string"/>.</param>
        /// <param name="clean">The clean<see cref="bool"/>.</param>
        internal CloseEventArgs(ushort code, string reason, bool clean)
        {
            _payloadData = new PayloadData(code, reason);
            _clean = clean;
        }

        /// <summary>
        /// Gets the status code for the connection close....
        /// </summary>
        public ushort Code
        {
            get
            {
                return _payloadData.Code;
            }
        }

        /// <summary>
        /// Gets the reason for the connection close....
        /// </summary>
        public string Reason
        {
            get
            {
                return _payloadData.Reason;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the connection has been closed cleanly....
        /// </summary>
        public bool WasClean
        {
            get
            {
                return _clean;
            }
        }
    }
}
