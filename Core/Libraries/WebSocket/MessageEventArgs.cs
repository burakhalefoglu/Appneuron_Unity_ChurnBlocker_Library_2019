/*
 * MessageEventArgs.cs
 *
 * The MIT License
 *
 * Copyright (c) 2012-2016 sta.blockhead
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
    /// Represents the event data for the <see cref="WebSocket.OnMessage"/> event.
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// Defines the _data.
        /// </summary>
        private string _data;

        /// <summary>
        /// Defines the _dataSet.
        /// </summary>
        private bool _dataSet;

        /// <summary>
        /// Defines the _opcode.
        /// </summary>
        private Opcode _opcode;

        /// <summary>
        /// Defines the _rawData.
        /// </summary>
        private byte[] _rawData;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageEventArgs"/> class.
        /// </summary>
        /// <param name="frame">The frame<see cref="WebSocketFrame"/>.</param>
        internal MessageEventArgs(WebSocketFrame frame)
        {
            _opcode = frame.Opcode;
            _rawData = frame.PayloadData.ApplicationData;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageEventArgs"/> class.
        /// </summary>
        /// <param name="opcode">The opcode<see cref="Opcode"/>.</param>
        /// <param name="rawData">The rawData<see cref="byte[]"/>.</param>
        internal MessageEventArgs(Opcode opcode, byte[] rawData)
        {
            if ((ulong)rawData.LongLength > PayloadData.MaxLength)
                throw new WebSocketException(CloseStatusCode.TooBig);

            _opcode = opcode;
            _rawData = rawData;
        }

        /// <summary>
        /// Gets the opcode for the message....
        /// </summary>
        internal Opcode Opcode
        {
            get
            {
                return _opcode;
            }
        }

        /// <summary>
        /// Gets the message data as a <see cref="string"/>....
        /// </summary>
        public string Data
        {
            get
            {
                setData();
                return _data;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the message type is binary....
        /// </summary>
        public bool IsBinary
        {
            get
            {
                return _opcode == Opcode.Binary;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the message type is ping....
        /// </summary>
        public bool IsPing
        {
            get
            {
                return _opcode == Opcode.Ping;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the message type is text....
        /// </summary>
        public bool IsText
        {
            get
            {
                return _opcode == Opcode.Text;
            }
        }

        /// <summary>
        /// Gets the message data as an array of <see cref="byte"/>....
        /// </summary>
        public byte[] RawData
        {
            get
            {
                setData();
                return _rawData;
            }
        }

        /// <summary>
        /// The setData.
        /// </summary>
        private void setData()
        {
            if (_dataSet)
                return;

            if (_opcode == Opcode.Binary)
            {
                _dataSet = true;
                return;
            }

            string data;
            if (_rawData.TryGetUTF8DecodedString(out data))
                _data = data;

            _dataSet = true;
        }
    }
}
