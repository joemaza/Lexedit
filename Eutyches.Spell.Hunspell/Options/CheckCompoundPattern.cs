//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eutyches.Spell.Hunspell
{
    [Serializable]
    public class CheckCompoundPattern : ListItem
    {
        #region Properties

        [JsonProperty("EC")]
        public string EndChars { get; set; }

        [JsonProperty("EF")]
        public List<string> EndFlags { get; set; }

        [JsonProperty("R")]
        public string Replacement { get; set; }

        [JsonProperty("SC")]
        public string StartChars { get; set; }

        [JsonProperty("SF")]
        public List<string> StartFlags { get; set; }

        #endregion Properties

        #region Methods

        public override string ToText(bool addComments)
        {
            var startValue = StartFlags?.Count() > 0
                ? $"{StartChars}/{string.Join("", StartFlags)}"
                : $"{StartChars}";

            var endValue = EndFlags?.Count() > 0
                ? $"{EndChars}/{string.Join("", EndFlags)}"
                : $"{EndChars}";

            var line = $"{startValue}\t{endValue}";

            if(addComments)
                return AddComment(line);
            else
                return line;
        }

        #endregion Methods
    }
}