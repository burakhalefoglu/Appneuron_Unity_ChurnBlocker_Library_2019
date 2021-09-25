/*
 * Cookie.cs
 *
 * This code is derived from Cookie.cs (System.Net) of Mono
 * (http://www.mono-project.com).
 *
 * The MIT License
 *
 * Copyright (c) 2004,2009 Novell, Inc. (http://www.novell.com)
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

/*
 * Authors:
 * - Lawrence Pit <loz@cable.a2000.nl>
 * - Gonzalo Paniagua Javier <gonzalo@ximian.com>
 * - Daniel Nauck <dna@mono-project.de>
 * - Sebastien Pouliot <sebastien@ximian.com>
 */

namespace AppneuronUnity.Core.Libraries.WebSocketSharp.Net
{
    using System;
    using System.Globalization;
    using System.Text;
using AppneuronUnity.Core.Libraries.WebSocketSharp.Net;

    /// <summary>
    /// Provides a set of methods and properties used to manage an HTTP cookie.
    /// </summary>
    [Serializable]
    public sealed class Cookie
    {
        /// <summary>
        /// Defines the _comment.
        /// </summary>
        private string _comment;

        /// <summary>
        /// Defines the _commentUri.
        /// </summary>
        private Uri _commentUri;

        /// <summary>
        /// Defines the _discard.
        /// </summary>
        private bool _discard;

        /// <summary>
        /// Defines the _domain.
        /// </summary>
        private string _domain;

        /// <summary>
        /// Defines the _emptyPorts.
        /// </summary>
        private static readonly int[] _emptyPorts;

        /// <summary>
        /// Defines the _expires.
        /// </summary>
        private DateTime _expires;

        /// <summary>
        /// Defines the _httpOnly.
        /// </summary>
        private bool _httpOnly;

        /// <summary>
        /// Defines the _name.
        /// </summary>
        private string _name;

        /// <summary>
        /// Defines the _path.
        /// </summary>
        private string _path;

        /// <summary>
        /// Defines the _port.
        /// </summary>
        private string _port;

        /// <summary>
        /// Defines the _ports.
        /// </summary>
        private int[] _ports;

        /// <summary>
        /// Defines the _reservedCharsForValue.
        /// </summary>
        private static readonly char[] _reservedCharsForValue;

        /// <summary>
        /// Defines the _sameSite.
        /// </summary>
        private string _sameSite;

        /// <summary>
        /// Defines the _secure.
        /// </summary>
        private bool _secure;

        /// <summary>
        /// Defines the _timeStamp.
        /// </summary>
        private DateTime _timeStamp;

        /// <summary>
        /// Defines the _value.
        /// </summary>
        private string _value;

        /// <summary>
        /// Defines the _version.
        /// </summary>
        private int _version;

