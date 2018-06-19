//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Newtonsoft.Json;
using System;

namespace Eutyches.Spell.Hunspell
{
    /// <summary>
    /// Class Toggle, an option that clear if NULL; set, if not NULL.
    /// </summary>
    [Serializable]
    public class Toggle
    {
        #region Properties

        [JsonProperty("C")]
        public string Comments { get; set; }

        #endregion Properties
    }
}