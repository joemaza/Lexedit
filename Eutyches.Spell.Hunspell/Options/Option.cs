//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Newtonsoft.Json;
using System;

namespace Eutyches.Spell.Hunspell
{
    [Serializable]
    public class Option<T> : Toggle
    {
        #region Classes

        public sealed class Defaults
        {
            #region Fields

            public string LongFlags = "long";
            public string NumericFlags = "num";
            public string QwertyKey = "qwertyuiop|zxcvbnm|asdfghjkl|qaw|saz|wse|dsx|sz|edr|fdc|dx|rft|gfv|fc|tgy|hgb|gv|yhu|jhn|hb|uji|kjm|jn|iko|lkm";
            public string QwertzKey = "qwertzuiop|yxcvbnm|qaw|say|wse|dsx|sy|edr|fdc|dx|rft|gfv|fc|tgz|hgb|gv|zhu|jhn|hb|uji|kjm|jn|iko|lkm";
            public string SetUTF8 = "UTF-8";

            #endregion Fields
        }

        #endregion Classes

        #region Properties

        [JsonProperty("V")]
        public T Value { get; set; }

        #endregion Properties
    }
}