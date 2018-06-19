//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Newtonsoft.Json;
using System;

namespace Eutyches.Spell.Hunspell
{
    [Serializable]
    public class CompoundRule : ListItem
    {
        #region Properties

        [JsonProperty("V")]
        public string Value { get; set; }

        #endregion Properties

        #region Methods

        public override string ToText(bool addComments)
        {
            var line = $"COMPOUNDRULE {Value}";

            if(addComments)
                return AddComment(line);
            else
                return line;
        }

        #endregion Methods
    }
}