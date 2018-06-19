//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Eutyches.Spell
{
    public static class StringHelper
    {
        #region Fields

        private const int DefaultLineLength = 120;
        private const int DefaultPathLength = 48;
        private const string EscapeToken = "%";
        private const string WildChars = "*%#@?_";

        //private static Regex UnescapeRegex = new Regex(@"(?<!\\)(?:\\u[0-9a-fA-F]{4}|\\U[0-9a-fA-F]{8})", RegexOptions.Compiled);
        private static Regex UnescapeRegex = new Regex(@"(?<!\\)(?:%[0-9a-fA-F]{2})", RegexOptions.Compiled);

        #endregion Fields

        #region Methods

        /// <summary>
        /// Compacts the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.String.</returns>
        public static string CompactPath(this string path, int length = DefaultPathLength)
        {
            // StringBuild must be instantiated with a length.
            StringBuilder sb = new StringBuilder(length + 1);
            NativeMethods.PathCompactPathEx(sb, path, length, 0);
            return sb.ToString();
        }

        /// <summary>
        /// Escapes the specified characters.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="characters">The characters.</param>
        /// <returns>System.String.</returns>
        public static string Escape(this string input, IEnumerable<char> characters)
        {
            // Cycle through the characters that need to be escaped. If one is present in the string,
            // escape; otherwise, don't bother.

            if(input?.IndexOfAny(characters.ToArray()) == -1)
            {
                return input;
            }

            string escaped = input;

            foreach(var c in characters)
            {
                escaped = escaped.Replace(c.ToString(), $"%{(int) c:X2}");
            }

            return escaped;
        }

        /// <summary>
        /// Splits to lines.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>System.String.</returns>
        public static string SplitToLines(this string s, int maxLength = DefaultLineLength)
        {
            return Regex.Replace(s, @"(.{1," + maxLength + @"})(?:\s|$)", $"$1{Environment.NewLine}");
        }

        /// <summary>
        /// To the regex.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>System.String.</returns>
        public static string ToRegex(this string s)
        {
            s = Regex.Escape(s.Trim());

            if(s.HasSqlWildcard())
            {
                s = s.Replace("*", @".+")
                    .Replace("%", @".*")
                    .Replace("#", @"\d")
                    .Replace("@", @"[a-zA-Z]")
                    .Replace("?", @"\w")
                    .Replace("_", @"\w");
            }
            else
            {
                s = $@".*{s}.*";
            }

            return s;
        }

        /// <summary>
        /// Unescapes the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>System.String.</returns>
        public static string Unescape(this string input)
        {
            // Unescaping is a expensive process because of the the regex involved. Unless the string
            // has an escape sequence, let's not bother.
            if(input.Contains(EscapeToken))
            {
                return UnescapeRegex.Replace(input,
                    m =>
                    {
                        if(m.Value.IndexOf("%", StringComparison.Ordinal) > -1)
                            return char.ConvertFromUtf32(int.Parse(m.Value.Replace("%", ""), NumberStyles.HexNumber));
                        return Regex.Unescape(m.Value);
                    });
            }

            return input;
        }

        /// <summary>
        /// Determines whether [has SQL wildcard] [the specified s].
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns><c>true</c> if [has SQL wildcard] [the specified s]; otherwise, <c>false</c>.</returns>
        private static bool HasSqlWildcard(this string s)
        {
            if(string.IsNullOrWhiteSpace(s))
            {
                return false;
            }

            return s.IndexOfAny(WildChars.ToCharArray()) != -1;
        }

        private static class NativeMethods
        {        /// <summary>
            #region Methods

            /// Pathes the compact path ex. </summary> <param name="pszOut">The PSZ out.</param>
            /// <param name="szPath">The sz path.</param> <param name="cchMax">The CCH
            /// maximum.</param> <param name="dwFlags">The dw flags.</param> <returns><c>true</c> if
            /// XXXX, <c>false</c> otherwise.</returns>
            [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
            internal static extern bool PathCompactPathEx([Out] StringBuilder pszOut, string szPath, int cchMax, int dwFlags);

            #endregion Methods
        }

        #endregion Methods
    }
}