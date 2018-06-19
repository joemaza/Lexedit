//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Eutyches.Spell.Hunspell.Properties;
using System.ComponentModel;

namespace Eutyches.Spell.Hunspell
{
    /// <summary>
    /// Enum Category
    /// </summary>
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Category
    {
        /// <summary>
        /// None (Default)
        /// </summary>
        [Description("Not Categorized")]
        [LocalizedDescription("CategoryUncategorized", typeof(Resources))]
        None = 0,

        /// <summary>
        /// A noun or substantive
        /// </summary>
        [Description("Nominal")]
        [LocalizedDescription("CategoryNominal", typeof(Resources))]
        Noun = 1,

        /// <summary>
        /// A verb or verbal word
        /// </summary>
        [Description("Verbal")]
        [LocalizedDescription("CategoryVerbal", typeof(Resources))]
        Verb = 2,

        /// <summary>
        /// An adjective
        /// </summary>
        [Description("Adjectival")]
        [LocalizedDescription("CategoryAdjectival", typeof(Resources))]
        Adjective = 3,

        /// <summary>
        /// An adverb, modifies verbs, adjectives, other adverbs, phrases or sentences. Those
        /// modifying phrases and sentences include sentence particles like "kadi", "kuma", etc.
        /// </summary>
        [Description("Adverbial")]
        [LocalizedDescription("CategoryAdverbial", typeof(Resources))]
        Adverb = 4,

        /// <summary>
        /// The interjections, e.g., "Apo!", "Ala!", and social formulas, e.g., "Adios".
        /// </summary>
        [Description("Interjection")]
        [LocalizedDescription("CategoryInterjection", typeof(Resources))]
        Interjection = 5,

        /// <summary>
        /// A stem related to numbers
        /// </summary>
        [Description("Numeric")]
        [LocalizedDescription("CategoryNumeric", typeof(Resources))]
        Numeric = 6,

        /// <summary>
        /// The determiners, e.g., "ti", "ni", "dayta", etc.
        /// </summary>
        [Description("Determinative")]
        [LocalizedDescription("CategoryDeterminative", typeof(Resources))]
        Determinative = 7,

        /// <summary>
        /// The pronouns, words that can replace
        /// </summary>
        [Description("Pronominal")]
        [LocalizedDescription("CategoryPronominal", typeof(Resources))]
        Pronoun = 8,

        /// <summary>
        /// The conjunctions
        /// </summary>
        [Description("Conjunction")]
        [LocalizedDescription("CategoryConjunctive", typeof(Resources))]
        Conjunction = 9,

        /// <summary>
        /// The adposition
        /// </summary>
        [Description("Adposition")]
        [LocalizedDescription("CategoryAdpositional", typeof(Resources))]
        Adposition = 10,

        /// <summary>
        /// The question
        /// </summary>
        [Description("Interrogative")]
        [LocalizedDescription("CategoryInterrogative", typeof(Resources))]
        Question = 11,

        /// <summary>
        /// Foreign words or phrases that have not been assimilated
        /// </summary>
        [Description("Foreign")]
        [LocalizedDescription("CategoryForeign", typeof(Resources))]
        Foreign = 50,

        /// <summary>
        /// Particles such as "a", "nga", or other functional word
        /// </summary>
        [Description("Functional")]
        [LocalizedDescription("CategoryFunctional", typeof(Resources))]
        Functional = 90,

        /// <summary>
        /// The stem does not fall within one of the categories above
        /// </summary>
        [Description("Other")]
        [LocalizedDescription("CategoryOther", typeof(Resources))]
        Other = 100
    }

    /// <summary>
    /// Class CategoryExtension.
    /// </summary>
    public static class CategoryExtension
    {
        #region Methods

        /// <summary>
        /// Converts a <see cref="Category"/> tag to the corresponding <see cref="Category"/> value ("xxx").
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Category</returns>
        public static Category ParseTag(this string value)
        {
            Category category = Category.None;

            switch(value.ToLowerInvariant())
            {
                case "n":
                case "nn":
                    category = Category.Noun;
                    break;

                case "v":
                case "vv":
                case "vb":
                    category = Category.Verb;
                    break;

                case "adj":
                case "aj":
                case "jj":
                    category = Category.Adjective;
                    break;

                case "adv":
                case "av":
                case "rb":
                    category = Category.Adverb;
                    break;

                case "det":
                case "dt":
                    category = Category.Determinative;
                    break;

                case "pro":
                case "pp":
                    category = Category.Pronoun;
                    break;

                case "con":
                case "conj":
                case "cj":
                    category = Category.Conjunction;
                    break;

                case "fw":
                case "for":
                    category = Category.Foreign;
                    break;

                case "fun":
                case "fn":
                    category = Category.Functional;
                    break;

                case "num":
                case "no":
                case "cd":
                    category = Category.Numeric;
                    break;

                case "uh":
                    category = Category.Interjection;
                    break;

                case "in":
                case "ap":
                    category = Category.Adposition;
                    break;

                case "wh":
                case "qw":
                    category = Category.Question;
                    break;

                case "oth":
                case "oo":
                    category = Category.Other;
                    break;

                default:
                    category = Category.None;
                    break;
            }

            return category;
        }

        /// <summary>
        /// Converts the <see cref="Category"/> value to the corresponding tag: ("xx")
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>System.String.</returns>
        public static string ToTag(this Category value)
        {
            string tag = "xx";

            switch(value)
            {
                case Category.Adjective:
                    tag = "jj";
                    break;

                case Category.Adposition:
                    tag = "in";
                    break;

                case Category.Adverb:
                    tag = "rb";
                    break;

                case Category.Conjunction:
                    tag = "cj";
                    break;

                case Category.Determinative:
                    tag = "dt";
                    break;

                case Category.Functional:
                    tag = "fn";
                    break;

                case Category.Interjection:
                    tag = "uh";
                    break;

                case Category.Noun:
                    tag = "nn";
                    break;

                case Category.Numeric:
                    tag = "no";
                    break;

                case Category.Other:
                    tag = "oo";
                    break;

                case Category.Pronoun:
                    tag = "pp";
                    break;

                case Category.Question:
                    tag = "wh";
                    break;

                case Category.Verb:
                    tag = "vb";
                    break;

                case Category.Foreign:
                    tag = "ff";
                    break;

                default:
                    tag = "xx";
                    break;
            }

            return tag;
        }

        /// <summary>
        /// Converts the category to a string.
        /// </summary>
        /// <param name="category">The c.</param>
        /// <returns>System.String.</returns>
        public static string ToText(this Category category)
        {
            return $"po:{category.ToTag()}";
        }

        #endregion Methods
    }
}