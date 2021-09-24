/*
 * Ext.cs
 *
 * Some parts of this code are derived from Mono (http://www.mono-project.com):
 * - GetStatusDescription is derived from HttpListenerResponse.cs (System.Net)
 * - IsPredefinedScheme is derived from Uri.cs (System)
 * - MaybeUri is derived from Uri.cs (System)
 *
 * The MIT License
 *
 * Copyright (c) 2001 Garrett Rooney
 * Copyright (c) 2003 Ian MacLean
 * Copyright (c) 2003 Ben Maurer
 * Copyright (c) 2003, 2005, 2009 Novell, Inc. (http://www.novell.com)
 * Copyright (c) 2009 Stephane Delcroix
 * Copyright (c) 2010-2016 sta.blockhead
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
 * Contributors:
 * - Liryna <liryna.stark@gmail.com>
 * - Nikola Kovacevic <nikolak@outlook.com>
 * - Chris Swiedler
 */

namespace AppneuronUnity.Core.Libraries.WebSocket
{
    using AppneuronUnity.Core.Libraries.WebSocket.Net;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.IO.Compression;
    using System.Net.Sockets;
    using System.Text;

    /// <summary>
    /// Provides a set of static methods for websocket-sharp.
    /// </summary>
    public static class Ext
    {
        /// <summary>
        /// Defines the _last.
        /// </summary>
        private static readonly byte[] _last = new byte[] { 0x00 };

        /// <summary>
        /// Defines the _retry.
        /// </summary>
        private static readonly int _retry = 5;

        /// <summary>
        /// Defines the _tspecials.
        /// </summary>
        private const string _tspecials = "()<>@,;:\\\"/[]?={} \t";

        /// <summary>
        /// The compress.
        /// </summary>
        /// <param name="data">The data<see cref="byte[]"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        private static byte[] compress(this byte[] data)
        {
            if (data.LongLength == 0)
                //return new byte[] { 0x00, 0x00, 0x00, 0xff, 0xff };
                return data;

            using (var input = new MemoryStream(data))
                return input.compressToArray();
        }

        /// <summary>
        /// The compress.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <returns>The <see cref="MemoryStream"/>.</returns>
        private static MemoryStream compress(this Stream stream)
        {
            var output = new MemoryStream();
            if (stream.Length == 0)
                return output;

            stream.Position = 0;
            using (var ds = new DeflateStream(output, CompressionMode.Compress, true))
            {
                stream.CopyTo(ds, 1024);
                ds.Close(); // BFINAL set to 1.
                output.Write(_last, 0, 1);
                output.Position = 0;

                return output;
            }
        }

        /// <summary>
        /// The compressToArray.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        private static byte[] compressToArray(this Stream stream)
        {
            using (var output = stream.compress())
            {
                output.Close();
                return output.ToArray();
            }
        }

        /// <summary>
        /// The decompress.
        /// </summary>
        /// <param name="data">The data<see cref="byte[]"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        private static byte[] decompress(this byte[] data)
        {
            if (data.LongLength == 0)
                return data;

            using (var input = new MemoryStream(data))
                return input.decompressToArray();
        }

        /// <summary>
        /// The decompress.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <returns>The <see cref="MemoryStream"/>.</returns>
        private static MemoryStream decompress(this Stream stream)
        {
            var output = new MemoryStream();
            if (stream.Length == 0)
                return output;

            stream.Position = 0;
            using (var ds = new DeflateStream(stream, CompressionMode.Decompress, true))
            {
                ds.CopyTo(output, 1024);
                output.Position = 0;

                return output;
            }
        }

        /// <summary>
        /// The decompressToArray.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        private static byte[] decompressToArray(this Stream stream)
        {
            using (var output = stream.decompress())
            {
                output.Close();
                return output.ToArray();
            }
        }

        /// <summary>
        /// The isHttpMethod.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private static bool isHttpMethod(this string value)
        {
            return value == "GET"
                   || value == "HEAD"
                   || value == "POST"
                   || value == "PUT"
                   || value == "DELETE"
                   || value == "CONNECT"
                   || value == "OPTIONS"
                   || value == "TRACE";
        }

        /// <summary>
        /// The isHttpMethod10.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private static bool isHttpMethod10(this string value)
        {
            return value == "GET"
                   || value == "HEAD"
                   || value == "POST";
        }

        /// <summary>
        /// The Append.
        /// </summary>
        /// <param name="code">The code<see cref="ushort"/>.</param>
        /// <param name="reason">The reason<see cref="string"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        internal static byte[] Append(this ushort code, string reason)
        {
            var bytes = code.InternalToByteArray(ByteOrder.Big);

            if (reason == null || reason.Length == 0)
                return bytes;

            var buff = new List<byte>(bytes);
            buff.AddRange(Encoding.UTF8.GetBytes(reason));

            return buff.ToArray();
        }

        /// <summary>
        /// The Close.
        /// </summary>
        /// <param name="response">The response<see cref="HttpListenerResponse"/>.</param>
        /// <param name="code">The code<see cref="HttpStatusCode"/>.</param>
        internal static void Close(
      this HttpListenerResponse response, HttpStatusCode code
    )
        {
            response.StatusCode = (int)code;
            response.OutputStream.Close();
        }

