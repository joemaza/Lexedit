//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Eutyches.Spell.Hunspell
{
    /// <summary>
    /// A collection of affixes and stems for a given language.
    /// </summary>
    /// <seealso cref="Eutyches.Spell.IDeepClone{Eutyches.Spell.Hunspell.Lexicon}"/>
    [Serializable]
    public class Lexicon : IDeepClone<Lexicon>
    {
        #region Fields

        private string _languageCode = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Lexicon"/> class.
        /// </summary>
        public Lexicon() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Lexicon"/> class.
        /// </summary>
        /// <param name="languageCode">The language code.</param>
        public Lexicon(string languageCode)
        {
            LanguageCode = languageCode;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the affixes.
        /// </summary>
        /// <value>The affixes.</value>
        [JsonProperty(Order = 100)]
        public List<Affix> Affixes { get; set; } = new List<Affix>();

        /// <summary>
        /// Gets or sets the compounding options.
        /// </summary>
        /// <value>The compounding options.</value>
        [JsonProperty(Order = 4)]
        public CompoundingOptions CompoundingOptions { get; set; }

        /// <summary>
        /// Gets or sets the conversion options.
        /// </summary>
        /// <value>The conversion options.</value>
        [JsonProperty(Order = 5)]
        public ConversionOptions ConversionOptions { get; set; }

        /// <summary>
        /// Gets or sets the general options.
        /// </summary>
        /// <value>The general options.</value>
        [JsonProperty(Order = 2)]
        public GeneralOptions GeneralOptions { get; set; } = new GeneralOptions();

        /// <summary>
        /// Gets the language code.
        /// </summary>
        /// <value>The language code.</value>
        [JsonProperty(Order = 0)]
        public string LanguageCode
        {
            get => _languageCode;

            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    _languageCode = string.Empty;
                    return;
                }

                _languageCode = value.ToLowerInvariant();

                if(_languageCode.StartsWith("de") | _languageCode.StartsWith("ger"))
                {
                    GeneralOptions.CheckSharpS = new Toggle();
                }
                else if(_languageCode.StartsWith("hun"))
                {
                    CompoundingOptions.IsHungarian = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the meta information.
        /// </summary>
        /// <value>The meta information.</value>
        [JsonProperty(Order = 1)]
        public FileMetaInfo MetaInfo { get; set; } = new FileMetaInfo();

        /// <summary>
        /// Gets or sets the stems.
        /// </summary>
        /// <value>The stems.</value>
        [JsonProperty(Order = 200)]
        public List<Stem> Stems { get; set; } = new List<Stem>();

        /// <summary>
        /// Gets or sets the suggestion options.
        /// </summary>
        /// <value>The suggestion options.</value>
        [JsonProperty(Order = 3)]
        public SuggestionOptions SuggestionOptions { get; set; } = new SuggestionOptions();

        /// <summary>
        /// Gets a value indicating whether to use numeric flags.
        /// </summary>
        /// <value><c>true</c> if set; otherwise, <c>false</c>.</value>
        public bool UseNumericFlags => GeneralOptions.Flag.Value == FlagType.Numeric;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>Lexicon.</returns>
        public Lexicon Clone()
        {
            return ObjectExtensions.Clone(this);
        }

        /// <summary>
        /// Gets the affix by flag.
        /// </summary>
        /// <param name="flag">The flag.</param>
        /// <returns>Affix.</returns>
        public Affix GetAffixByFlag(string flag)
        {
            return Affixes.SingleOrDefault(aff => aff.Flag == flag);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures
        /// like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            byte[] bytes;

            using(var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, this);
                bytes = stream.ToArray();
            }

            byte[] hash = new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(bytes);
            uint hashResult = 0;

            for(int i = 0; i < hash.Length; i++)
            {
                hashResult ^= (uint) (hash[i] << i % 4);
            }

            return (int) hashResult;
        }

        /// <summary>
        /// Gets the stem by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Stem.</returns>
        public Stem GetStemById(Guid id)
        {
            return Stems.Single(stem => stem.Id == id);
        }

        public Lexicon GetSubset(IEnumerable<Stem> stems)
        {
            var subset = this.Clone();
            subset.Affixes.Clear();
            subset.Stems.Clear();

            subset.Stems.AddRange(stems);

            var flags = new List<string>();

            foreach(var stem in stems)
            {
                flags.AddRange(GetReferencedAffixFlags(stem.Affixes));
            }

            foreach(var flag in flags)
            {
                subset.Affixes.Add(GetAffixByFlag(flag));
            }

            return subset;
        }

        /// <summary>
        /// Gets the referenced affixes.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        private IEnumerable<string> GetReferencedAffixFlags(IEnumerable<string> flags)
        {
            if(flags == null)
                return null;

            var affixFlags = new List<string>(flags);

            foreach(var flag in flags)
            {
                var affix = GetAffixByFlag(flag);

                if(affix == null)
                    continue;

                if(affix.Rules.Count > 0)
                {
                    foreach(var rule in affix.Rules)
                    {
                        if(rule.Affixes == null)
                            continue;

                        affixFlags.AddRange(GetReferencedAffixFlags(rule.Affixes));
                    }
                }
            }

            return affixFlags.Distinct();
        }

        #endregion Methods

        #region Classes

        /// <summary>
        /// Class Extension. This class cannot be inherited.
        /// </summary>
        public sealed class Extension
        {
            #region Fields

            /// <summary>
            /// The affix
            /// </summary>
            public const string Affix = ".aff";

            /// <summary>
            /// Binary extension (*.blex)
            /// </summary>
            public const string Binary = ".blex";

            /// <summary>
            /// The dictionary
            /// </summary>
            public const string Dictionary = ".dic";

            /// <summary>
            /// JSON extension (*.jlex)
            /// </summary>
            public const string Json = ".jlex";

            /// <summary>
            /// Zip extension (*.zlex)
            /// </summary>
            public const string Zip = ".zlex";

            #endregion Fields
        }

        /// <summary>
        /// Class Token. This class cannot be inherited.
        /// </summary>
        private sealed class Token
        {
            #region Fields

            public const char BeginComment = '#';
            public const char BeginDerived = '>';
            public const char BeginGroup = '{';
            public const char EndDerived = '<';
            public const char EndGroup = '}';

            #endregion Fields
        }

        #endregion Classes
    }
}