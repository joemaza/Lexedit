//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System;
using System.Collections.Generic;

namespace Eutyches.Spell.Hunspell
{
    [Serializable]
    public class SuggestionOptions : AffixOptions, IDeepClone<SuggestionOptions>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the forbid warn.
        /// </summary>
        /// <value>The forbid warn.</value>
        public Toggle ForbidWarn { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Option<string> Key { get; set; }

        /// <summary>
        /// Gets or sets the mappings.
        /// </summary>
        /// <value>The mappings.</value>
        public List<Map> Mappings { get; set; } = new List<Map>();

        public Option<int?> MaxCpdSugs { get; set; }

        public Option<int?> MaxDiff { get; set; }

        public Option<int?> MaxNGramSugs { get; set; }

        public Toggle NoSplitSugs { get; set; }

        public FlagOption NoSuggest { get; set; }

        public Toggle OnlyMaxDiff { get; set; }

        public List<Phone> PhoneticRules { get; set; } = new List<Phone>();

        public List<Rep> Replacements { get; set; } = new List<Rep>();

        public Toggle SugsWithDots { get; set; }

        public Option<string> Try { get; set; }

        public FlagOption Warn { get; set; }

        public SuggestionOptions Clone()
        {
            return ObjectExtensions.Clone<SuggestionOptions>(this);
        }

        #endregion Properties
    }
}