        /// <summary>
        /// The CloseWithAuthChallenge.
        /// </summary>
        /// <param name="response">The response<see cref="HttpListenerResponse"/>.</param>
        /// <param name="challenge">The challenge<see cref="string"/>.</param>
        internal static void CloseWithAuthChallenge(
      this HttpListenerResponse response, string challenge
    )
        {
            response.Headers.InternalSet("WWW-Authenticate", challenge, true);
            response.Close(HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// The Compress.
        /// </summary>
        /// <param name="data">The data<see cref="byte[]"/>.</param>
        /// <param name="method">The method<see cref="CompressionMethod"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        internal static byte[] Compress(this byte[] data, CompressionMethod method)
        {
            return method == CompressionMethod.Deflate
                   ? data.compress()
                   : data;
        }

        /// <summary>
        /// The Compress.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="method">The method<see cref="CompressionMethod"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        internal static Stream Compress(this Stream stream, CompressionMethod method)
        {
            return method == CompressionMethod.Deflate
                   ? stream.compress()
                   : stream;
        }

        /// <summary>
        /// The CompressToArray.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="method">The method<see cref="CompressionMethod"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        internal static byte[] CompressToArray(this Stream stream, CompressionMethod method)
        {
            return method == CompressionMethod.Deflate
                   ? stream.compressToArray()
                   : stream.ToByteArray();
        }

        /// <summary>
        /// Determines whether the specified string contains any of characters in
        /// the specified array of <see cref="char"/>.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <param name="anyOf">The anyOf<see cref="char[]"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool Contains(this string value, params char[] anyOf)
        {
            return anyOf != null && anyOf.Length > 0
                   ? value.IndexOfAny(anyOf) > -1
                   : false;
        }

        /// <summary>
        /// The Contains.
        /// </summary>
        /// <param name="collection">The collection<see cref="NameValueCollection"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool Contains(
      this NameValueCollection collection, string name
    )
        {
            return collection[name] != null;
        }

        /// <summary>
        /// The Contains.
        /// </summary>
        /// <param name="collection">The collection<see cref="NameValueCollection"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <param name="comparisonTypeForValue">The comparisonTypeForValue<see cref="StringComparison"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool Contains(
      this NameValueCollection collection,
      string name,
      string value,
      StringComparison comparisonTypeForValue
    )
        {
            var val = collection[name];
            if (val == null)
                return false;

            foreach (var elm in val.Split(','))
            {
                if (elm.Trim().Equals(value, comparisonTypeForValue))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// The Contains.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="source">The source<see cref="IEnumerable{T}"/>.</param>
        /// <param name="condition">The condition<see cref="Func{T, bool}"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool Contains<T>(
      this IEnumerable<T> source, Func<T, bool> condition
    )
        {
            foreach (T elm in source)
            {
                if (condition(elm))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// The ContainsTwice.
        /// </summary>
        /// <param name="values">The values<see cref="string[]"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool ContainsTwice(this string[] values)
        {
            var len = values.Length;
            var end = len - 1;

            Func<int, bool> seek = null;
            seek = idx =>
            {
                if (idx == end)
                    return false;

                var val = values[idx];
                for (var i = idx + 1; i < len; i++)
                {
                    if (values[i] == val)
                        return true;
                }

                return seek(++idx);
            };

            return seek(0);
        }

        /// <summary>
        /// The Copy.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="source">The source<see cref="T[]"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <returns>The <see cref="T[]"/>.</returns>
        internal static T[] Copy<T>(this T[] source, int length)
        {
            var dest = new T[length];
            Array.Copy(source, 0, dest, 0, length);

            return dest;
        }

        /// <summary>
        /// The Copy.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="source">The source<see cref="T[]"/>.</param>
        /// <param name="length">The length<see cref="long"/>.</param>
        /// <returns>The <see cref="T[]"/>.</returns>
        internal static T[] Copy<T>(this T[] source, long length)
        {
            var dest = new T[length];
            Array.Copy(source, 0, dest, 0, length);

            return dest;
        }

        /// <summary>
        /// The CopyTo.
        /// </summary>
        /// <param name="source">The source<see cref="Stream"/>.</param>
        /// <param name="destination">The destination<see cref="Stream"/>.</param>
        /// <param name="bufferLength">The bufferLength<see cref="int"/>.</param>
        internal static void CopyTo(
      this Stream source, Stream destination, int bufferLength
    )
        {
            var buff = new byte[bufferLength];
            var nread = 0;

            while (true)
            {
                nread = source.Read(buff, 0, bufferLength);
                if (nread <= 0)
                    break;

                destination.Write(buff, 0, nread);
            }
        }

        /// <summary>
        /// The CopyToAsync.
        /// </summary>
        /// <param name="source">The source<see cref="Stream"/>.</param>
        /// <param name="destination">The destination<see cref="Stream"/>.</param>
        /// <param name="bufferLength">The bufferLength<see cref="int"/>.</param>
        /// <param name="completed">The completed<see cref="Action"/>.</param>
        /// <param name="error">The error<see cref="Action{Exception}"/>.</param>
        internal static void CopyToAsync(
      this Stream source,
      Stream destination,
      int bufferLength,
      Action completed,
      Action<Exception> error
    )
        {
            var buff = new byte[bufferLength];

            AsyncCallback callback = null;
            callback =
              ar =>
              {
                  try
                  {
                      var nread = source.EndRead(ar);
                      if (nread <= 0)
                      {
                          if (completed != null)
                              completed();

                          return;
                      }

                      destination.Write(buff, 0, nread);
                      source.BeginRead(buff, 0, bufferLength, callback, null);
                  }
                  catch (Exception ex)
                  {
                      if (error != null)
                          error(ex);
                  }
              };

            try
            {
                source.BeginRead(buff, 0, bufferLength, callback, null);
            }
            catch (Exception ex)
            {
                if (error != null)
                    error(ex);
            }
        }

        /// <summary>
        /// The Decompress.
        /// </summary>
        /// <param name="data">The data<see cref="byte[]"/>.</param>
        /// <param name="method">The method<see cref="CompressionMethod"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        internal static byte[] Decompress(this byte[] data, CompressionMethod method)
        {
            return method == CompressionMethod.Deflate
                   ? data.decompress()
                   : data;
        }

        /// <summary>
        /// The Decompress.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="method">The method<see cref="CompressionMethod"/>.</param>
        /// <returns>The <see cref="Stream"/>.</returns>
        internal static Stream Decompress(this Stream stream, CompressionMethod method)
        {
            return method == CompressionMethod.Deflate
                   ? stream.decompress()
                   : stream;
        }

        /// <summary>
        /// The DecompressToArray.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="method">The method<see cref="CompressionMethod"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        internal static byte[] DecompressToArray(this Stream stream, CompressionMethod method)
        {
            return method == CompressionMethod.Deflate
                   ? stream.decompressToArray()
                   : stream.ToByteArray();
        }

        /// <summary>
        /// Determines whether the specified <see cref="int"/> equals the specified <see cref="char"/>,
        /// and invokes the specified <c>Action&lt;int&gt;</c> delegate at the same time.
        /// </summary>
        /// <param name="value">The value<see cref="int"/>.</param>
        /// <param name="c">The c<see cref="char"/>.</param>
        /// <param name="action">The action<see cref="Action{int}"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool EqualsWith(this int value, char c, Action<int> action)
        {
            action(value);
            return value == c - 0;
        }

        /// <summary>
        /// Gets the absolute path from the specified <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string GetAbsolutePath(this Uri uri)
        {
            if (uri.IsAbsoluteUri)
                return uri.AbsolutePath;

            var original = uri.OriginalString;
            if (original[0] != '/')
                return null;

            var idx = original.IndexOfAny(new[] { '?', '#' });
            return idx > 0 ? original.Substring(0, idx) : original;
        }

        /// <summary>
        /// The GetCookies.
        /// </summary>
        /// <param name="headers">The headers<see cref="NameValueCollection"/>.</param>
        /// <param name="response">The response<see cref="bool"/>.</param>
        /// <returns>The <see cref="CookieCollection"/>.</returns>
        internal static CookieCollection GetCookies(
      this NameValueCollection headers, bool response
    )
        {
            var val = headers[response ? "Set-Cookie" : "Cookie"];
            return val != null
                   ? CookieCollection.Parse(val, response)
                   : new CookieCollection();
        }

        /// <summary>
        /// The GetDnsSafeHost.
        /// </summary>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <param name="bracketIPv6">The bracketIPv6<see cref="bool"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string GetDnsSafeHost(this Uri uri, bool bracketIPv6)
        {
            return bracketIPv6 && uri.HostNameType == UriHostNameType.IPv6
                   ? uri.Host
                   : uri.DnsSafeHost;
        }

        /// <summary>
        /// The GetMessage.
        /// </summary>
        /// <param name="code">The code<see cref="CloseStatusCode"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string GetMessage(this CloseStatusCode code)
        {
            return code == CloseStatusCode.ProtocolError
                   ? "A WebSocket protocol error has occurred."
                   : code == CloseStatusCode.UnsupportedData
                     ? "Unsupported data has been received."
                     : code == CloseStatusCode.Abnormal
                       ? "An exception has occurred."
                       : code == CloseStatusCode.InvalidData
                         ? "Invalid data has been received."
                         : code == CloseStatusCode.PolicyViolation
                           ? "A policy violation has occurred."
                           : code == CloseStatusCode.TooBig
                             ? "A too big message has been received."
                             : code == CloseStatusCode.MandatoryExtension
                               ? "WebSocket client didn't receive expected extension(s)."
                               : code == CloseStatusCode.ServerError
                                 ? "WebSocket server got an internal error."
                                 : code == CloseStatusCode.TlsHandshakeFailure
                                   ? "An error has occurred during a TLS handshake."
                                   : String.Empty;
        }

        /// <summary>
        /// Gets the name from the specified string that contains a pair of
        /// name and value separated by a character.
        /// </summary>
        /// <param name="nameAndValue">The nameAndValue<see cref="string"/>.</param>
        /// <param name="separator">The separator<see cref="char"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string GetName(this string nameAndValue, char separator)
        {
            var idx = nameAndValue.IndexOf(separator);
            return idx > 0 ? nameAndValue.Substring(0, idx).Trim() : null;
        }

        /// <summary>
        /// The GetUTF8DecodedString.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string GetUTF8DecodedString(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// The GetUTF8EncodedBytes.
        /// </summary>
        /// <param name="s">The s<see cref="string"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        internal static byte[] GetUTF8EncodedBytes(this string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }

        /// <summary>
        /// Gets the value from the specified string that contains a pair of
        /// name and value separated by a character.
        /// </summary>
        /// <param name="nameAndValue">The nameAndValue<see cref="string"/>.</param>
        /// <param name="separator">The separator<see cref="char"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string GetValue(this string nameAndValue, char separator)
        {
            return nameAndValue.GetValue(separator, false);
        }

        /// <summary>
        /// Gets the value from the specified string that contains a pair of
        /// name and value separated by a character.
        /// </summary>
        /// <param name="nameAndValue">The nameAndValue<see cref="string"/>.</param>
        /// <param name="separator">The separator<see cref="char"/>.</param>
        /// <param name="unquote">The unquote<see cref="bool"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string GetValue(
      this string nameAndValue, char separator, bool unquote
    )
        {
            var idx = nameAndValue.IndexOf(separator);
            if (idx < 0 || idx == nameAndValue.Length - 1)
                return null;

            var val = nameAndValue.Substring(idx + 1).Trim();
            return unquote ? val.Unquote() : val;
        }

        /// <summary>
        /// The InternalToByteArray.
        /// </summary>
        /// <param name="value">The value<see cref="ushort"/>.</param>
        /// <param name="order">The order<see cref="ByteOrder"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        internal static byte[] InternalToByteArray(
      this ushort value, ByteOrder order
    )
        {
            var ret = BitConverter.GetBytes(value);

            if (!order.IsHostOrder())
                Array.Reverse(ret);

            return ret;
        }

        /// <summary>
        /// The InternalToByteArray.
        /// </summary>
        /// <param name="value">The value<see cref="ulong"/>.</param>
        /// <param name="order">The order<see cref="ByteOrder"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        internal static byte[] InternalToByteArray(
      this ulong value, ByteOrder order
    )
        {
            var ret = BitConverter.GetBytes(value);

            if (!order.IsHostOrder())
                Array.Reverse(ret);

            return ret;
        }

