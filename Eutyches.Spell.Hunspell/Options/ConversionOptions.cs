//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Eutyches.Spell.Hunspell
{
    [Serializable]
    public class ConversionOptions : AffixOptions, IDeepClone<ConversionOptions>
    {
        #region Properties

        [JsonProperty("Inputs")]
        public List<InputConversion> InputConversions { get; set; } = new List<InputConversion>();

        [JsonProperty("Outputs")]
        public List<OutputConversion> OutputConversions { get; set; } = new List<OutputConversion>();

        public ConversionOptions Clone()
        {
            return ObjectExtensions.Clone<ConversionOptions>(this);
        }

        #endregion Properties
    }
}