//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System;

/// <summary>
/// The Hunspell namespace.
/// </summary>
namespace Eutyches.Spell.Hunspell
{
    [Serializable]
    public class GeneralOptions : AffixOptions, IDeepClone<GeneralOptions>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the check sharp s. (German only)
        /// </summary>
        /// <value>The check sharp s.</value>
        public Toggle CheckSharpS { get; set; } = null;

        /// <summary>
        /// Gets or sets the circumfix.
        /// </summary>
        /// <value>The circumfix.</value>
        public FlagOption Circumfix { get; set; } = null;

        /// <summary>
        /// Gets or sets the complex prefixes.
        /// </summary>
        /// <value>The complex prefixes.</value>
        public Toggle ComplexPrefixes { get; set; } = null;

        /// <summary>
        /// Gets or sets the flag.
        /// </summary>
        /// <value>The flag.</value>
        public Option<FlagType> Flag { get; set; } = new Option<FlagType>
        {
            Value = FlagType.Long,
            Comments = "Long flags, i.e., two-character flags (e.g. 'AB', 'XY')"
        };

        /// <summary>
        /// Gets or sets the forbidden word.
        /// </summary>
        /// <value>The forbidden word.</value>
        public FlagOption ForbiddenWord { get; set; } = new FlagOption
        {
            Value = FlagOption.Default.ForbiddenWord,
            Comments = "Used to mark misspellings or inadmissible forms."
        };

        /// <summary>
        /// Gets or sets the full strip.
        /// </summary>
        /// <value>The full strip.</value>
        public Toggle FullStrip { get; set; } = null;

        /// <summary>
        /// Gets or sets the home.
        /// </summary>
        /// <value>The home.</value>
        public Option<string> Home { get; set; } = new Option<string>
        {
            Value = "[TBD]",
            Comments = "To be determined"
        };

        /// <summary>
        /// Gets or sets the ignore.
        /// </summary>
        /// <value>The ignore.</value>
        public Option<string> Ignore { get; set; }

        /// <summary>
        /// Gets or sets the keep case.
        /// </summary>
        /// <value>The keep case.</value>
        public FlagOption KeepCase { get; set; } = new FlagOption
        {
            Value = FlagOption.Default.KeepCase,
            Comments = "Used to mark forms that need to maintain case sensitivity to be valid."
        };

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>The language.</value>
        public Option<string> Lang { get; set; }

        /// <summary>
        /// Gets or sets the need affix.
        /// </summary>
        /// <value>The need affix.</value>
        public FlagOption NeedAffix { get; set; } = new FlagOption
        {
            Value = FlagOption.Default.NeedAffix,
            Comments = "Used to mark forms that are not valid words without some affixation."
        };

        /// <summary>
        /// Gets or sets the set.
        /// </summary>
        /// <value>The set.</value>
        public Option<string> Set { get; set; }

        public FlagOption Substandard { get; set; } = new FlagOption
        {
            Value = FlagOption.Default.Substandard,
            Comments = "Used to mark substandard or dialectical forms."
        };

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public Option<string> Version { get; set; } = new Option<string> { Value = "1.0" };

        /// <summary>
        /// Gets or sets the word chars.
        /// </summary>
        /// <value>The word chars.</value>
        public Option<string> WordChars { get; set; }

        public GeneralOptions Clone()
        {
            return ObjectExtensions.Clone<GeneralOptions>(this);
        }

        #endregion Properties
    }
}