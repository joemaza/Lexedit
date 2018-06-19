//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Newtonsoft.Json;
using System;

namespace Eutyches.Spell.Hunspell
{
    [Serializable]
    public abstract class Conversion : ListItem
    {
        #region Properties

        [JsonProperty("F")]
        public string From { get; set; }

        [JsonProperty("T")]
        public string To { get; set; }

        #endregion Properties
    }
}