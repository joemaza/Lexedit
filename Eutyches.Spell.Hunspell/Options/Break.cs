//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Newtonsoft.Json;
using System;

namespace Eutyches.Spell.Hunspell
{
    [Serializable]
    public class Break : ListItem
    {
        #region Properties

        #region Properties

        [JsonProperty("Value")]
        public string Value { get; set; }

        #endregion Properties

        #endregion Properties

        #region Methods

        public override string ToText(bool addComments)
        {
            var line = $"BREAK {Value}";
            return base.AddComment(line);
        }

        #endregion Methods
    }
}