        /// <summary>
        /// The IsCompressionExtension.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <param name="method">The method<see cref="CompressionMethod"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool IsCompressionExtension(
      this string value, CompressionMethod method
    )
        {
            return value.StartsWith(method.ToExtensionString());
        }

        /// <summary>
        /// The IsControl.
        /// </summary>
        /// <param name="opcode">The opcode<see cref="byte"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool IsControl(this byte opcode)
        {
            return opcode > 0x7 && opcode < 0x10;
        }

        /// <summary>
        /// The IsControl.
        /// </summary>
        /// <param name="opcode">The opcode<see cref="Opcode"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool IsControl(this Opcode opcode)
        {
            return opcode >= Opcode.Close;
        }

        /// <summary>
        /// The IsData.
        /// </summary>
        /// <param name="opcode">The opcode<see cref="byte"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool IsData(this byte opcode)
        {
            return opcode == 0x1 || opcode == 0x2;
        }

        /// <summary>
        /// The IsData.
        /// </summary>
        /// <param name="opcode">The opcode<see cref="Opcode"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool IsData(this Opcode opcode)
        {
            return opcode == Opcode.Text || opcode == Opcode.Binary;
        }

        /// <summary>
        /// The IsHttpMethod.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <param name="version">The version<see cref="Version"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool IsHttpMethod(this string value, Version version)
        {
            return version == HttpVersion.Version10
                   ? value.isHttpMethod10()
                   : value.isHttpMethod();
        }

        /// <summary>
        /// The IsPortNumber.
        /// </summary>
        /// <param name="value">The value<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool IsPortNumber(this int value)
        {
            return value > 0 && value < 65536;
        }

        /// <summary>
        /// The IsReserved.
        /// </summary>
        /// <param name="code">The code<see cref="ushort"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool IsReserved(this ushort code)
        {
            return code == 1004
                   || code == 1005
                   || code == 1006
                   || code == 1015;
        }

        /// <summary>
        /// The IsReserved.
        /// </summary>
        /// <param name="code">The code<see cref="CloseStatusCode"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool IsReserved(this CloseStatusCode code)
        {
            return code == CloseStatusCode.Undefined
                   || code == CloseStatusCode.NoStatus
                   || code == CloseStatusCode.Abnormal
                   || code == CloseStatusCode.TlsHandshakeFailure;
        }

        /// <summary>
        /// The IsSupported.
        /// </summary>
        /// <param name="opcode">The opcode<see cref="byte"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool IsSupported(this byte opcode)
        {
            return Enum.IsDefined(typeof(Opcode), opcode);
        }

