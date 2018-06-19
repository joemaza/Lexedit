
using System.Collections.Generic;
using System.IO;

namespace Eutyches.Spell
{
    public static class TextFile
    {
        #region Fields

        public static class Token
        {
            #region Fields

            public const string BeginComment = "#";
            public const string NoOp = "-";
            public const string Semicolon = ";";

            #endregion Fields
        }

        #endregion Fields

        #region Methods

        /// <summary>
        /// Parses the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="includeNoOp">if set to <c>true</c> [include no op].</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        public static IEnumerable<string> Parse(string filePath, bool includeNoOp = false)
        {
            var content = new List<string>();

            var lines = File.ReadAllLines(filePath);

            foreach(var line in lines)
            {
                // Remove empty lines and lines with comments
                if(StartsWithComment(line, includeNoOp))
                {
                    continue;
                }

                content.Add(line.Trim());
            }

            return content;
        }

        /// <summary>
        /// Startses the with comment.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>System.Boolean.</returns>
        private static bool StartsWithComment(string input, bool allowNoOp)
        {
            var trimmed = input.Trim();

            if(trimmed.Length <= 0)
            {
                return true;
            }

            if(trimmed.StartsWith(Token.BeginComment))
            {
                return true;
            }

            if(trimmed.StartsWith(Token.NoOp))
            {
                if(!allowNoOp)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion Methods
    }
}