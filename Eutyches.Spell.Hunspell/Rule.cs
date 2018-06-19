//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eutyches.Spell.Hunspell
{
    /// <summary>
    /// Class Rule.
    /// </summary>
    /// <seealso cref="Eutyches.Spell.Hunspell.Morpheme"/>
    /// <seealso cref="System.IComparable{Eutyches.Spell.Hunspell.Rule}"/>
    [Serializable]
    public class Rule : Morpheme, IComparable<Rule>, IDeepClone<Rule>
    {
        #region Fields

        public const string DefaultCondition = ".";
        public const string DefaultStrip = "0";
        private string _condition;
        private string _strip;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Rule"/> class.
        /// </summary>
        public Rule() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rule"/> class.
        /// </summary>
        /// <param name="form">The form.</param>
        public Rule(string form) : base(form)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rule"/> class.
        /// </summary>
        /// <param name="other">The other.</param>
        public Rule(Rule other) : base(other)
        {
            Strip = other.Strip;
            Condition = other.Condition;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        /// <value>The condition.</value>
        [JsonProperty("C")]
        public string Condition
        {
            get
            {
                if(string.IsNullOrWhiteSpace(_condition))
                {
                    return DefaultCondition;
                }
                else
                {
                    return _condition;
                }
            }

            set => _condition = value;
        }

        /// <summary>
        /// Gets or sets the strip.
        /// </summary>
        /// <value>The strip.</value>
        [JsonProperty("R")]
        public string Strip
        {
            get
            {
                if(string.IsNullOrWhiteSpace(_strip))
                {
                    return DefaultStrip;
                }
                else
                {
                    return _strip;
                }
            }

            set => _strip = value;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Copies this instance.
        /// </summary>
        /// <returns></returns>
        public Rule Clone()
        {
            return ObjectExtensions.Clone(this);
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
        public int CompareTo(Rule other)
        {
            if(other is null)
                return 1;

            if(ReferenceEquals(other, this))
                return 0;

            if(Strip == other.Strip)
            {
                if(Form == other.Form)
                {
                    return Condition.CompareTo(other.Condition);
                }

                return Form.CompareTo(other.Form);
            }
            else
            {
                return Strip.CompareTo(other.Strip);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Returns a specially formated <see cref="String"/> for Hunspell
        /// </summary>
        /// <param name="affix">The affix.</param>
        /// <param name="lexicon">The lexicon.</param>
        /// <param name="addComments">if set to <c>true</c> [add comments].</param>
        /// <returns>System.String.</returns>
        public string ToText(Affix affix, Lexicon lexicon, bool addComments = false)
        {
            var sb = new StringBuilder($"{affix.Type.ToFlag()} {affix.Flag} {Strip} {Form}{GetFlags(lexicon)} {Condition}");

            // Fill out Data
            if(Data?.Count > 0)
            {
                sb.Append($" {string.Join(" ", Data)}");
            }

            if(addComments)
            {
                if(!string.IsNullOrWhiteSpace(Comments))
                {
                    sb.Append($" # {Comments}");
                }
            }

            return sb.ToString();
        }

        #endregion Methods
    }

    public class RuleComparer : IEqualityComparer<Rule>
    {
        #region Methods

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <paramref name="T"/> to compare.</param>
        /// <param name="y">The second object of type <paramref name="T"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the specified objects are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Rule x, Rule y)
        {
            //Check whether the compared objects reference the same data.
            if(object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if(x is null || y is null)
                return false;

            //Check whether the products' properties are equal.
            return x.Strip == y.Strip && x.Form == y.Form && x.Condition == y.Condition;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures
        /// like a hash table.
        /// </returns>
        public int GetHashCode(Rule rule)
        {
            //Check whether the object is null
            if(rule is null) return 0;

            //Get hash code for the Name field if it is not null.
            int hashForm = rule.Form == null ? 0 : rule.Form.GetHashCode();

            //Get hash code for the Code field.
            int hashStrip = rule.Form.GetHashCode();

            //Calculate the hash code for the product.
            return hashForm ^ hashStrip;
        }

        #endregion Methods
    }
}