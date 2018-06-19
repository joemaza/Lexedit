//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Newtonsoft.Json;
using System;

namespace Eutyches.Spell.Hunspell
{
    /// <summary>
    /// A Hunspell option specified by a flag.
    /// </summary>
    /// <seealso cref="Eutyches.Spell.Hunspell.Option"/>
    [Serializable]
    public class FlagOption : Toggle
    {
        #region Enums

        public static class Default
        {
            #region Fields

            public const string CheckSharpS = "$~";
            public const string Circumfix = "><";
            public const string CompoundBegin = "@!";
            public const string CompoundFlag = "@@";
            public const string CompoundForbidFlag = "@-";
            public const string CompoundLast = "@?";
            public const string CompoundMiddle = "@|";
            public const string CompoundPermitFlag = "@+";
            public const string CompoundRoot = "@%";
            public const string ForbiddenWord = "!!";
            public const string ForceUCase = "@^";
            public const string KeepCase = "^^";
            public const string NeedAffix = "++";
            public const string NoSuggest = "~~";
            public const string OnlyInCompound = "@=";
            public const string Substandard = "$$";
            public const string SyllableNum = "@$";
            public const string Warn = "%%";

            #endregion Fields
        }

        #endregion Enums

        #region Properties

        [JsonProperty("V")]
        public string Value { get; set; }

        #endregion Properties
    }
}