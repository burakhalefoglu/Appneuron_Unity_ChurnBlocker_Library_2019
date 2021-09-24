/*
 * WebSocketSessionManager.cs
 *
 * The MIT License
 *
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

namespace AppneuronUnity.Core.Libraries.WebSocket.Server
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Provides the management function for the sessions in a WebSocket service.
    /// </summary>
    public class WebSocketSessionManager
    {
        /// <summary>
        /// Defines the _clean.
        /// </summary>
        private volatile bool _clean;

        /// <summary>
        /// Defines the _forSweep.
        /// </summary>
        private object _forSweep;

        /// <summary>
        /// Defines the _log.
        /// </summary>
        private Logger _log;

        /// <summary>
        /// Defines the _sessions.
        /// </summary>
        private Dictionary<string, IWebSocketSession> _sessions;

        /// <summary>
        /// Defines the _state.
        /// </summary>
        private volatile ServerState _state;

        /// <summary>
        /// Defines the _sweeping.
        /// </summary>
        private volatile bool _sweeping;

        /// <summary>
        /// Defines the _sweepTimer.
        /// </summary>
        private System.Timers.Timer _sweepTimer;

        /// <summary>
        /// Defines the _sync.
        /// </summary>
        private object _sync;

        /// <summary>
        /// Defines the _waitTime.
        /// </summary>
        private TimeSpan _waitTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketSessionManager"/> class.
        /// </summary>
        /// <param name="log">The log<see cref="Logger"/>.</param>
        internal WebSocketSessionManager(Logger log)
        {
            _log = log;

            _clean = true;
            _forSweep = new object();
            _sessions = new Dictionary<string, IWebSocketSession>();
            _state = ServerState.Ready;
            _sync = ((ICollection)_sessions).SyncRoot;
            _waitTime = TimeSpan.FromSeconds(1);

            setSweepTimer(60000);
        }

        /// <summary>
        /// Gets the State.
        /// </summary>
        internal ServerState State
        {
            get
            {
                return _state;
            }
        }

        /// <summary>
        /// Gets the IDs for the active sessions in the WebSocket service....
        /// </summary>
        public IEnumerable<string> ActiveIDs
        {
            get
            {
                foreach (var res in broadping(WebSocketFrame.EmptyPingBytes))
                {
                    if (res.Value)
                        yield return res.Key;
                }
            }
        }

        /// <summary>
        /// Gets the number of the sessions in the WebSocket service....
        /// </summary>
        public int Count
        {
            get
            {
                lock (_sync)
                    return _sessions.Count;
            }
        }

        /// <summary>
        /// Gets the IDs for the sessions in the WebSocket service....
        /// </summary>
        public IEnumerable<string> IDs
        {
            get
            {
                if (_state != ServerState.Start)
                    return Enumerable.Empty<string>();

                lock (_sync)
                {
                    if (_state != ServerState.Start)
                        return Enumerable.Empty<string>();

                    return _sessions.Keys.ToList();
                }
            }
        }

        /// <summary>
        /// Gets the IDs for the inactive sessions in the WebSocket service....
        /// </summary>
        public IEnumerable<string> InactiveIDs
        {
            get
            {
                foreach (var res in broadping(WebSocketFrame.EmptyPingBytes))
                {
                    if (!res.Value)
                        yield return res.Key;
                }
            }
        }


        /// <summary>
        /// Gets the session instance with <paramref name="id"/>.
        /// </summary>
        /// <value>
        ///   <para>
        ///   A <see cref="IWebSocketSession"/> instance or <see langword="null"/>
        ///   if not found.
        ///   </para>
        ///   <para>
        ///   The session instance provides the function to access the information
        ///   in the session.
        ///   </para>
        /// </value>
        /// <param name="id">
        /// A <see cref="string"/> that represents the ID of the session to find.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="id"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="id"/> is an empty string.
        /// </exception>
        public IWebSocketSession this[string id]
        {
            get
            {
                if (id == null)
                    throw new ArgumentNullException("id");

                if (id.Length == 0)
                    throw new ArgumentException("An empty string.", "id");

                IWebSocketSession session;
                tryGetSession(id, out session);

                return session;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether the inactive sessions in
        /// the WebSocket service are cleaned up periodically....
        /// </summary>
        public bool KeepClean
        {
            get
            {
                return _clean;
            }

            set
            {
                string msg;
                if (!canSet(out msg))
                {
                    _log.Warn(msg);
                    return;
                }

                lock (_sync)
                {
                    if (!canSet(out msg))
                    {
                        _log.Warn(msg);
                        return;
                    }

                    _clean = value;
                }
            }
        }

        /// <summary>
        /// Gets the session instances in the WebSocket service....
        /// </summary>
        public IEnumerable<IWebSocketSession> Sessions
        {
            get
            {
                if (_state != ServerState.Start)
                    return Enumerable.Empty<IWebSocketSession>();

                lock (_sync)
                {
                    if (_state != ServerState.Start)
                        return Enumerable.Empty<IWebSocketSession>();

                    return _sessions.Values.ToList();
                }
            }
        }

        /// <summary>
        /// Gets or sets the time to wait for the response to the WebSocket Ping or
        /// Close....
        /// </summary>
        public TimeSpan WaitTime
        {
            get
            {
                return _waitTime;
            }

            set
            {
                if (value <= TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException("value", "Zero or less.");

                string msg;
                if (!canSet(out msg))
                {
                    _log.Warn(msg);
                    return;
                }

                lock (_sync)
                {
                    if (!canSet(out msg))
                    {
                        _log.Warn(msg);
                        return;
                    }

                    _waitTime = value;
                }
            }
        }

        /// <summary>
        /// The broadcast.
        /// </summary>
        /// <param name="opcode">The opcode<see cref="Opcode"/>.</param>
        /// <param name="data">The data<see cref="byte[]"/>.</param>
        /// <param name="completed">The completed<see cref="Action"/>.</param>
        private void broadcast(Opcode opcode, byte[] data, Action completed)
        {
            var cache = new Dictionary<CompressionMethod, byte[]>();

            try
            {
                foreach (var session in Sessions)
                {
                    if (_state != ServerState.Start)
                    {
                        _log.Error("The service is shutting down.");
                        break;
                    }

                    session.Context.WebSocket.Send(opcode, data, cache);
                }

                if (completed != null)
                    completed();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                _log.Debug(ex.ToString());
            }
            finally
            {
                cache.Clear();
            }
        }

        /// <summary>
        /// The broadcast.
        /// </summary>
        /// <param name="opcode">The opcode<see cref="Opcode"/>.</param>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="completed">The completed<see cref="Action"/>.</param>
        private void broadcast(Opcode opcode, Stream stream, Action completed)
        {
            var cache = new Dictionary<CompressionMethod, Stream>();

            try
            {
                foreach (var session in Sessions)
                {
                    if (_state != ServerState.Start)
                    {
                        _log.Error("The service is shutting down.");
                        break;
                    }

                    session.Context.WebSocket.Send(opcode, stream, cache);
                }

                if (completed != null)
                    completed();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                _log.Debug(ex.ToString());
            }
            finally
            {
                foreach (var cached in cache.Values)
                    cached.Dispose();

                cache.Clear();
            }
        }

        /// <summary>
        /// The broadcastAsync.
        /// </summary>
        /// <param name="opcode">The opcode<see cref="Opcode"/>.</param>
        /// <param name="data">The data<see cref="byte[]"/>.</param>
        /// <param name="completed">The completed<see cref="Action"/>.</param>
        private void broadcastAsync(Opcode opcode, byte[] data, Action completed)
        {
            ThreadPool.QueueUserWorkItem(
              state => broadcast(opcode, data, completed)
            );
        }

        /// <summary>
        /// The broadcastAsync.
        /// </summary>
        /// <param name="opcode">The opcode<see cref="Opcode"/>.</param>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="completed">The completed<see cref="Action"/>.</param>
        private void broadcastAsync(Opcode opcode, Stream stream, Action completed)
        {
            ThreadPool.QueueUserWorkItem(
              state => broadcast(opcode, stream, completed)
            );
        }

        /// <summary>
        /// The broadping.
        /// </summary>
        /// <param name="frameAsBytes">The frameAsBytes<see cref="byte[]"/>.</param>
        /// <returns>The <see cref="Dictionary{string, bool}"/>.</returns>
        private Dictionary<string, bool> broadping(byte[] frameAsBytes)
        {
            var ret = new Dictionary<string, bool>();

            foreach (var session in Sessions)
            {
                if (_state != ServerState.Start)
                {
                    _log.Error("The service is shutting down.");
                    break;
                }

                var res = session.Context.WebSocket.Ping(frameAsBytes, _waitTime);
                ret.Add(session.ID, res);
            }

            return ret;
        }

        /// <summary>
        /// The canSet.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool canSet(out string message)
        {
            message = null;

            if (_state == ServerState.Start)
            {
                message = "The service has already started.";
                return false;
            }

            if (_state == ServerState.ShuttingDown)
            {
                message = "The service is shutting down.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// The createID.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        private static string createID()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// The setSweepTimer.
        /// </summary>
        /// <param name="interval">The interval<see cref="double"/>.</param>
        private void setSweepTimer(double interval)
        {
            _sweepTimer = new System.Timers.Timer(interval);
            _sweepTimer.Elapsed += (sender, e) => Sweep();
        }

        /// <summary>
        /// The stop.
        /// </summary>
        /// <param name="payloadData">The payloadData<see cref="PayloadData"/>.</param>
        /// <param name="send">The send<see cref="bool"/>.</param>
        private void stop(PayloadData payloadData, bool send)
        {
            var bytes = send
                        ? WebSocketFrame.CreateCloseFrame(payloadData, false).ToArray()
                        : null;

            lock (_sync)
            {
                _state = ServerState.ShuttingDown;

                _sweepTimer.Enabled = false;
                foreach (var session in _sessions.Values.ToList())
                    session.Context.WebSocket.Close(payloadData, bytes);

                _state = ServerState.Stop;
            }
        }

        /// <summary>
        /// The tryGetSession.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <param name="session">The session<see cref="IWebSocketSession"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool tryGetSession(string id, out IWebSocketSession session)
        {
            session = null;

            if (_state != ServerState.Start)
                return false;

            lock (_sync)
            {
                if (_state != ServerState.Start)
                    return false;

                return _sessions.TryGetValue(id, out session);
            }
        }

        /// <summary>
        /// The Add.
        /// </summary>
        /// <param name="session">The session<see cref="IWebSocketSession"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal string Add(IWebSocketSession session)
        {
            lock (_sync)
            {
                if (_state != ServerState.Start)
                    return null;

                var id = createID();
                _sessions.Add(id, session);

                return id;
            }
        }

        /// <summary>
        /// The Broadcast.
        /// </summary>
        /// <param name="opcode">The opcode<see cref="Opcode"/>.</param>
        /// <param name="data">The data<see cref="byte[]"/>.</param>
        /// <param name="cache">The cache<see cref="Dictionary{CompressionMethod, byte[]}"/>.</param>
        internal void Broadcast(
      Opcode opcode, byte[] data, Dictionary<CompressionMethod, byte[]> cache
    )
        {
            foreach (var session in Sessions)
            {
                if (_state != ServerState.Start)
                {
                    _log.Error("The service is shutting down.");
                    break;
                }

                session.Context.WebSocket.Send(opcode, data, cache);
            }
        }

        /// <summary>
        /// The Broadcast.
        /// </summary>
        /// <param name="opcode">The opcode<see cref="Opcode"/>.</param>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="cache">The cache<see cref="Dictionary {CompressionMethod, Stream}"/>.</param>
        internal void Broadcast(
      Opcode opcode, Stream stream, Dictionary<CompressionMethod, Stream> cache
    )
        {
            foreach (var session in Sessions)
            {
                if (_state != ServerState.Start)
                {
                    _log.Error("The service is shutting down.");
                    break;
                }

                session.Context.WebSocket.Send(opcode, stream, cache);
            }
        }

        /// <summary>
        /// The Broadping.
        /// </summary>
        /// <param name="frameAsBytes">The frameAsBytes<see cref="byte[]"/>.</param>
        /// <param name="timeout">The timeout<see cref="TimeSpan"/>.</param>
        /// <returns>The <see cref="Dictionary{string, bool}"/>.</returns>
        internal Dictionary<string, bool> Broadping(
      byte[] frameAsBytes, TimeSpan timeout
    )
        {
            var ret = new Dictionary<string, bool>();

            foreach (var session in Sessions)
            {
                if (_state != ServerState.Start)
                {
                    _log.Error("The service is shutting down.");
                    break;
                }

                var res = session.Context.WebSocket.Ping(frameAsBytes, timeout);
                ret.Add(session.ID, res);
            }

            return ret;
        }

        /// <summary>
        /// The Remove.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal bool Remove(string id)
        {
            lock (_sync)
                return _sessions.Remove(id);
        }

        /// <summary>
        /// The Start.
        /// </summary>
        internal void Start()
        {
            lock (_sync)
            {
                _sweepTimer.Enabled = _clean;
                _state = ServerState.Start;
            }
        }

        /// <summary>
        /// The Stop.
        /// </summary>
        /// <param name="code">The code<see cref="ushort"/>.</param>
        /// <param name="reason">The reason<see cref="string"/>.</param>
        internal void Stop(ushort code, string reason)
        {
            if (code == 1005)
            { // == no status
                stop(PayloadData.Empty, true);
                return;
            }

            stop(new PayloadData(code, reason), !code.IsReserved());
        }

        /// <summary>
        /// Sends <paramref name="data"/> to every client in the WebSocket service.
        /// </summary>
        /// <param name="data">The data<see cref="byte[]"/>.</param>
        public void Broadcast(byte[] data)
        {
            if (_state != ServerState.Start)
            {
                var msg = "The current state of the manager is not Start.";
                throw new InvalidOperationException(msg);
            }

            if (data == null)
                throw new ArgumentNullException("data");

            if (data.LongLength <= WebSocket.FragmentLength)
                broadcast(Opcode.Binary, data, null);
            else
                broadcast(Opcode.Binary, new MemoryStream(data), null);
        }

        /// <summary>
        /// Sends <paramref name="data"/> to every client in the WebSocket service.
        /// </summary>
        /// <param name="data">The data<see cref="string"/>.</param>
        public void Broadcast(string data)
        {
            if (_state != ServerState.Start)
            {
                var msg = "The current state of the manager is not Start.";
                throw new InvalidOperationException(msg);
            }

            if (data == null)
                throw new ArgumentNullException("data");

            byte[] bytes;
            if (!data.TryGetUTF8EncodedBytes(out bytes))
            {
                var msg = "It could not be UTF-8-encoded.";
                throw new ArgumentException(msg, "data");
            }

            if (bytes.LongLength <= WebSocket.FragmentLength)
                broadcast(Opcode.Text, bytes, null);
            else
                broadcast(Opcode.Text, new MemoryStream(bytes), null);
        }

        /// <summary>
        /// Sends the data from <paramref name="stream"/> to every client in
        /// the WebSocket service.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        public void Broadcast(Stream stream, int length)
        {
            if (_state != ServerState.Start)
            {
                var msg = "The current state of the manager is not Start.";
                throw new InvalidOperationException(msg);
            }

            if (stream == null)
                throw new ArgumentNullException("stream");

            if (!stream.CanRead)
            {
                var msg = "It cannot be read.";
                throw new ArgumentException(msg, "stream");
            }

            if (length < 1)
            {
                var msg = "Less than 1.";
                throw new ArgumentException(msg, "length");
            }

            var bytes = stream.ReadBytes(length);

            var len = bytes.Length;
            if (len == 0)
            {
                var msg = "No data could be read from it.";
                throw new ArgumentException(msg, "stream");
            }

            if (len < length)
            {
                _log.Warn(
                  String.Format(
                    "Only {0} byte(s) of data could be read from the stream.",
                    len
                  )
                );
            }

            if (len <= WebSocket.FragmentLength)
                broadcast(Opcode.Binary, bytes, null);
            else
                broadcast(Opcode.Binary, new MemoryStream(bytes), null);
        }

        /// <summary>
        /// Sends <paramref name="data"/> asynchronously to every client in
        /// the WebSocket service.
        /// </summary>
        /// <param name="data">The data<see cref="byte[]"/>.</param>
        /// <param name="completed">The completed<see cref="Action"/>.</param>
        public void BroadcastAsync(byte[] data, Action completed)
        {
            if (_state != ServerState.Start)
            {
                var msg = "The current state of the manager is not Start.";
                throw new InvalidOperationException(msg);
            }

            if (data == null)
                throw new ArgumentNullException("data");

            if (data.LongLength <= WebSocket.FragmentLength)
                broadcastAsync(Opcode.Binary, data, completed);
            else
                broadcastAsync(Opcode.Binary, new MemoryStream(data), completed);
        }

        /// <summary>
        /// Sends <paramref name="data"/> asynchronously to every client in
        /// the WebSocket service.
        /// </summary>
        /// <param name="data">The data<see cref="string"/>.</param>
        /// <param name="completed">The completed<see cref="Action"/>.</param>
        public void BroadcastAsync(string data, Action completed)
        {
            if (_state != ServerState.Start)
            {
                var msg = "The current state of the manager is not Start.";
                throw new InvalidOperationException(msg);
            }

            if (data == null)
                throw new ArgumentNullException("data");

            byte[] bytes;
            if (!data.TryGetUTF8EncodedBytes(out bytes))
            {
                var msg = "It could not be UTF-8-encoded.";
                throw new ArgumentException(msg, "data");
            }

            if (bytes.LongLength <= WebSocket.FragmentLength)
                broadcastAsync(Opcode.Text, bytes, completed);
            else
                broadcastAsync(Opcode.Text, new MemoryStream(bytes), completed);
        }

        /// <summary>
        /// Sends the data from <paramref name="stream"/> asynchronously to
        /// every client in the WebSocket service.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="completed">The completed<see cref="Action"/>.</param>
        public void BroadcastAsync(Stream stream, int length, Action completed)
        {
            if (_state != ServerState.Start)
            {
                var msg = "The current state of the manager is not Start.";
                throw new InvalidOperationException(msg);
            }

            if (stream == null)
                throw new ArgumentNullException("stream");

            if (!stream.CanRead)
            {
                var msg = "It cannot be read.";
                throw new ArgumentException(msg, "stream");
            }

            if (length < 1)
            {
                var msg = "Less than 1.";
                throw new ArgumentException(msg, "length");
            }

            var bytes = stream.ReadBytes(length);

            var len = bytes.Length;
            if (len == 0)
            {
                var msg = "No data could be read from it.";
                throw new ArgumentException(msg, "stream");
            }

            if (len < length)
            {
                _log.Warn(
                  String.Format(
                    "Only {0} byte(s) of data could be read from the stream.",
                    len
                  )
                );
            }

            if (len <= WebSocket.FragmentLength)
                broadcastAsync(Opcode.Binary, bytes, completed);
            else
                broadcastAsync(Opcode.Binary, new MemoryStream(bytes), completed);
        }

        /// <summary>
        /// Sends a ping to every client in the WebSocket service.
        /// </summary>
        /// <returns>The <see cref="Dictionary{string, bool}"/>.</returns>
        [Obsolete("This method will be removed.")]
        public Dictionary<string, bool> Broadping()
        {
            if (_state != ServerState.Start)
            {
                var msg = "The current state of the manager is not Start.";
                throw new InvalidOperationException(msg);
            }

            return Broadping(WebSocketFrame.EmptyPingBytes, _waitTime);
        }

        /// <summary>
        /// Sends a ping with <paramref name="message"/> to every client in
        /// the WebSocket service.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <returns>The <see cref="Dictionary{string, bool}"/>.</returns>
        [Obsolete("This method will be removed.")]
        public Dictionary<string, bool> Broadping(string message)
        {
            if (_state != ServerState.Start)
            {
                var msg = "The current state of the manager is not Start.";
                throw new InvalidOperationException(msg);
            }

            if (message.IsNullOrEmpty())
                return Broadping(WebSocketFrame.EmptyPingBytes, _waitTime);

            byte[] bytes;
            if (!message.TryGetUTF8EncodedBytes(out bytes))
            {
                var msg = "It could not be UTF-8-encoded.";
                throw new ArgumentException(msg, "message");
            }

            if (bytes.Length > 125)
            {
                var msg = "Its size is greater than 125 bytes.";
                throw new ArgumentOutOfRangeException("message", msg);
            }

            var frame = WebSocketFrame.CreatePingFrame(bytes, false);
            return Broadping(frame.ToArray(), _waitTime);
        }

        /// <summary>
        /// Closes the specified session.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        public void CloseSession(string id)
        {
            IWebSocketSession session;
            if (!TryGetSession(id, out session))
            {
                var msg = "The session could not be found.";
                throw new InvalidOperationException(msg);
            }

            session.Context.WebSocket.Close();
        }

        /// <summary>
        /// Closes the specified session with <paramref name="code"/> and
        /// <paramref name="reason"/>.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <param name="code">The code<see cref="ushort"/>.</param>
        /// <param name="reason">The reason<see cref="string"/>.</param>
        public void CloseSession(string id, ushort code, string reason)
        {
            IWebSocketSession session;
            if (!TryGetSession(id, out session))
            {
                var msg = "The session could not be found.";
                throw new InvalidOperationException(msg);
            }

            session.Context.WebSocket.Close(code, reason);
        }

        /// <summary>
        /// Closes the specified session with <paramref name="code"/> and
        /// <paramref name="reason"/>.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <param name="code">The code<see cref="CloseStatusCode"/>.</param>
        /// <param name="reason">The reason<see cref="string"/>.</param>
        public void CloseSession(string id, CloseStatusCode code, string reason)
        {
            IWebSocketSession session;
            if (!TryGetSession(id, out session))
            {
                var msg = "The session could not be found.";
                throw new InvalidOperationException(msg);
            }

            session.Context.WebSocket.Close(code, reason);
        }

        /// <summary>
        /// Sends a ping to the client using the specified session.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool PingTo(string id)
        {
            IWebSocketSession session;
            if (!TryGetSession(id, out session))
            {
                var msg = "The session could not be found.";
                throw new InvalidOperationException(msg);
            }

            return session.Context.WebSocket.Ping();
        }

        /// <summary>
        /// Sends a ping with <paramref name="message"/> to the client using
        /// the specified session.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool PingTo(string message, string id)
        {
            IWebSocketSession session;
            if (!TryGetSession(id, out session))
            {
                var msg = "The session could not be found.";
                throw new InvalidOperationException(msg);
            }

            return session.Context.WebSocket.Ping(message);
        }

        /// <summary>
        /// Sends <paramref name="data"/> to the client using the specified session.
        /// </summary>
        /// <param name="data">The data<see cref="byte[]"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        public void SendTo(byte[] data, string id)
        {
            IWebSocketSession session;
            if (!TryGetSession(id, out session))
            {
                var msg = "The session could not be found.";
                throw new InvalidOperationException(msg);
            }

            session.Context.WebSocket.Send(data);
        }

        /// <summary>
        /// Sends <paramref name="data"/> to the client using the specified session.
        /// </summary>
        /// <param name="data">The data<see cref="string"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        public void SendTo(string data, string id)
        {
            IWebSocketSession session;
            if (!TryGetSession(id, out session))
            {
                var msg = "The session could not be found.";
                throw new InvalidOperationException(msg);
            }

            session.Context.WebSocket.Send(data);
        }

        /// <summary>
        /// Sends the data from <paramref name="stream"/> to the client using
        /// the specified session.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        public void SendTo(Stream stream, int length, string id)
        {
            IWebSocketSession session;
            if (!TryGetSession(id, out session))
            {
                var msg = "The session could not be found.";
                throw new InvalidOperationException(msg);
            }

            session.Context.WebSocket.Send(stream, length);
        }

        /// <summary>
        /// Sends <paramref name="data"/> asynchronously to the client using
        /// the specified session.
        /// </summary>
        /// <param name="data">The data<see cref="byte[]"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <param name="completed">The completed<see cref="Action{bool}"/>.</param>
        public void SendToAsync(byte[] data, string id, Action<bool> completed)
        {
            IWebSocketSession session;
            if (!TryGetSession(id, out session))
            {
                var msg = "The session could not be found.";
                throw new InvalidOperationException(msg);
            }

            session.Context.WebSocket.SendAsync(data, completed);
        }

        /// <summary>
        /// Sends <paramref name="data"/> asynchronously to the client using
        /// the specified session.
        /// </summary>
        /// <param name="data">The data<see cref="string"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <param name="completed">The completed<see cref="Action{bool}"/>.</param>
        public void SendToAsync(string data, string id, Action<bool> completed)
        {
            IWebSocketSession session;
            if (!TryGetSession(id, out session))
            {
                var msg = "The session could not be found.";
                throw new InvalidOperationException(msg);
            }

            session.Context.WebSocket.SendAsync(data, completed);
        }

        /// <summary>
        /// Sends the data from <paramref name="stream"/> asynchronously to
        /// the client using the specified session.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <param name="completed">The completed<see cref="Action{bool}"/>.</param>
        public void SendToAsync(
      Stream stream, int length, string id, Action<bool> completed
    )
        {
            IWebSocketSession session;
            if (!TryGetSession(id, out session))
            {
                var msg = "The session could not be found.";
                throw new InvalidOperationException(msg);
            }

            session.Context.WebSocket.SendAsync(stream, length, completed);
        }

        /// <summary>
        /// Cleans up the inactive sessions in the WebSocket service.
        /// </summary>
        public void Sweep()
        {
            if (_sweeping)
            {
                _log.Info("The sweeping is already in progress.");
                return;
            }

            lock (_forSweep)
            {
                if (_sweeping)
                {
                    _log.Info("The sweeping is already in progress.");
                    return;
                }

                _sweeping = true;
            }

            foreach (var id in InactiveIDs)
            {
                if (_state != ServerState.Start)
                    break;

                lock (_sync)
                {
                    if (_state != ServerState.Start)
                        break;

                    IWebSocketSession session;
                    if (_sessions.TryGetValue(id, out session))
                    {
                        var state = session.ConnectionState;
                        if (state == WebSocketState.Open)
                            session.Context.WebSocket.Close(CloseStatusCode.Abnormal);
                        else if (state == WebSocketState.Closing)
                            continue;
                        else
                            _sessions.Remove(id);
                    }
                }
            }

            _sweeping = false;
        }

        /// <summary>
        /// Tries to get the session instance with <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <param name="session">The session<see cref="IWebSocketSession"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool TryGetSession(string id, out IWebSocketSession session)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            if (id.Length == 0)
                throw new ArgumentException("An empty string.", "id");

            return tryGetSession(id, out session);
        }
    }
}
