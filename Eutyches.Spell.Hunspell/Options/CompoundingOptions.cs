//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System;
using System.Collections.Generic;

namespace Eutyches.Spell.Hunspell
{
    [Serializable]
    public class CompoundingOptions : AffixOptions, IDeepClone<CompoundingOptions>
    {
        #region Properties

        public List<Break> Breaks { get; set; } = new List<Break>();

        public Toggle CheckCompoundCase { get; set; }

        public Toggle CheckCompoundDup { get; set; }

        public List<CheckCompoundPattern> CheckCompoundPatterns { get; set; } = new List<CheckCompoundPattern>();

        public Toggle CheckCompoundRep { get; set; }

        public Toggle CheckCompoundTriple { get; set; }

        public FlagOption CompoundBegin { get; set; }

        public FlagOption CompoundFlag { get; set; }

        public FlagOption CompoundForbidFlag { get; set; }

        public FlagOption CompoundLast { get; set; }

        public FlagOption CompoundMiddle { get; set; }

        public Option<int?> CompoundMin { get; set; }

        public FlagOption CompoundPermitFlag { get; set; }

        public FlagOption CompoundRoot { get; set; }

        public List<CompoundRule> CompoundRules { get; set; } = new List<CompoundRule>();

        public Option<string> CompoundSyllable { get; set; }

        public Option<int?> CompoundWordMax { get; set; }

        public FlagOption ForceUCase { get; set; }

        public bool IsHungarian { get; set; } = false;

        public FlagOption OnlyInCompound { get; set; }

        public Toggle SimplifiedTriple { get; set; }

        public FlagOption SyllableNum { get; set; }

        public CompoundingOptions Clone()
        {
            return ObjectExtensions.Clone<CompoundingOptions>(this);
        }

        #endregion Properties
    }
}