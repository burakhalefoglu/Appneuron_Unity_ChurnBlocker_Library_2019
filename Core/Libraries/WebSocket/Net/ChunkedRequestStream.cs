/*
 * ChunkedRequestStream.cs
 *
 * This code is derived from ChunkedInputStream.cs (System.Net) of Mono
 * (http://www.mono-project.com).
 *
 * The MIT License
 *
 * Copyright (c) 2005 Novell, Inc. (http://www.novell.com)
 * Copyright (c) 2012-2015 sta.blockhead
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

namespace AppneuronUnity.Core.Libraries.WebSocket.Net
{
    using System;
    using System.IO;

    /// <summary>
    /// Defines the <see cref="ChunkedRequestStream" />.
    /// </summary>
    internal class ChunkedRequestStream : RequestStream
    {
        /// <summary>
        /// Defines the _bufferLength.
        /// </summary>
        private const int _bufferLength = 8192;

        /// <summary>
        /// Defines the _context.
        /// </summary>
        private HttpListenerContext _context;

        /// <summary>
        /// Defines the _decoder.
        /// </summary>
        private ChunkStream _decoder;

        /// <summary>
        /// Defines the _disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Defines the _noMoreData.
        /// </summary>
        private bool _noMoreData;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChunkedRequestStream"/> class.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="buffer">The buffer<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="count">The count<see cref="int"/>.</param>
        /// <param name="context">The context<see cref="HttpListenerContext"/>.</param>
        internal ChunkedRequestStream(
      Stream stream, byte[] buffer, int offset, int count, HttpListenerContext context)
      : base(stream, buffer, offset, count)
        {
            _context = context;
            _decoder = new ChunkStream((WebHeaderCollection)context.Request.Headers);
        }

        /// <summary>
        /// Gets or sets the Decoder.
        /// </summary>
        internal ChunkStream Decoder
        {
            get
            {
                return _decoder;
            }

            set
            {
                _decoder = value;
            }
        }

        /// <summary>
        /// The onRead.
        /// </summary>
        /// <param name="asyncResult">The asyncResult<see cref="IAsyncResult"/>.</param>
        private void onRead(IAsyncResult asyncResult)
        {
            var rstate = (ReadBufferState)asyncResult.AsyncState;
            var ares = rstate.AsyncResult;
            try
            {
                var nread = base.EndRead(asyncResult);
                _decoder.Write(ares.Buffer, ares.Offset, nread);
                nread = _decoder.Read(rstate.Buffer, rstate.Offset, rstate.Count);
                rstate.Offset += nread;
                rstate.Count -= nread;
                if (rstate.Count == 0 || !_decoder.WantMore || nread == 0)
                {
                    _noMoreData = !_decoder.WantMore && nread == 0;
                    ares.Count = rstate.InitialCount - rstate.Count;
                    ares.Complete();

                    return;
                }

                ares.Offset = 0;
                ares.Count = Math.Min(_bufferLength, _decoder.ChunkLeft + 6);
                base.BeginRead(ares.Buffer, ares.Offset, ares.Count, onRead, rstate);
            }
            catch (Exception ex)
            {
                _context.Connection.SendError(ex.Message, 400);
                ares.Complete(ex);
            }
        }

        /// <summary>
        /// The BeginRead.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="count">The count<see cref="int"/>.</param>
        /// <param name="callback">The callback<see cref="AsyncCallback"/>.</param>
        /// <param name="state">The state<see cref="object"/>.</param>
        /// <returns>The <see cref="IAsyncResult"/>.</returns>
        public override IAsyncResult BeginRead(
      byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().ToString());

            if (buffer == null)
                throw new ArgumentNullException("buffer");

            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "A negative value.");

            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "A negative value.");

            var len = buffer.Length;
            if (offset + count > len)
                throw new ArgumentException(
                  "The sum of 'offset' and 'count' is greater than 'buffer' length.");

            var ares = new HttpStreamAsyncResult(callback, state);
            if (_noMoreData)
            {
                ares.Complete();
                return ares;
            }

            var nread = _decoder.Read(buffer, offset, count);
            offset += nread;
            count -= nread;
            if (count == 0)
            {
                // Got all we wanted, no need to bother the decoder yet.
                ares.Count = nread;
                ares.Complete();

                return ares;
            }

            if (!_decoder.WantMore)
            {
                _noMoreData = nread == 0;
                ares.Count = nread;
                ares.Complete();

                return ares;
            }

            ares.Buffer = new byte[_bufferLength];
            ares.Offset = 0;
            ares.Count = _bufferLength;

            var rstate = new ReadBufferState(buffer, offset, count, ares);
            rstate.InitialCount += nread;
            base.BeginRead(ares.Buffer, ares.Offset, ares.Count, onRead, rstate);

            return ares;
        }

        /// <summary>
        /// The Close.
        /// </summary>
        public override void Close()
        {
            if (_disposed)
                return;

            _disposed = true;
            base.Close();
        }

        /// <summary>
        /// The EndRead.
        /// </summary>
        /// <param name="asyncResult">The asyncResult<see cref="IAsyncResult"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public override int EndRead(IAsyncResult asyncResult)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().ToString());

            if (asyncResult == null)
                throw new ArgumentNullException("asyncResult");

            var ares = asyncResult as HttpStreamAsyncResult;
            if (ares == null)
                throw new ArgumentException("A wrong IAsyncResult.", "asyncResult");

            if (!ares.IsCompleted)
                ares.AsyncWaitHandle.WaitOne();

            if (ares.HasException)
                throw new HttpListenerException(400, "I/O operation aborted.");

            return ares.Count;
        }

        /// <summary>
        /// The Read.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="count">The count<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            var ares = BeginRead(buffer, offset, count, null, null);
            return EndRead(ares);
        }
    }
}