        /// <summary>
        /// Initializes static members of the <see cref="Cookie"/> class.
        /// </summary>
        static Cookie()
        {
            _emptyPorts = new int[0];
            _reservedCharsForValue = new[] { ';', ',' };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cookie"/> class.
        /// </summary>
        internal Cookie()
        {
            init(String.Empty, String.Empty, String.Empty, String.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cookie"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="value">The value<see cref="string"/>.</param>
        public Cookie(string name, string value)
      : this(name, value, String.Empty, String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cookie"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        public Cookie(string name, string value, string path)
      : this(name, value, path, String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cookie"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="domain">The domain<see cref="string"/>.</param>
        public Cookie(string name, string value, string path, string domain)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            if (name.Length == 0)
                throw new ArgumentException("An empty string.", "name");

            if (name[0] == '$')
            {
                var msg = "It starts with a dollar sign.";
                throw new ArgumentException(msg, "name");
            }

            if (!name.IsToken())
            {
                var msg = "It contains an invalid character.";
                throw new ArgumentException(msg, "name");
            }

            if (value == null)
                value = String.Empty;

            if (value.Contains(_reservedCharsForValue))
            {
                if (!value.IsEnclosedIn('"'))
                {
                    var msg = "A string not enclosed in double quotes.";
                    throw new ArgumentException(msg, "value");
                }
            }

            init(name, value, path ?? String.Empty, domain ?? String.Empty);
        }

        /// <summary>
        /// Gets a value indicating whether ExactDomain.
        /// </summary>
        internal bool ExactDomain
        {
            get
            {
                return _domain.Length == 0 || _domain[0] != '.';
            }
        }

        /// <summary>
        /// Gets or sets the MaxAge.
        /// </summary>
        internal int MaxAge
        {
            get
            {
                if (_expires == DateTime.MinValue)
                    return 0;

                var expires = _expires.Kind != DateTimeKind.Local
                              ? _expires.ToLocalTime()
                              : _expires;

                var span = expires - DateTime.Now;
                return span > TimeSpan.Zero
                       ? (int)span.TotalSeconds
                       : 0;
            }

            set
            {
                _expires = value > 0
                           ? DateTime.Now.AddSeconds((double)value)
                           : DateTime.Now;
            }
        }

        /// <summary>
        /// Gets the Ports.
        /// </summary>
        internal int[] Ports
        {
            get
            {
                return _ports ?? _emptyPorts;
            }
        }

        /// <summary>
        /// Gets or sets the SameSite.
        /// </summary>
        internal string SameSite
        {
            get
            {
                return _sameSite;
            }

            set
            {
                _sameSite = value;
            }
        }

        /// <summary>
        /// Gets or sets the Comment
        /// Gets the value of the Comment attribute of the cookie....
        /// </summary>
        public string Comment
        {
            get
            {
                return _comment;
            }

            internal set
            {
                _comment = value;
            }
        }

        /// <summary>
        /// Gets or sets the CommentUri
        /// Gets the value of the CommentURL attribute of the cookie....
        /// </summary>
        public Uri CommentUri
        {
            get
            {
                return _commentUri;
            }

            internal set
            {
                _commentUri = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Discard
        /// Gets a value indicating whether the client discards the cookie
        /// unconditionally when the client terminates....
        /// </summary>
        public bool Discard
        {
            get
            {
                return _discard;
            }

            internal set
            {
                _discard = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the Domain attribute of the cookie....
        /// </summary>
        public string Domain
        {
            get
            {
                return _domain;
            }

            set
            {
                _domain = value ?? String.Empty;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cookie has expired....
        /// </summary>
        public bool Expired
        {
            get
            {
                return _expires != DateTime.MinValue && _expires <= DateTime.Now;
            }

            set
            {
                _expires = value ? DateTime.Now : DateTime.MinValue;
            }
        }

        /// <summary>
        /// Gets or sets the value of the Expires attribute of the cookie....
        /// </summary>
        public DateTime Expires
        {
            get
            {
                return _expires;
            }

            set
            {
                _expires = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether non-HTTP APIs can access
        /// the cookie....
        /// </summary>
        public bool HttpOnly
        {
            get
            {
                return _httpOnly;
            }

            set
            {
                _httpOnly = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the cookie....
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (value.Length == 0)
                    throw new ArgumentException("An empty string.", "value");

                if (value[0] == '$')
                {
                    var msg = "It starts with a dollar sign.";
                    throw new ArgumentException(msg, "value");
                }

                if (!value.IsToken())
                {
                    var msg = "It contains an invalid character.";
                    throw new ArgumentException(msg, "value");
                }

                _name = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the Path attribute of the cookie....
        /// </summary>
        public string Path
        {
            get
            {
                return _path;
            }

            set
            {
                _path = value ?? String.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the Port
        /// Gets the value of the Port attribute of the cookie....
        /// </summary>
        public string Port
        {
            get
            {
                return _port;
            }

            internal set
            {
                int[] ports;
                if (!tryCreatePorts(value, out ports))
                    return;

                _port = value;
                _ports = ports;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the security level of
        /// the cookie is secure....
        /// </summary>
        public bool Secure
        {
            get
            {
                return _secure;
            }

            set
            {
                _secure = value;
            }
        }

        /// <summary>
        /// Gets the time when the cookie was issued....
        /// </summary>
        public DateTime TimeStamp
        {
            get
            {
                return _timeStamp;
            }
        }

        /// <summary>
        /// Gets or sets the value of the cookie....
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }

            set
            {
                if (value == null)
                    value = String.Empty;

                if (value.Contains(_reservedCharsForValue))
                {
                    if (!value.IsEnclosedIn('"'))
                    {
                        var msg = "A string not enclosed in double quotes.";
                        throw new ArgumentException(msg, "value");
                    }
                }

                _value = value;
            }
        }

        /// <summary>
        /// Gets or sets the Version
        /// Gets the value of the Version attribute of the cookie....
        /// </summary>
        public int Version
        {
            get
            {
                return _version;
            }

            internal set
            {
                if (value < 0 || value > 1)
                    return;

                _version = value;
            }
        }

        /// <summary>
        /// The hash.
        /// </summary>
        /// <param name="i">The i<see cref="int"/>.</param>
        /// <param name="j">The j<see cref="int"/>.</param>
        /// <param name="k">The k<see cref="int"/>.</param>
        /// <param name="l">The l<see cref="int"/>.</param>
        /// <param name="m">The m<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        private static int hash(int i, int j, int k, int l, int m)
        {
            return i
                   ^ (j << 13 | j >> 19)
                   ^ (k << 26 | k >> 6)
                   ^ (l << 7 | l >> 25)
                   ^ (m << 20 | m >> 12);
        }

        /// <summary>
        /// The init.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="domain">The domain<see cref="string"/>.</param>
        private void init(string name, string value, string path, string domain)
        {
            _name = name;
            _value = value;
            _path = path;
            _domain = domain;

            _expires = DateTime.MinValue;
            _timeStamp = DateTime.Now;
        }

        /// <summary>
        /// The toResponseStringVersion0.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        private string toResponseStringVersion0()
        {
            var buff = new StringBuilder(64);

            buff.AppendFormat("{0}={1}", _name, _value);

            if (_expires != DateTime.MinValue)
            {
                buff.AppendFormat(
                  "; Expires={0}",
                  _expires.ToUniversalTime().ToString(
                    "ddd, dd'-'MMM'-'yyyy HH':'mm':'ss 'GMT'",
                    CultureInfo.CreateSpecificCulture("en-US")
                  )
                );
            }

            if (!_path.IsNullOrEmpty())
                buff.AppendFormat("; Path={0}", _path);

            if (!_domain.IsNullOrEmpty())
                buff.AppendFormat("; Domain={0}", _domain);

            if (!_sameSite.IsNullOrEmpty())
                buff.AppendFormat("; SameSite={0}", _sameSite);

            if (_secure)
                buff.Append("; Secure");

            if (_httpOnly)
                buff.Append("; HttpOnly");

            return buff.ToString();
        }

        /// <summary>
        /// The toResponseStringVersion1.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        private string toResponseStringVersion1()
        {
            var buff = new StringBuilder(64);

            buff.AppendFormat("{0}={1}; Version={2}", _name, _value, _version);

            if (_expires != DateTime.MinValue)
                buff.AppendFormat("; Max-Age={0}", MaxAge);

            if (!_path.IsNullOrEmpty())
                buff.AppendFormat("; Path={0}", _path);

            if (!_domain.IsNullOrEmpty())
                buff.AppendFormat("; Domain={0}", _domain);

            if (_port != null)
            {
                if (_port != "\"\"")
                    buff.AppendFormat("; Port={0}", _port);
                else
                    buff.Append("; Port");
            }

            if (_comment != null)
                buff.AppendFormat("; Comment={0}", HttpUtility.UrlEncode(_comment));

            if (_commentUri != null)
            {
                var url = _commentUri.OriginalString;
                buff.AppendFormat(
                  "; CommentURL={0}", !url.IsToken() ? url.Quote() : url
                );
            }

            if (_discard)
                buff.Append("; Discard");

            if (_secure)
                buff.Append("; Secure");

            return buff.ToString();
        }

        /// <summary>
        /// The tryCreatePorts.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <param name="result">The result<see cref="int[]"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private static bool tryCreatePorts(string value, out int[] result)
        {
            result = null;

            var arr = value.Trim('"').Split(',');
            var len = arr.Length;
            var res = new int[len];

            for (var i = 0; i < len; i++)
            {
                var s = arr[i].Trim();
                if (s.Length == 0)
                {
                    res[i] = Int32.MinValue;
                    continue;
                }

                if (!Int32.TryParse(s, out res[i]))
                    return false;
            }

            result = res;
            return true;
        }

        /// <summary>
        /// The EqualsWithoutValue.
        /// </summary>
        /// <param name="cookie">The cookie<see cref="Cookie"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal bool EqualsWithoutValue(Cookie cookie)
        {
            var caseSensitive = StringComparison.InvariantCulture;
            var caseInsensitive = StringComparison.InvariantCultureIgnoreCase;

            return _name.Equals(cookie._name, caseInsensitive)
                   && _path.Equals(cookie._path, caseSensitive)
                   && _domain.Equals(cookie._domain, caseInsensitive)
                   && _version == cookie._version;
        }

        /// <summary>
        /// The ToRequestString.
        /// </summary>
        /// <param name="uri">The uri<see cref="Uri"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal string ToRequestString(Uri uri)
        {
            if (_name.Length == 0)
                return String.Empty;

            if (_version == 0)
                return String.Format("{0}={1}", _name, _value);

            var buff = new StringBuilder(64);

            buff.AppendFormat("$Version={0}; {1}={2}", _version, _name, _value);

            if (!_path.IsNullOrEmpty())
                buff.AppendFormat("; $Path={0}", _path);
            else if (uri != null)
                buff.AppendFormat("; $Path={0}", uri.GetAbsolutePath());
            else
                buff.Append("; $Path=/");

            if (!_domain.IsNullOrEmpty())
            {
                if (uri == null || uri.Host != _domain)
                    buff.AppendFormat("; $Domain={0}", _domain);
            }

            if (_port != null)
            {
                if (_port != "\"\"")
                    buff.AppendFormat("; $Port={0}", _port);
                else
                    buff.Append("; $Port");
            }

            return buff.ToString();
        }

        /// <summary>
        /// Returns a string that represents the current cookie instance.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        internal string ToResponseString()
        {
            return _name.Length == 0
                   ? String.Empty
                   : _version == 0
                     ? toResponseStringVersion0()
                     : toResponseStringVersion1();
        }

        /// <summary>
        /// The TryCreate.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <param name="result">The result<see cref="Cookie"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool TryCreate(
      string name, string value, out Cookie result
    )
        {
            result = null;

            try
            {
                result = new Cookie(name, value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether the current cookie instance is equal to
        /// the specified <see cref="object"/> instance.
        /// </summary>
        /// <param name="comparand">The comparand<see cref="object"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Equals(object comparand)
        {
            var cookie = comparand as Cookie;
            if (cookie == null)
                return false;

            var caseSensitive = StringComparison.InvariantCulture;
            var caseInsensitive = StringComparison.InvariantCultureIgnoreCase;

            return _name.Equals(cookie._name, caseInsensitive)
                   && _value.Equals(cookie._value, caseSensitive)
                   && _path.Equals(cookie._path, caseSensitive)
                   && _domain.Equals(cookie._domain, caseInsensitive)
                   && _version == cookie._version;
        }

        /// <summary>
        /// Gets a hash code for the current cookie instance.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public override int GetHashCode()
        {
            return hash(
                     StringComparer.InvariantCultureIgnoreCase.GetHashCode(_name),
                     _value.GetHashCode(),
                     _path.GetHashCode(),
                     StringComparer.InvariantCultureIgnoreCase.GetHashCode(_domain),
                     _version
                   );
        }

        /// <summary>
        /// Returns a string that represents the current cookie instance.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public override string ToString()
        {
            return ToRequestString(null);
        }
    }
}
