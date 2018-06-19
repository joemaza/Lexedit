//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
//=============================================================================
//
//=============================================================================
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Eutyches.Spell.Hunspell
{
    /// <summary>
    /// Class Affix.
    /// </summary>
    /// <seealso cref="System.IComparable{Eutyches.Spell.Hunspell.Affix}"/>
    /// <seealso cref="Eutyches.Spell.IDeepClone{Eutyches.Spell.Hunspell.Affix}"/>
    [Serializable]
    public class Affix : HunspellObject, IComparable<Affix>, IDeepClone<Affix>, IComparer<Affix>
    {
        #region Constructors

        public Affix() { }

        #endregion Constructors

        #region Fields

        /// <summary>
        /// The default flag
        /// </summary>
        public const string DefaultFlag = "--";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance can combine.
        /// </summary>
        /// <value><c>true</c> if this instance can combine; otherwise, <c>false</c>.</value>
        [JsonProperty("Xpro")]
        public bool CanCombine { get; set; } = true;

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        [JsonProperty("Comments", Order = 100, Required = Required.AllowNull)]
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the flag.
        /// </summary>
        /// <value>The flag.</value>
        [JsonProperty(Required = Required.Always)]
        public string Flag { get; set; } = DefaultFlag;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary.
        /// </summary>
        /// <value><c>true</c> if this instance is primary; otherwise, <c>false</c>.</value>
        [JsonProperty("Pri")]
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        [JsonProperty("Lbl")]
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the rules.
        /// </summary>
        /// <value>The rules.</value>
        public List<Rule> Rules { get; set; } = new List<Rule>();

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public AffixType Type { get; set; } = AffixType.Unknown;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>Affix.</returns>
        public Affix Clone()
        {
            return ObjectExtensions.Clone(this);
        }

        /// <summary>
        /// Compares the specified left.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>System.Int32.</returns>
        int IComparer<Affix>.Compare(Affix left, Affix right)
        {
            if(left is null) return 1;
            if(right is null) return -1;
            if(ReferenceEquals(left, right)) return 0;

            return left.Flag.CompareTo(right.Flag);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer
        /// that indicates whether the current instance precedes, follows, or occurs in the same
        /// position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value
        /// has these meanings: Value Meaning Less than zero This instance precedes <paramref
        /// name="other"/> in the sort order. Zero This instance occurs in the same position in the
        /// sort order as <paramref name="other"/>. Greater than zero This instance follows <paramref
        /// name="other"/> in the sort order.
        /// </returns>
        public int CompareTo(Affix other)
        {
            if(other is null) return 1;
            if(ReferenceEquals(other, this)) return 0;

            return Flag.CompareTo(other.Flag);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return $"Flag: {Flag};Label = {Label};Rules = {Rules.Count}";
        }

        #endregion Methods
    }
}