        /// <summary>
        /// The IsText.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool IsText(this string value)
        {
            var len = value.Length;

            for (var i = 0; i < len; i++)
            {
                var c = value[i];
                if (c < 0x20)
                {
                    if ("\r\n\t".IndexOf(c) == -1)
                        return false;

                    if (c == '\n')
                    {
                        i++;
                        if (i == len)
                            break;

                        c = value[i];
                        if (" \t".IndexOf(c) == -1)
                            return false;
                    }

                    continue;
                }

                if (c == 0x7f)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// The IsToken.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool IsToken(this string value)
        {
            foreach (var c in value)
            {
                if (c < 0x20)
                    return false;

                if (c > 0x7e)
                    return false;

                if (_tspecials.IndexOf(c) > -1)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// The KeepsAlive.
        /// </summary>
        /// <param name="headers">The headers<see cref="NameValueCollection"/>.</param>
        /// <param name="version">The version<see cref="Version"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool KeepsAlive(
      this NameValueCollection headers, Version version
    )
        {
            var comparison = StringComparison.OrdinalIgnoreCase;
            return version < HttpVersion.Version11
                   ? headers.Contains("Connection", "keep-alive", comparison)
                   : !headers.Contains("Connection", "close", comparison);
        }

        /// <summary>
        /// The Quote.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string Quote(this string value)
        {
            return String.Format("\"{0}\"", value.Replace("\"", "\\\""));
        }

        /// <summary>
        /// The ReadBytes.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        internal static byte[] ReadBytes(this Stream stream, int length)
        {
            var buff = new byte[length];
            var offset = 0;
            var retry = 0;
            var nread = 0;

            while (length > 0)
            {
                nread = stream.Read(buff, offset, length);
                if (nread <= 0)
                {
                    if (retry < _retry)
                    {
                        retry++;
                        continue;
                    }

                    return buff.SubArray(0, offset);
                }

                retry = 0;

                offset += nread;
                length -= nread;
            }

            return buff;
        }

        /// <summary>
        /// The ReadBytes.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="length">The length<see cref="long"/>.</param>
        /// <param name="bufferLength">The bufferLength<see cref="int"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        internal static byte[] ReadBytes(
      this Stream stream, long length, int bufferLength
    )
        {
            using (var dest = new MemoryStream())
            {
                var buff = new byte[bufferLength];
                var retry = 0;
                var nread = 0;

                while (length > 0)
                {
                    if (length < bufferLength)
                        bufferLength = (int)length;

                    nread = stream.Read(buff, 0, bufferLength);
                    if (nread <= 0)
                    {
                        if (retry < _retry)
                        {
                            retry++;
                            continue;
                        }

                        break;
                    }

                    retry = 0;

                    dest.Write(buff, 0, nread);
                    length -= nread;
                }

                dest.Close();
                return dest.ToArray();
            }
        }

        /// <summary>
        /// Attempt to read at least a specific number of bytes from the stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <param name="length">The minimum number of bytes that need to be read.</param>
        /// <param name="completed">An action to call when teh bytes are ready.</param>
        /// <param name="error">An action to call if an error occurs.</param>
        internal async static void ReadBytesAsync(
          this Stream stream,
          int length,
          Action<byte[]> completed,
          Action<Exception> error
        )
        {
            try
            {
                var buff = new byte[length];
                var offset = 0;
                var retry = 0;

                while (retry < _retry)
                {
                    retry++;
                    var nread = await stream.ReadAsync(buff, offset, length - offset);
                    offset += nread;

                    //if(nread == 0)
                    //{
                    //    error(new SocketException(SocketError.ConnectionReset.GetHashCode()));
                    //}
                    if (nread == length)
                    {
                        completed?.Invoke(buff);
                        break;
                    }
                    else if (retry >= _retry && offset > 0)
                    {
                        completed?.Invoke(buff.SubArray(0, offset));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                error?.Invoke(ex);
            }
        }

        /// <summary>
        /// The ReadBytesAsync.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="length">The length<see cref="long"/>.</param>
        /// <param name="bufferLength">The bufferLength<see cref="int"/>.</param>
        /// <param name="completed">The completed<see cref="Action{byte[]}"/>.</param>
        /// <param name="error">The error<see cref="Action{Exception}"/>.</param>
        internal static void ReadBytesAsync(
      this Stream stream,
      long length,
      int bufferLength,
      Action<byte[]> completed,
      Action<Exception> error
    )
        {
            var dest = new MemoryStream();
            var buff = new byte[bufferLength];
            var retry = 0;

            Action<long> read = null;
            read =
              len =>
              {
                  if (len < bufferLength)
                      bufferLength = (int)len;

                  stream.BeginRead(
              buff,
              0,
              bufferLength,
              ar =>
                  {
                      try
                      {
                          var nread = stream.EndRead(ar);
                          if (nread <= 0)
                          {
                              if (retry < _retry)
                              {
                                  retry++;
                                  read(len);

                                  return;
                              }

                              if (completed != null)
                              {
                                  dest.Close();
                                  completed(dest.ToArray());
                              }

                              dest.Dispose();
                              return;
                          }

                          dest.Write(buff, 0, nread);

                          if (nread == len)
                          {
                              if (completed != null)
                              {
                                  dest.Close();
                                  completed(dest.ToArray());
                              }

                              dest.Dispose();
                              return;
                          }

                          retry = 0;

                          read(len - nread);
                      }
                      catch (Exception ex)
                      {
                          dest.Dispose();
                          if (error != null)
                              error(ex);
                      }
                  },
              null
            );
              };

            try
            {
                read(length);
            }
            catch (Exception ex)
            {
                dest.Dispose();
                if (error != null)
                    error(ex);
            }
        }

        /// <summary>
        /// The Reverse.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="array">The array<see cref="T[]"/>.</param>
        /// <returns>The <see cref="T[]"/>.</returns>
        internal static T[] Reverse<T>(this T[] array)
        {
            var len = array.Length;
            var ret = new T[len];

            var end = len - 1;
            for (var i = 0; i <= end; i++)
                ret[i] = array[end - i];

            return ret;
        }

        /// <summary>
        /// The SplitHeaderValue.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <param name="separators">The separators<see cref="char[]"/>.</param>
        /// <returns>The <see cref="IEnumerable{string}"/>.</returns>
        internal static IEnumerable<string> SplitHeaderValue(
      this string value, params char[] separators
    )
        {
            var len = value.Length;

            var buff = new StringBuilder(32);
            var end = len - 1;
            var escaped = false;
            var quoted = false;

            for (var i = 0; i <= end; i++)
            {
                var c = value[i];
                buff.Append(c);

                if (c == '"')
                {
                    if (escaped)
                    {
                        escaped = false;
                        continue;
                    }

                    quoted = !quoted;
                    continue;
                }

                if (c == '\\')
                {
                    if (i == end)
                        break;

                    if (value[i + 1] == '"')
                        escaped = true;

                    continue;
                }

                if (Array.IndexOf(separators, c) > -1)
                {
                    if (quoted)
                        continue;

                    buff.Length -= 1;
                    yield return buff.ToString();

                    buff.Length = 0;
                    continue;
                }
            }

            yield return buff.ToString();
        }

        /// <summary>
        /// The ToByteArray.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        internal static byte[] ToByteArray(this Stream stream)
        {
            using (var output = new MemoryStream())
            {
                stream.Position = 0;
                stream.CopyTo(output, 1024);
                output.Close();

                return output.ToArray();
            }
        }

        /// <summary>
        /// The ToCompressionMethod.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="CompressionMethod"/>.</returns>
        internal static CompressionMethod ToCompressionMethod(this string value)
        {
            var methods = Enum.GetValues(typeof(CompressionMethod));
            foreach (CompressionMethod method in methods)
            {
                if (method.ToExtensionString() == value)
                    return method;
            }

            return CompressionMethod.None;
        }

        /// <summary>
        /// The ToExtensionString.
        /// </summary>
        /// <param name="method">The method<see cref="CompressionMethod"/>.</param>
        /// <param name="parameters">The parameters<see cref="string[]"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string ToExtensionString(
      this CompressionMethod method, params string[] parameters
    )
        {
            if (method == CompressionMethod.None)
                return String.Empty;

            var name = String.Format(
                         "permessage-{0}", method.ToString().ToLower()
                       );

            return parameters != null && parameters.Length > 0
                   ? String.Format("{0}; {1}", name, parameters.ToString("; "))
                   : name;
        }

        /// <summary>
        /// The ToIPAddress.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="System.Net.IPAddress"/>.</returns>
        internal static System.Net.IPAddress ToIPAddress(this string value)
        {
            if (value == null || value.Length == 0)
                return null;

            System.Net.IPAddress addr;
            if (System.Net.IPAddress.TryParse(value, out addr))
                return addr;

            try
            {
                var addrs = System.Net.Dns.GetHostAddresses(value);
                return addrs[0];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// The ToList.
        /// </summary>
        /// <typeparam name="TSource">.</typeparam>
        /// <param name="source">The source<see cref="IEnumerable{TSource}"/>.</param>
        /// <returns>The <see cref="List{TSource}"/>.</returns>
        internal static List<TSource> ToList<TSource>(
      this IEnumerable<TSource> source
    )
        {
            return new List<TSource>(source);
        }

        /// <summary>
        /// The ToString.
        /// </summary>
        /// <param name="address">The address<see cref="System.Net.IPAddress"/>.</param>
        /// <param name="bracketIPv6">The bracketIPv6<see cref="bool"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string ToString(
      this System.Net.IPAddress address, bool bracketIPv6
    )
        {
            return bracketIPv6
                   && address.AddressFamily == AddressFamily.InterNetworkV6
                   ? String.Format("[{0}]", address.ToString())
                   : address.ToString();
        }

        /// <summary>
        /// The ToUInt16.
        /// </summary>
        /// <param name="source">The source<see cref="byte[]"/>.</param>
        /// <param name="sourceOrder">The sourceOrder<see cref="ByteOrder"/>.</param>
        /// <returns>The <see cref="ushort"/>.</returns>
        internal static ushort ToUInt16(this byte[] source, ByteOrder sourceOrder)
        {
            return BitConverter.ToUInt16(source.ToHostOrder(sourceOrder), 0);
        }

        /// <summary>
        /// The ToUInt64.
        /// </summary>
        /// <param name="source">The source<see cref="byte[]"/>.</param>
        /// <param name="sourceOrder">The sourceOrder<see cref="ByteOrder"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        internal static ulong ToUInt64(this byte[] source, ByteOrder sourceOrder)
        {
            return BitConverter.ToUInt64(source.ToHostOrder(sourceOrder), 0);
        }

        /// <summary>
        /// The Trim.
        /// </summary>
        /// <param name="source">The source<see cref="IEnumerable{string}"/>.</param>
        /// <returns>The <see cref="IEnumerable{string}"/>.</returns>
        internal static IEnumerable<string> Trim(this IEnumerable<string> source)
        {
            foreach (var elm in source)
                yield return elm.Trim();
        }

        /// <summary>
        /// The TrimSlashFromEnd.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string TrimSlashFromEnd(this string value)
        {
            var ret = value.TrimEnd('/');
            return ret.Length > 0 ? ret : "/";
        }

        /// <summary>
        /// The TrimSlashOrBackslashFromEnd.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string TrimSlashOrBackslashFromEnd(this string value)
        {
            var ret = value.TrimEnd('/', '\\');
            return ret.Length > 0 ? ret : value[0].ToString();
        }

        /// <summary>
        /// The TryCreateVersion.
        /// </summary>
        /// <param name="versionString">The versionString<see cref="string"/>.</param>
        /// <param name="result">The result<see cref="Version"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool TryCreateVersion(
      this string versionString, out Version result
    )
        {
            result = null;

            try
            {
                result = new Version(versionString);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Tries to create a new <see cref="Uri"/> for WebSocket with
        /// the specified <paramref name="uriString"/>.
        /// </summary>
        /// <param name="uriString">The uriString<see cref="string"/>.</param>
        /// <param name="result">The result<see cref="Uri"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool TryCreateWebSocketUri(
      this string uriString, out Uri result, out string message
    )
        {
            result = null;
            message = null;

            var uri = uriString.ToUri();
            if (uri == null)
            {
                message = "An invalid URI string.";
                return false;
            }

            if (!uri.IsAbsoluteUri)
            {
                message = "A relative URI.";
                return false;
            }

            var schm = uri.Scheme;
            if (!(schm == "ws" || schm == "wss"))
            {
                message = "The scheme part is not 'ws' or 'wss'.";
                return false;
            }

            var port = uri.Port;
            if (port == 0)
            {
                message = "The port part is zero.";
                return false;
            }

            if (uri.Fragment.Length > 0)
            {
                message = "It includes the fragment component.";
                return false;
            }

            result = port != -1
                     ? uri
                     : new Uri(
                         String.Format(
                           "{0}://{1}:{2}{3}",
                           schm,
                           uri.Host,
                           schm == "ws" ? 80 : 443,
                           uri.PathAndQuery
                         )
                       );

            return true;
        }

        /// <summary>
        /// The TryGetUTF8DecodedString.
        /// </summary>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        /// <param name="s">The s<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool TryGetUTF8DecodedString(this byte[] bytes, out string s)
        {
            s = null;

            try
            {
                s = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The TryGetUTF8EncodedBytes.
        /// </summary>
        /// <param name="s">The s<see cref="string"/>.</param>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool TryGetUTF8EncodedBytes(this string s, out byte[] bytes)
        {
            bytes = null;

            try
            {
                bytes = Encoding.UTF8.GetBytes(s);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The TryOpenRead.
        /// </summary>
        /// <param name="fileInfo">The fileInfo<see cref="FileInfo"/>.</param>
        /// <param name="fileStream">The fileStream<see cref="FileStream"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool TryOpenRead(
      this FileInfo fileInfo, out FileStream fileStream
    )
        {
            fileStream = null;

            try
            {
                fileStream = fileInfo.OpenRead();
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The Unquote.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string Unquote(this string value)
        {
            var start = value.IndexOf('"');
            if (start == -1)
                return value;

            var end = value.LastIndexOf('"');
            if (end == start)
                return value;

            var len = end - start - 1;
            return len > 0
                   ? value.Substring(start + 1, len).Replace("\\\"", "\"")
                   : String.Empty;
        }

        /// <summary>
        /// The Upgrades.
        /// </summary>
        /// <param name="headers">The headers<see cref="NameValueCollection"/>.</param>
        /// <param name="protocol">The protocol<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool Upgrades(
      this NameValueCollection headers, string protocol
    )
        {
            var comparison = StringComparison.OrdinalIgnoreCase;
            return headers.Contains("Upgrade", protocol, comparison)
                   && headers.Contains("Connection", "Upgrade", comparison);
        }

        /// <summary>
        /// The UrlDecode.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <param name="encoding">The encoding<see cref="Encoding"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string UrlDecode(this string value, Encoding encoding)
        {
            return HttpUtility.UrlDecode(value, encoding);
        }

        /// <summary>
        /// The UrlEncode.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <param name="encoding">The encoding<see cref="Encoding"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string UrlEncode(this string value, Encoding encoding)
        {
            return HttpUtility.UrlEncode(value, encoding);
        }

        /// <summary>
        /// The WriteBytes.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        /// <param name="bufferLength">The bufferLength<see cref="int"/>.</param>
        internal static void WriteBytes(
      this Stream stream, byte[] bytes, int bufferLength
    )
        {
            using (var src = new MemoryStream(bytes))
                src.CopyTo(stream, bufferLength);
        }

        /// <summary>
        /// The WriteBytesAsync.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        /// <param name="bufferLength">The bufferLength<see cref="int"/>.</param>
        /// <param name="completed">The completed<see cref="Action"/>.</param>
        /// <param name="error">The error<see cref="Action{Exception}"/>.</param>
        internal static void WriteBytesAsync(
      this Stream stream,
      byte[] bytes,
      int bufferLength,
      Action completed,
      Action<Exception> error
    )
        {
            var src = new MemoryStream(bytes);
            src.CopyToAsync(
              stream,
              bufferLength,
              () =>
              {
                  if (completed != null)
                      completed();

                  src.Dispose();
              },
              ex =>
              {
                  src.Dispose();
                  if (error != null)
                      error(ex);
              }
            );
        }

        /// <summary>
        /// Emits the specified <see cref="EventHandler"/> delegate if it isn't <see langword="null"/>.
        /// </summary>
        /// <param name="eventHandler">The eventHandler<see cref="EventHandler"/>.</param>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        public static void Emit(this EventHandler eventHandler, object sender, EventArgs e)
        {
            if (eventHandler != null)
                eventHandler(sender, e);
        }

        /// <summary>
        /// Emits the specified <c>EventHandler&lt;TEventArgs&gt;</c> delegate if it isn't
        /// <see langword="null"/>.
        /// </summary>
        /// <typeparam name="TEventArgs">.</typeparam>
        /// <param name="eventHandler">The eventHandler<see cref="EventHandler{TEventArgs}"/>.</param>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="TEventArgs"/>.</param>
        public static void Emit<TEventArgs>(
      this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs e)
      where TEventArgs : EventArgs
        {
            if (eventHandler != null)
                eventHandler(sender, e);
        }

        /// <summary>
        /// Gets the description of the specified HTTP status <paramref name="code"/>.
        /// </summary>
        /// <param name="code">The code<see cref="HttpStatusCode"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetDescription(this HttpStatusCode code)
        {
            return ((int)code).GetStatusDescription();
        }

        /// <summary>
        /// Gets the description of the specified HTTP status <paramref name="code"/>.
        /// </summary>
        /// <param name="code">The code<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetStatusDescription(this int code)
        {
            switch (code)
            {
                case 100: return "Continue";
                case 101: return "Switching Protocols";
                case 102: return "Processing";
                case 200: return "OK";
                case 201: return "Created";
                case 202: return "Accepted";
                case 203: return "Non-Authoritative Information";
                case 204: return "No Content";
                case 205: return "Reset Content";
                case 206: return "Partial Content";
                case 207: return "Multi-Status";
                case 300: return "Multiple Choices";
                case 301: return "Moved Permanently";
                case 302: return "Found";
                case 303: return "See Other";
                case 304: return "Not Modified";
                case 305: return "Use Proxy";
                case 307: return "Temporary Redirect";
                case 400: return "Bad Request";
                case 401: return "Unauthorized";
                case 402: return "Payment Required";
                case 403: return "Forbidden";
                case 404: return "Not Found";
                case 405: return "Method Not Allowed";
                case 406: return "Not Acceptable";
                case 407: return "Proxy Authentication Required";
                case 408: return "Request Timeout";
                case 409: return "Conflict";
                case 410: return "Gone";
                case 411: return "Length Required";
                case 412: return "Precondition Failed";
                case 413: return "Request Entity Too Large";
                case 414: return "Request-Uri Too Long";
                case 415: return "Unsupported Media Type";
                case 416: return "Requested Range Not Satisfiable";
                case 417: return "Expectation Failed";
                case 422: return "Unprocessable Entity";
                case 423: return "Locked";
                case 424: return "Failed Dependency";
                case 500: return "Internal Server Error";
                case 501: return "Not Implemented";
                case 502: return "Bad Gateway";
                case 503: return "Service Unavailable";
                case 504: return "Gateway Timeout";
                case 505: return "Http Version Not Supported";
                case 507: return "Insufficient Storage";
            }

            return String.Empty;
        }

        /// <summary>
        /// Determines whether the specified ushort is in the range of
        /// the status code for the WebSocket connection close.
        /// </summary>
        /// <param name="value">The value<see cref="ushort"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsCloseStatusCode(this ushort value)
        {
            return value > 999 && value < 5000;
        }

        /// <summary>
        /// Determines whether the specified string is enclosed in
        /// the specified character.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <param name="c">The c<see cref="char"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsEnclosedIn(this string value, char c)
        {
            if (value == null)
                return false;

            var len = value.Length;
            if (len < 2)
                return false;

            return value[0] == c && value[len - 1] == c;
        }

        /// <summary>
        /// Determines whether the specified byte order is host (this computer
        /// architecture) byte order.
        /// </summary>
        /// <param name="order">The order<see cref="ByteOrder"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsHostOrder(this ByteOrder order)
        {
            // true: !(true ^ true) or !(false ^ false)
            // false: !(true ^ false) or !(false ^ true)
            return !(BitConverter.IsLittleEndian ^ (order == ByteOrder.Little));
        }

        /// <summary>
        /// Determines whether the specified IP address is a local IP address.
        /// </summary>
        /// <param name="address">The address<see cref="System.Net.IPAddress"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsLocal(this System.Net.IPAddress address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            if (address.Equals(System.Net.IPAddress.Any))
                return true;

            if (address.Equals(System.Net.IPAddress.Loopback))
                return true;

            if (Socket.OSSupportsIPv6)
            {
                if (address.Equals(System.Net.IPAddress.IPv6Any))
                    return true;

                if (address.Equals(System.Net.IPAddress.IPv6Loopback))
                    return true;
            }

            var host = System.Net.Dns.GetHostName();
            var addrs = System.Net.Dns.GetHostAddresses(host);
            foreach (var addr in addrs)
            {
                if (address.Equals(addr))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified string is <see langword="null"/> or
        /// an empty string.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return value == null || value.Length == 0;
        }

        /// <summary>
        /// Determines whether the specified string is a predefined scheme.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsPredefinedScheme(this string value)
        {
            if (value == null || value.Length < 2)
                return false;

            var c = value[0];
            if (c == 'h')
                return value == "http" || value == "https";

            if (c == 'w')
                return value == "ws" || value == "wss";

            if (c == 'f')
                return value == "file" || value == "ftp";

            if (c == 'g')
                return value == "gopher";

            if (c == 'm')
                return value == "mailto";

            if (c == 'n')
            {
                c = value[1];
                return c == 'e'
                       ? value == "news" || value == "net.pipe" || value == "net.tcp"
                       : value == "nntp";
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified string is a URI string.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool MaybeUri(this string value)
        {
            if (value == null)
                return false;

            if (value.Length == 0)
                return false;

            var idx = value.IndexOf(':');
            if (idx == -1)
                return false;

            if (idx >= 10)
                return false;

            var schm = value.Substring(0, idx);
            return schm.IsPredefinedScheme();
        }

        /// <summary>
        /// Retrieves a sub-array from the specified array. A sub-array starts at
        /// the specified index in the array.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="array">The array<see cref="T[]"/>.</param>
        /// <param name="startIndex">The startIndex<see cref="int"/>.</param>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <returns>The <see cref="T[]"/>.</returns>
        public static T[] SubArray<T>(this T[] array, int startIndex, int length)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            var len = array.Length;
            if (len == 0)
            {
                if (startIndex != 0)
                    throw new ArgumentOutOfRangeException("startIndex");

                if (length != 0)
                    throw new ArgumentOutOfRangeException("length");

                return array;
            }

            if (startIndex < 0 || startIndex >= len)
                throw new ArgumentOutOfRangeException("startIndex");

            if (length < 0 || length > len - startIndex)
                throw new ArgumentOutOfRangeException("length");

            if (length == 0)
                return new T[0];

            if (length == len)
                return array;

            var subArray = new T[length];
            Array.Copy(array, startIndex, subArray, 0, length);

            return subArray;
        }

        /// <summary>
        /// Retrieves a sub-array from the specified array. A sub-array starts at
        /// the specified index in the array.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="array">The array<see cref="T[]"/>.</param>
        /// <param name="startIndex">The startIndex<see cref="long"/>.</param>
        /// <param name="length">The length<see cref="long"/>.</param>
        /// <returns>The <see cref="T[]"/>.</returns>
        public static T[] SubArray<T>(this T[] array, long startIndex, long length)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            var len = array.LongLength;
            if (len == 0)
            {
                if (startIndex != 0)
                    throw new ArgumentOutOfRangeException("startIndex");

                if (length != 0)
                    throw new ArgumentOutOfRangeException("length");

                return array;
            }

            if (startIndex < 0 || startIndex >= len)
                throw new ArgumentOutOfRangeException("startIndex");

            if (length < 0 || length > len - startIndex)
                throw new ArgumentOutOfRangeException("length");

            if (length == 0)
                return new T[0];

            if (length == len)
                return array;

            var subArray = new T[length];
            Array.Copy(array, startIndex, subArray, 0, length);

            return subArray;
        }

        /// <summary>
        /// Executes the specified delegate <paramref name="n"/> times.
        /// </summary>
        /// <param name="n">The n<see cref="int"/>.</param>
        /// <param name="action">The action<see cref="Action"/>.</param>
        public static void Times(this int n, Action action)
        {
            if (n <= 0)
                return;

            if (action == null)
                return;

            for (int i = 0; i < n; i++)
                action();
        }

        /// <summary>
        /// Executes the specified delegate <paramref name="n"/> times.
        /// </summary>
        /// <param name="n">The n<see cref="long"/>.</param>
        /// <param name="action">The action<see cref="Action"/>.</param>
        public static void Times(this long n, Action action)
        {
            if (n <= 0)
                return;

            if (action == null)
                return;

            for (long i = 0; i < n; i++)
                action();
        }

        /// <summary>
        /// Executes the specified delegate <paramref name="n"/> times.
        /// </summary>
        /// <param name="n">The n<see cref="uint"/>.</param>
        /// <param name="action">The action<see cref="Action"/>.</param>
        public static void Times(this uint n, Action action)
        {
            if (n == 0)
                return;

            if (action == null)
                return;

            for (uint i = 0; i < n; i++)
                action();
        }

        /// <summary>
        /// Executes the specified delegate <paramref name="n"/> times.
        /// </summary>
        /// <param name="n">The n<see cref="ulong"/>.</param>
        /// <param name="action">The action<see cref="Action"/>.</param>
        public static void Times(this ulong n, Action action)
        {
            if (n == 0)
                return;

            if (action == null)
                return;

            for (ulong i = 0; i < n; i++)
                action();
        }

        /// <summary>
        /// Executes the specified delegate <paramref name="n"/> times.
        /// </summary>
        /// <param name="n">The n<see cref="int"/>.</param>
        /// <param name="action">The action<see cref="Action{int}"/>.</param>
        public static void Times(this int n, Action<int> action)
        {
            if (n <= 0)
                return;

            if (action == null)
                return;

            for (int i = 0; i < n; i++)
                action(i);
        }

        /// <summary>
        /// Executes the specified delegate <paramref name="n"/> times.
        /// </summary>
        /// <param name="n">The n<see cref="long"/>.</param>
        /// <param name="action">The action<see cref="Action{long}"/>.</param>
        public static void Times(this long n, Action<long> action)
        {
            if (n <= 0)
                return;

            if (action == null)
                return;

            for (long i = 0; i < n; i++)
                action(i);
        }

        /// <summary>
        /// Executes the specified delegate <paramref name="n"/> times.
        /// </summary>
        /// <param name="n">The n<see cref="uint"/>.</param>
        /// <param name="action">The action<see cref="Action{uint}"/>.</param>
        public static void Times(this uint n, Action<uint> action)
        {
            if (n == 0)
                return;

            if (action == null)
                return;

            for (uint i = 0; i < n; i++)
                action(i);
        }

        /// <summary>
        /// Executes the specified delegate <paramref name="n"/> times.
        /// </summary>
        /// <param name="n">The n<see cref="ulong"/>.</param>
        /// <param name="action">The action<see cref="Action{ulong}"/>.</param>
        public static void Times(this ulong n, Action<ulong> action)
        {
            if (n == 0)
                return;

            if (action == null)
                return;

            for (ulong i = 0; i < n; i++)
                action(i);
        }

        /// <summary>
        /// Converts the specified byte array to the specified type value.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="source">The source<see cref="byte[]"/>.</param>
        /// <param name="sourceOrder">The sourceOrder<see cref="ByteOrder"/>.</param>
        /// <returns>The <see cref="T"/>.</returns>
        [Obsolete("This method will be removed.")]
        public static T To<T>(this byte[] source, ByteOrder sourceOrder)
      where T : struct
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (source.Length == 0)
                return default(T);

            var type = typeof(T);
            var val = source.ToHostOrder(sourceOrder);

            return type == typeof(Boolean)
                   ? (T)(object)BitConverter.ToBoolean(val, 0)
                   : type == typeof(Char)
                     ? (T)(object)BitConverter.ToChar(val, 0)
                     : type == typeof(Double)
                       ? (T)(object)BitConverter.ToDouble(val, 0)
                       : type == typeof(Int16)
                         ? (T)(object)BitConverter.ToInt16(val, 0)
                         : type == typeof(Int32)
                           ? (T)(object)BitConverter.ToInt32(val, 0)
                           : type == typeof(Int64)
                             ? (T)(object)BitConverter.ToInt64(val, 0)
                             : type == typeof(Single)
                               ? (T)(object)BitConverter.ToSingle(val, 0)
                               : type == typeof(UInt16)
                                 ? (T)(object)BitConverter.ToUInt16(val, 0)
                                 : type == typeof(UInt32)
                                   ? (T)(object)BitConverter.ToUInt32(val, 0)
                                   : type == typeof(UInt64)
                                     ? (T)(object)BitConverter.ToUInt64(val, 0)
                                     : default(T);
        }

        /// <summary>
        /// Converts the specified value to a byte array.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="value">The value<see cref="T"/>.</param>
        /// <param name="order">The order<see cref="ByteOrder"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        [Obsolete("This method will be removed.")]
        public static byte[] ToByteArray<T>(this T value, ByteOrder order)
      where T : struct
        {
            var type = typeof(T);
            var bytes = type == typeof(Boolean)
                        ? BitConverter.GetBytes((Boolean)(object)value)
                        : type == typeof(Byte)
                          ? new byte[] { (Byte)(object)value }
                          : type == typeof(Char)
                            ? BitConverter.GetBytes((Char)(object)value)
                            : type == typeof(Double)
                              ? BitConverter.GetBytes((Double)(object)value)
                              : type == typeof(Int16)
                                ? BitConverter.GetBytes((Int16)(object)value)
                                : type == typeof(Int32)
                                  ? BitConverter.GetBytes((Int32)(object)value)
                                  : type == typeof(Int64)
                                    ? BitConverter.GetBytes((Int64)(object)value)
                                    : type == typeof(Single)
                                      ? BitConverter.GetBytes((Single)(object)value)
                                      : type == typeof(UInt16)
                                        ? BitConverter.GetBytes((UInt16)(object)value)
                                        : type == typeof(UInt32)
                                          ? BitConverter.GetBytes((UInt32)(object)value)
                                          : type == typeof(UInt64)
                                            ? BitConverter.GetBytes((UInt64)(object)value)
                                            : WebSocket.EmptyBytes;

            if (bytes.Length > 1)
            {
                if (!order.IsHostOrder())
                    Array.Reverse(bytes);
            }

            return bytes;
        }

        /// <summary>
        /// Converts the order of elements in the specified byte array to
        /// host (this computer architecture) byte order.
        /// </summary>
        /// <param name="source">The source<see cref="byte[]"/>.</param>
        /// <param name="sourceOrder">The sourceOrder<see cref="ByteOrder"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] ToHostOrder(this byte[] source, ByteOrder sourceOrder)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (source.Length < 2)
                return source;

            if (sourceOrder.IsHostOrder())
                return source;

            return source.Reverse();
        }

        /// <summary>
        /// Converts the specified array to a string.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="array">The array<see cref="T[]"/>.</param>
        /// <param name="separator">The separator<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ToString<T>(this T[] array, string separator)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            var len = array.Length;
            if (len == 0)
                return String.Empty;

            if (separator == null)
                separator = String.Empty;

            var buff = new StringBuilder(64);
            var end = len - 1;

            for (var i = 0; i < end; i++)
                buff.AppendFormat("{0}{1}", array[i], separator);

            buff.Append(array[end].ToString());
            return buff.ToString();
        }

        /// <summary>
        /// Converts the specified string to a <see cref="Uri"/>.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="Uri"/>.</returns>
        public static Uri ToUri(this string value)
        {
            Uri ret;
            Uri.TryCreate(
              value, value.MaybeUri() ? UriKind.Absolute : UriKind.Relative, out ret
            );

            return ret;
        }

        /// <summary>
        /// Sends the specified content data with the HTTP response.
        /// </summary>
        /// <param name="response">The response<see cref="HttpListenerResponse"/>.</param>
        /// <param name="content">The content<see cref="byte[]"/>.</param>
        [Obsolete("This method will be removed.")]
        public static void WriteContent(
      this HttpListenerResponse response, byte[] content
    )
        {
            if (response == null)
                throw new ArgumentNullException("response");

            if (content == null)
                throw new ArgumentNullException("content");

            var len = content.LongLength;
            if (len == 0)
            {
                response.Close();
                return;
            }

            response.ContentLength64 = len;

            var output = response.OutputStream;

            if (len <= Int32.MaxValue)
                output.Write(content, 0, (int)len);
            else
                output.WriteBytes(content, 1024);

            output.Close();
        }
    }
}
