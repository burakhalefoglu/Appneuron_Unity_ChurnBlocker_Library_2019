/*
 * WebSocketServiceManager.cs
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
    using System.Threading;

    /// <summary>
    /// Provides the management function for the WebSocket services.
    /// </summary>
    public class WebSocketServiceManager
    {
        /// <summary>
        /// Defines the _clean.
        /// </summary>
        private volatile bool _clean;

        /// <summary>
        /// Defines the _hosts.
        /// </summary>
        private Dictionary<string, WebSocketServiceHost> _hosts;

        /// <summary>
        /// Defines the _log.
        /// </summary>
        private Logger _log;

        /// <summary>
        /// Defines the _state.
        /// </summary>
        private volatile ServerState _state;

        /// <summary>
        /// Defines the _sync.
        /// </summary>
        private object _sync;

        /// <summary>
        /// Defines the _waitTime.
        /// </summary>
        private TimeSpan _waitTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketServiceManager"/> class.
        /// </summary>
        /// <param name="log">The log<see cref="Logger"/>.</param>
        internal WebSocketServiceManager(Logger log)
        {
            _log = log;

            _clean = true;
            _hosts = new Dictionary<string, WebSocketServiceHost>();
            _state = ServerState.Ready;
            _sync = ((ICollection)_hosts).SyncRoot;
            _waitTime = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Gets the number of the WebSocket services....
        /// </summary>
        public int Count
        {
            get
            {
                lock (_sync)
                    return _hosts.Count;
            }
        }

        /// <summary>
        /// Gets the host instances for the WebSocket services....
        /// </summary>
        public IEnumerable<WebSocketServiceHost> Hosts
        {
            get
            {
                lock (_sync)
                    return _hosts.Values.ToList();
            }
        }


        /// <summary>
        /// Gets the host instance for a WebSocket service with the specified path.
        /// </summary>
        /// <value>
        ///   <para>
        ///   A <see cref="WebSocketServiceHost"/> instance or
        ///   <see langword="null"/> if not found.
        ///   </para>
        ///   <para>
        ///   The host instance provides the function to access
        ///   the information in the service.
        ///   </para>
        /// </value>
        /// <param name="path">
        ///   <para>
        ///   A <see cref="string"/> that represents an absolute path to
        ///   the service to find.
        ///   </para>
        ///   <para>
        ///   / is trimmed from the end of the string if present.
        ///   </para>
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   <para>
        ///   <paramref name="path"/> is empty.
        ///   </para>
        ///   <para>
        ///   -or-
        ///   </para>
        ///   <para>
        ///   <paramref name="path"/> is not an absolute path.
        ///   </para>
        ///   <para>
        ///   -or-
        ///   </para>
        ///   <para>
        ///   <paramref name="path"/> includes either or both
        ///   query and fragment components.
        ///   </para>
        /// </exception>
        public WebSocketServiceHost this[string path]
        {
            get
            {
                if (path == null)
                    throw new ArgumentNullException("path");

                if (path.Length == 0)
                    throw new ArgumentException("An empty string.", "path");

                if (path[0] != '/')
                    throw new ArgumentException("Not an absolute path.", "path");

                if (path.IndexOfAny(new[] { '?', '#' }) > -1)
                {
                    var msg = "It includes either or both query and fragment components.";
                    throw new ArgumentException(msg, "path");
                }

                WebSocketServiceHost host;
                InternalTryGetServiceHost(path, out host);

                return host;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether the inactive sessions in
        /// the WebSocket services are cleaned up periodically....
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

                    foreach (var host in _hosts.Values)
                        host.KeepClean = value;

                    _clean = value;
                }
            }
        }

        /// <summary>
        /// Gets the paths for the WebSocket services....
        /// </summary>
        public IEnumerable<string> Paths
        {
            get
            {
                lock (_sync)
                    return _hosts.Keys.ToList();
            }
        }

        /// <summary>
        /// Gets the total number of the sessions in the WebSocket services....
        /// </summary>
        [Obsolete("This property will be removed.")]
        public int SessionCount
        {
            get
            {
                var cnt = 0;
                foreach (var host in Hosts)
                {
                    if (_state != ServerState.Start)
                        break;

                    cnt += host.Sessions.Count;
                }

                return cnt;
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

                    foreach (var host in _hosts.Values)
                        host.WaitTime = value;

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
                foreach (var host in Hosts)
                {
                    if (_state != ServerState.Start)
                    {
                        _log.Error("The server is shutting down.");
                        break;
                    }

                    host.Sessions.Broadcast(opcode, data, cache);
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
                foreach (var host in Hosts)
                {
                    if (_state != ServerState.Start)
                    {
                        _log.Error("The server is shutting down.");
                        break;
                    }

                    host.Sessions.Broadcast(opcode, stream, cache);
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
        /// <param name="timeout">The timeout<see cref="TimeSpan"/>.</param>
        /// <returns>The <see cref="Dictionary{string, Dictionary{string, bool}}"/>.</returns>
        private Dictionary<string, Dictionary<string, bool>> broadping(
      byte[] frameAsBytes, TimeSpan timeout
    )
        {
            var ret = new Dictionary<string, Dictionary<string, bool>>();

            foreach (var host in Hosts)
            {
                if (_state != ServerState.Start)
                {
                    _log.Error("The server is shutting down.");
                    break;
                }

                var res = host.Sessions.Broadping(frameAsBytes, timeout);
                ret.Add(host.Path, res);
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
                message = "The server has already started.";
                return false;
            }

            if (_state == ServerState.ShuttingDown)
            {
                message = "The server is shutting down.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// The Add.
        /// </summary>
        /// <typeparam name="TBehavior">.</typeparam>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="creator">The creator<see cref="Func{TBehavior}"/>.</param>
        internal void Add<TBehavior>(string path, Func<TBehavior> creator)
      where TBehavior : WebSocketBehavior
        {
            path = path.TrimSlashFromEnd();

            lock (_sync)
            {
                WebSocketServiceHost host;
                if (_hosts.TryGetValue(path, out host))
                    throw new ArgumentException("Already in use.", "path");

                host = new WebSocketServiceHost<TBehavior>(
                         path, creator, null, _log
                       );

                if (!_clean)
                    host.KeepClean = false;

                if (_waitTime != host.WaitTime)
                    host.WaitTime = _waitTime;

                if (_state == ServerState.Start)
                    host.Start();

                _hosts.Add(path, host);
            }
        }

        /// <summary>
        /// The InternalTryGetServiceHost.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="host">The host<see cref="WebSocketServiceHost"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal bool InternalTryGetServiceHost(
      string path, out WebSocketServiceHost host
    )
        {
            path = path.TrimSlashFromEnd();

            lock (_sync)
                return _hosts.TryGetValue(path, out host);
        }

        /// <summary>
        /// The Start.
        /// </summary>
        internal void Start()
        {
            lock (_sync)
            {
                foreach (var host in _hosts.Values)
                    host.Start();

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
            lock (_sync)
            {
                _state = ServerState.ShuttingDown;

                foreach (var host in _hosts.Values)
                    host.Stop(code, reason);

                _state = ServerState.Stop;
            }
        }

        /// <summary>
        /// Adds a WebSocket service with the specified behavior, path,
        /// and delegate.
        /// </summary>
        /// <typeparam name="TBehavior">.</typeparam>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="initializer">The initializer<see cref="Action{TBehavior}"/>.</param>
        public void AddService<TBehavior>(
      string path, Action<TBehavior> initializer
    )
      where TBehavior : WebSocketBehavior, new()
        {
            if (path == null)
                throw new ArgumentNullException("path");

            if (path.Length == 0)
                throw new ArgumentException("An empty string.", "path");

            if (path[0] != '/')
                throw new ArgumentException("Not an absolute path.", "path");

            if (path.IndexOfAny(new[] { '?', '#' }) > -1)
            {
                var msg = "It includes either or both query and fragment components.";
                throw new ArgumentException(msg, "path");
            }

            path = path.TrimSlashFromEnd();

            lock (_sync)
            {
                WebSocketServiceHost host;
                if (_hosts.TryGetValue(path, out host))
                    throw new ArgumentException("Already in use.", "path");

                host = new WebSocketServiceHost<TBehavior>(
                         path, () => new TBehavior(), initializer, _log
                       );

                if (!_clean)
                    host.KeepClean = false;

                if (_waitTime != host.WaitTime)
                    host.WaitTime = _waitTime;

                if (_state == ServerState.Start)
                    host.Start();

                _hosts.Add(path, host);
            }
        }

        /// <summary>
        /// Sends <paramref name="data"/> to every client in the WebSocket services.
        /// </summary>
        /// <param name="data">The data<see cref="byte[]"/>.</param>
        [Obsolete("This method will be removed.")]
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
        /// Sends <paramref name="data"/> to every client in the WebSocket services.
        /// </summary>
        /// <param name="data">The data<see cref="string"/>.</param>
        [Obsolete("This method will be removed.")]
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
        /// Sends <paramref name="data"/> asynchronously to every client in
        /// the WebSocket services.
        /// </summary>
        /// <param name="data">The data<see cref="byte[]"/>.</param>
        /// <param name="completed">The completed<see cref="Action"/>.</param>
        [Obsolete("This method will be removed.")]
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
        /// the WebSocket services.
        /// </summary>
        /// <param name="data">The data<see cref="string"/>.</param>
        /// <param name="completed">The completed<see cref="Action"/>.</param>
        [Obsolete("This method will be removed.")]
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
        /// every client in the WebSocket services.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <param name="completed">The completed<see cref="Action"/>.</param>
        [Obsolete("This method will be removed.")]
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
        /// Sends a ping to every client in the WebSocket services.
        /// </summary>
        /// <returns>The <see cref="Dictionary{string, Dictionary{string, bool}}"/>.</returns>
        [Obsolete("This method will be removed.")]
        public Dictionary<string, Dictionary<string, bool>> Broadping()
        {
            if (_state != ServerState.Start)
            {
                var msg = "The current state of the manager is not Start.";
                throw new InvalidOperationException(msg);
            }

            return broadping(WebSocketFrame.EmptyPingBytes, _waitTime);
        }

        /// <summary>
        /// Sends a ping with <paramref name="message"/> to every client in
        /// the WebSocket services.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <returns>The <see cref="Dictionary{string, Dictionary{string, bool}}"/>.</returns>
        [Obsolete("This method will be removed.")]
        public Dictionary<string, Dictionary<string, bool>> Broadping(string message)
        {
            if (_state != ServerState.Start)
            {
                var msg = "The current state of the manager is not Start.";
                throw new InvalidOperationException(msg);
            }

            if (message.IsNullOrEmpty())
                return broadping(WebSocketFrame.EmptyPingBytes, _waitTime);

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
            return broadping(frame.ToArray(), _waitTime);
        }

        /// <summary>
        /// Removes all WebSocket services managed by the manager.
        /// </summary>
        public void Clear()
        {
            List<WebSocketServiceHost> hosts = null;

            lock (_sync)
            {
                hosts = _hosts.Values.ToList();
                _hosts.Clear();
            }

            foreach (var host in hosts)
            {
                if (host.State == ServerState.Start)
                    host.Stop(1001, String.Empty);
            }
        }

        /// <summary>
        /// Removes a WebSocket service with the specified path.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool RemoveService(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            if (path.Length == 0)
                throw new ArgumentException("An empty string.", "path");

            if (path[0] != '/')
                throw new ArgumentException("Not an absolute path.", "path");

            if (path.IndexOfAny(new[] { '?', '#' }) > -1)
            {
                var msg = "It includes either or both query and fragment components.";
                throw new ArgumentException(msg, "path");
            }

            path = path.TrimSlashFromEnd();

            WebSocketServiceHost host;
            lock (_sync)
            {
                if (!_hosts.TryGetValue(path, out host))
                    return false;

                _hosts.Remove(path);
            }

            if (host.State == ServerState.Start)
                host.Stop(1001, String.Empty);

            return true;
        }

        /// <summary>
        /// Tries to get the host instance for a WebSocket service with
        /// the specified path.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="host">The host<see cref="WebSocketServiceHost"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool TryGetServiceHost(string path, out WebSocketServiceHost host)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            if (path.Length == 0)
                throw new ArgumentException("An empty string.", "path");

            if (path[0] != '/')
                throw new ArgumentException("Not an absolute path.", "path");

            if (path.IndexOfAny(new[] { '?', '#' }) > -1)
            {
                var msg = "It includes either or both query and fragment components.";
                throw new ArgumentException(msg, "path");
            }

            return InternalTryGetServiceHost(path, out host);
        }
    }
}
