//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Eutyches.Spell.Hunspell
{
    /// <summary>
    /// Class AffixFileOptions.
    /// </summary>
    [Serializable]
    public abstract class AffixOptions
    {
        #region Fields

        private const int OptionWidth = -24;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Converts the list to text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="list">The list.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        protected IEnumerable<string> ListToText<T>(string name, string tag, IEnumerable<T> list, bool addComments) where T : IListItem
        {
            var lines = new List<string>();

            if(list == null || list.Count() == 0)
            {
                return lines;
            }

            if(addComments)
            {
                lines.Add($"# {name.ToUpperInvariant()}");
            }

            lines.Add($"{tag.ToUpperInvariant()} {list.Count()}");

            foreach(var item in list)
            {
                lines.Add(item.ToText(addComments));
            }

            return lines;
        }

        /// <summary>
        /// Converts the property to its text equivalent and assigns it to <c>line</c>
        /// </summary>
        /// <param name="pi">The pi.</param>
        /// <param name="line">The line.</param>
        /// <returns><c>true</c> if a value can be retrieved, <c>false</c> otherwise.</returns>
        protected virtual bool PropertyToText(PropertyInfo pi, bool addComments, out string line)
        {
            line = string.Empty;

            bool isSet = false;
            var pType = pi.PropertyType;
            var pValue = pi.GetValue(this);
            var ucName = pi.Name.ToUpperInvariant();

            if(pType == typeof(Option<string>) || pType == typeof(Option<int>)
                 || pType == typeof(FlagOption))
            {
                var value = pType.GetProperty("Value").GetValue(pValue);
                var comments = addComments
                    ? pType.GetProperty("Comments").GetValue(pValue)?.ToString()
                    : null;

                // Return something only if the Value property is set
                if(value != null)
                {
                    line = string.IsNullOrWhiteSpace(comments)
                        ? $"{ucName,OptionWidth} {value}"
                        : $"{ucName,OptionWidth} {value} # {comments}";

                    isSet = true;
                }
            }
            else if(pType == typeof(Option<FlagType>))
            {
                var value = (FlagType) pType.GetProperty("Value").GetValue(pValue);
                var comments = addComments
                    ? pType.GetProperty("Comments").GetValue(pValue)?.ToString()
                    : null;

                // Return something only if the Value property is set
                line = string.IsNullOrWhiteSpace(comments)
                    ? $"{ucName,OptionWidth} {value.ToText()}"
                    : $"{ucName,OptionWidth} {value.ToText()} # {comments}";

                isSet = true;
            }
            else
            {
                var comments = pType.GetProperty("Comments").GetValue(pValue);

                line = string.IsNullOrWhiteSpace(comments?.ToString())
                    ? $"{ucName}"
                    : $"{ucName,OptionWidth} # {comments.ToString()}";

                isSet = true;
            }

            return isSet;
        }

        #endregion Methods
    }
}