//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eutyches.Spell.Hunspell
{
    /// <summary>
    /// Class Stem.
    /// </summary>
    /// <seealso cref="Eutyches.Spell.Hunspell.Morpheme"/>
    /// <seealso cref="System.IComparable{Eutyches.Spell.Hunspell.Stem}"/>
    /// <seealso cref="Eutyches.Spell.IDeepClone{Eutyches.Spell.Hunspell.Stem}"/>
    [Serializable]
    public class Stem : Morpheme, IComparable, IComparable<Stem>, IDeepClone<Stem>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Stem"/> class.
        /// </summary>
        public Stem() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stem"/> class.
        /// </summary>
        /// <param name="other">The other.</param>
        public Stem(Stem other) : base(other)
        {
            Id = other.Id;
            RootId = other.RootId;
            BaseId = other.BaseId;
            Category = other.Category;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the base identifier.
        /// </summary>
        /// <value>The base identifier.</value>
        [JsonProperty("B")]
        public Guid? BaseId { get; set; } = null;

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        [JsonProperty("K")]
        public Category Category { get; set; } = Category.None;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the root identifier.
        /// </summary>
        /// <value>The root identifier.</value>
        [JsonProperty("R")]
        public Guid? RootId { get; set; } = null;

        /// <summary>
        /// Gets or sets the sense.
        /// </summary>
        /// <value>The sense.</value>
        [JsonProperty("S")]
        public string Sense { get; set; } = null;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <remarks>
        /// Creates a clone of the current instance with the same data, but all new references.
        /// </remarks>
        /// <returns>A cloned copy of this instance.</returns>
        public Stem Clone()
        {
            return ObjectExtensions.Clone(this);
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>System.Int32.</returns>
        public int CompareTo(Stem other)
        {
            if(other is null)
                return 1;

            if(ReferenceEquals(other, this))
                return 0;

            return Form.CompareTo(other.Form);
        }

        public int CompareTo(object other)
        {
            if(other is null)
                return 1;

            if(other is Stem)
                return CompareTo(other as Stem);

            throw new ArgumentException("Parameter is an incorrect type.", nameof(other));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return $"{Form} ({Category})";
        }

        /// <summary>
        /// Returns a specially formated <see cref="String"/> for Hunspell
        /// </summary>
        /// <param name="lexicon">The lexicon.</param>
        /// <param name="addComments">if set to <c>true</c> [add comments].</param>
        /// <returns>System.String.</returns>
        public string ToText(Lexicon lexicon, bool addComments)
        {
            var sb = new StringBuilder($"{Form}{GetFlags(lexicon)}");

            // Fill out Data
            var data = new List<Datum>();

            if(Data?.Count > 0)
            {
                data.AddRange(Data);
            }

            // Set Category
            if(Category != Category.None)
            {
                data.Insert(0, new Datum(DatumType.Category, Category.ToTag()));
            }

            //Get Stems
            if(!(RootId is null))
            {
                var root = lexicon.Stems.FirstOrDefault(r => r.Id == RootId);

                if(!(root is null))
                {
                    data.Add(new Datum(DatumType.Stem, root.Form));
                }
            }

            var derivatives = lexicon.Stems.Where(s => s.RootId == Id);

            foreach(var d in derivatives)
            {
                data.Add(new Datum(DatumType.Allomorph, d.Form));
            }

            if(data?.Count > 0)
            {
                sb.Append($" {string.Join(" ", data)}");
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
}