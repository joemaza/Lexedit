//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Eutyches.Spell.Hunspell
{
    [Serializable]
    public class Rep : Conversion, IComparable<Rep>
    {
        #region Properties

        [JsonProperty("R")]
        public bool IsReversable { get; set; }

        #endregion Properties

        #region Methods

        public int CompareTo(Rep other)
        {
            return $"{From}{To}".CompareTo($"{other.From}{other.To}");
        }

        public IEnumerable<string> ToLines(bool addComments)
        {
            var lines = new List<string>
            {
                ToText(addComments)
            };

            if(IsReversable)
            {
                var reversed = new Rep { From = To, To = From, Comments = Comments };
                lines.Add(reversed.ToText(addComments));
            }

            return lines;
        }

        public override string ToText(bool addComments)
        {
            var line = $"REP {From} {To}";

            if(addComments)
                return AddComment(line);
            else
                return line;
        }

        #endregion Methods
    }
}