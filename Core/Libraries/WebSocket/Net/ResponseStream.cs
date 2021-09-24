/*
 * ResponseStream.cs
 *
 * This code is derived from ResponseStream.cs (System.Net) of Mono
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
    using System.Text;

    /// <summary>
    /// Defines the <see cref="ResponseStream" />.
    /// </summary>
    internal class ResponseStream : Stream
    {
        /// <summary>
        /// Defines the _body.
        /// </summary>
        private MemoryStream _body;

        /// <summary>
        /// Defines the _crlf.
        /// </summary>
        private static readonly byte[] _crlf = new byte[] { 13, 10 };

        /// <summary>
        /// Defines the _disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Defines the _response.
        /// </summary>
        private HttpListenerResponse _response;

        /// <summary>
        /// Defines the _sendChunked.
        /// </summary>
        private bool _sendChunked;

        /// <summary>
        /// Defines the _stream.
        /// </summary>
        private Stream _stream;

        /// <summary>
        /// Defines the _write.
        /// </summary>
        private Action<byte[], int, int> _write;

        /// <summary>
        /// Defines the _writeBody.
        /// </summary>
        private Action<byte[], int, int> _writeBody;

        /// <summary>
        /// Defines the _writeChunked.
        /// </summary>
        private Action<byte[], int, int> _writeChunked;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseStream"/> class.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="response">The response<see cref="HttpListenerResponse"/>.</param>
        /// <param name="ignoreWriteExceptions">The ignoreWriteExceptions<see cref="bool"/>.</param>
        internal ResponseStream(
      Stream stream, HttpListenerResponse response, bool ignoreWriteExceptions)
        {
            _stream = stream;
            _response = response;

            if (ignoreWriteExceptions)
            {
                _write = writeWithoutThrowingException;
                _writeChunked = writeChunkedWithoutThrowingException;
            }
            else
            {
                _write = stream.Write;
                _writeChunked = writeChunked;
            }

            _body = new MemoryStream();
        }

        /// <summary>
        /// Gets a value indicating whether CanRead.
        /// </summary>
        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether CanSeek.
        /// </summary>
        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether CanWrite.
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                return !_disposed;
            }
        }

        /// <summary>
        /// Gets the Length.
        /// </summary>
        public override long Length
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets or sets the Position.
        /// </summary>
        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// The flush.
        /// </summary>
        /// <param name="closing">The closing<see cref="bool"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool flush(bool closing)
        {
            if (!_response.HeadersSent)
            {
                if (!flushHeaders(closing))
                {
                    if (closing)
                        _response.CloseConnection = true;

                    return false;
                }

                _sendChunked = _response.SendChunked;
                _writeBody = _sendChunked ? _writeChunked : _write;
            }

            flushBody(closing);
            if (closing && _sendChunked)
            {
                var last = getChunkSizeBytes(0, true);
                _write(last, 0, last.Length);
            }

            return true;
        }

        /// <summary>
        /// The flushBody.
        /// </summary>
        /// <param name="closing">The closing<see cref="bool"/>.</param>
        private void flushBody(bool closing)
        {
            using (_body)
            {
                var len = _body.Length;
                if (len > Int32.MaxValue)
                {
                    _body.Position = 0;
                    var buffLen = 1024;
                    var buff = new byte[buffLen];
                    var nread = 0;
                    while ((nread = _body.Read(buff, 0, buffLen)) > 0)
                        _writeBody(buff, 0, nread);
                }
                else if (len > 0)
                {
                    _writeBody(_body.GetBuffer(), 0, (int)len);
                }
            }

            _body = !closing ? new MemoryStream() : null;
        }

        /// <summary>
        /// The flushHeaders.
        /// </summary>
        /// <param name="closing">The closing<see cref="bool"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool flushHeaders(bool closing)
        {
            using (var buff = new MemoryStream())
            {
                var headers = _response.WriteHeadersTo(buff);
                var start = buff.Position;
                var len = buff.Length - start;
                if (len > 32768)
                    return false;

                if (!_response.SendChunked && _response.ContentLength64 != _body.Length)
                    return false;

                _write(buff.GetBuffer(), (int)start, (int)len);
                _response.CloseConnection = headers["Connection"] == "close";
                _response.HeadersSent = true;
            }

            return true;
        }

        /// <summary>
        /// The getChunkSizeBytes.
        /// </summary>
        /// <param name="size">The size<see cref="int"/>.</param>
        /// <param name="final">The final<see cref="bool"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        private static byte[] getChunkSizeBytes(int size, bool final)
        {
            return Encoding.ASCII.GetBytes(String.Format("{0:x}\r\n{1}", size, final ? "\r\n" : ""));
        }

        /// <summary>
        /// The writeChunked.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="count">The count<see cref="int"/>.</param>
        private void writeChunked(byte[] buffer, int offset, int count)
        {
            var size = getChunkSizeBytes(count, false);
            _stream.Write(size, 0, size.Length);
            _stream.Write(buffer, offset, count);
            _stream.Write(_crlf, 0, 2);
        }

        /// <summary>
        /// The writeChunkedWithoutThrowingException.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="count">The count<see cref="int"/>.</param>
        private void writeChunkedWithoutThrowingException(byte[] buffer, int offset, int count)
        {
            try
            {
                writeChunked(buffer, offset, count);
            }
            catch
            {
            }
        }

        /// <summary>
        /// The writeWithoutThrowingException.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="count">The count<see cref="int"/>.</param>
        private void writeWithoutThrowingException(byte[] buffer, int offset, int count)
        {
            try
            {
                _stream.Write(buffer, offset, count);
            }
            catch
            {
            }
        }

        /// <summary>
        /// The Close.
        /// </summary>
        /// <param name="force">The force<see cref="bool"/>.</param>
        internal void Close(bool force)
        {
            if (_disposed)
                return;

            _disposed = true;
            if (!force && flush(true))
            {
                _response.Close();
            }
            else
            {
                if (_sendChunked)
                {
                    var last = getChunkSizeBytes(0, true);
                    _write(last, 0, last.Length);
                }

                _body.Dispose();
                _body = null;

                _response.Abort();
            }

            _response = null;
            _stream = null;
        }

        /// <summary>
        /// The InternalWrite.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="count">The count<see cref="int"/>.</param>
        internal void InternalWrite(byte[] buffer, int offset, int count)
        {
            _write(buffer, offset, count);
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
            throw new NotSupportedException();
        }

        /// <summary>
        /// The BeginWrite.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="count">The count<see cref="int"/>.</param>
        /// <param name="callback">The callback<see cref="AsyncCallback"/>.</param>
        /// <param name="state">The state<see cref="object"/>.</param>
        /// <returns>The <see cref="IAsyncResult"/>.</returns>
        public override IAsyncResult BeginWrite(
      byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().ToString());

            return _body.BeginWrite(buffer, offset, count, callback, state);
        }

        /// <summary>
        /// The Close.
        /// </summary>
        public override void Close()
        {
            Close(false);
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        /// <param name="disposing">The disposing<see cref="bool"/>.</param>
        protected override void Dispose(bool disposing)
        {
            Close(!disposing);
        }

        /// <summary>
        /// The EndRead.
        /// </summary>
        /// <param name="asyncResult">The asyncResult<see cref="IAsyncResult"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public override int EndRead(IAsyncResult asyncResult)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// The EndWrite.
        /// </summary>
        /// <param name="asyncResult">The asyncResult<see cref="IAsyncResult"/>.</param>
        public override void EndWrite(IAsyncResult asyncResult)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().ToString());

            _body.EndWrite(asyncResult);
        }

        /// <summary>
        /// The Flush.
        /// </summary>
        public override void Flush()
        {
            if (!_disposed && (_sendChunked || _response.SendChunked))
                flush(false);
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
            throw new NotSupportedException();
        }

        /// <summary>
        /// The Seek.
        /// </summary>
        /// <param name="offset">The offset<see cref="long"/>.</param>
        /// <param name="origin">The origin<see cref="SeekOrigin"/>.</param>
        /// <returns>The <see cref="long"/>.</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// The SetLength.
        /// </summary>
        /// <param name="value">The value<see cref="long"/>.</param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// The Write.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="count">The count<see cref="int"/>.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().ToString());

            _body.Write(buffer, offset, count);
        }
    }
}
