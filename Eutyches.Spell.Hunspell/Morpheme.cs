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
    /// Class Morpheme.
    /// </summary>
    [Serializable]
    public abstract class Morpheme : HunspellObject, IMorpheme
    {
        #region Fields

        /// <summary>
        /// The default form
        /// </summary>
        public const string DefaultForm = "0";

        private List<string> _affixes;
        private string _form;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Morpheme"/> class.
        /// </summary>
        public Morpheme() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Morpheme"/> class.
        /// </summary>
        /// <param name="form">The form.</param>
        public Morpheme(string form)
        {
            Form = form;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Morpheme"/> class.
        /// </summary>
        /// <param name="other">The other.</param>
        public Morpheme(Morpheme other)
        {
            Form = other.Form;
            Comments = other.Comments;
            RequisiteValues = other.RequisiteValues;
            SuggestionValues = other.SuggestionValues;
            CompoundingValues = other.CompoundingValues;

            Affixes = ObjectExtensions.Clone(other.Affixes);
            Data = ObjectExtensions.Clone(other.Data);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the flags.
        /// </summary>
        /// <value>The flags.</value>
        [JsonProperty("A", Order = 10, Required = Required.Default)]
        public List<string> Affixes
        {
            // No need to filter for distinct values, but order the items
            get => _affixes?.OrderBy(a => a).ToList();

            set
            {
                if(value is null || value.Count <= 0)
                {
                    _affixes = null;
                    return;
                }

                // Clear the instance, and add only distinct items.

                if(_affixes is null)
                {
                    _affixes = value.Distinct().ToList();
                }
                else
                {
                    _affixes.Clear();
                    _affixes.AddRange(value.Distinct());
                }
            }
        }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        [JsonProperty("C", Order = 100, Required = Required.Default)]
        public string Comments { get; set; } = null;

        /// <summary>
        /// Gets or sets the compounding.
        /// </summary>
        /// <value>The compounding.</value>
        [JsonProperty("P", Order = 90, Required = Required.Default)]
        public Compounding.Value CompoundingValues { get; set; } = Hunspell.Compounding.Value.Default;

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        [JsonProperty("D", Order = 20, Required = Required.Default)]
        public List<Datum> Data { get; set; } = null;

        /// <summary>
        /// Gets the data as string.
        /// </summary>
        /// <value>The data as string.</value>
        [JsonIgnore]
        public string DataAsString => Data?.Count > 0 ? string.Join(" ", Data) : string.Empty;

        /// <summary>
        /// Gets or sets the form.
        /// </summary>
        /// <value>The form.</value>
        [JsonProperty("M", Order = 0, Required = Required.Always)]
        public string Form
        {
            get
            {
                if(string.IsNullOrWhiteSpace(_form))
                {
                    return DefaultForm;
                }
                else
                {
                    return _form;
                }
            }

            set => _form = value;
        }

        /// <summary>
        /// Gets or sets the requesite values.
        /// </summary>
        /// <value>The operation.</value>
        [JsonProperty("Q", Order = 50, Required = Required.Default)]
        public Requisites.Value RequisiteValues { get; set; } = Hunspell.Requisites.Value.None;

        /// <summary>
        /// Gets or sets the suggestion values.
        /// </summary>
        /// <value>The suggestion.</value>
        [JsonProperty("G", Order = 60, Required = Required.Default)]
        public Suggestion.Value SuggestionValues { get; set; } = Hunspell.Suggestion.Value.Default;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder(Form);

            if(Affixes?.Count > 0)
            {
                sb.Append($" /[{string.Join(",", Affixes)}]");
            }

            if(Data?.Count > 0)
            {
                sb.Append($" :{string.Join(",", Data)}");
            }

            if(!string.IsNullOrWhiteSpace(Comments))
            {
                sb.Append($" # {Comments}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the flags.
        /// </summary>
        /// <param name="lexicon">The lexicon.</param>
        /// <returns>System.String.</returns>
        protected string GetFlags(Lexicon lexicon)
        {
            var flags = new List<string>();

            // Add affix flags
            if(Affixes?.Count > 0)
            {
                flags.AddRange(Affixes);
            }

            // Add flags for compounding
            if(CompoundingValues != Hunspell.Compounding.Value.Default)
            {
                flags.AddRange(CompoundingValues.ToFlags(lexicon));
            }

            // Add flags for suggestion
            if(SuggestionValues != Hunspell.Suggestion.Value.Default)
            {
                flags.AddRange(SuggestionValues.ToFlags(lexicon));
            }

            // Add flags for requisites
            if(RequisiteValues != Hunspell.Requisites.Value.None)
            {
                flags.AddRange(RequisiteValues.ToFlags(lexicon));
            }

            if(flags?.Count > 0)
            {
                string delimiter = lexicon.GeneralOptions.Flag.Value == FlagType.Numeric ? "," : "";

                return $"/{(string.Join(delimiter, flags))}";
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Methods
    }
}