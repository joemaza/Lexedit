//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System;

namespace Eutyches.Spell.Hunspell
{
    /// <summary>
    /// Categories (Parts of Speech).
    /// </summary>
    /// <remarks>
    /// Each stem or root and each affix class can be fall under any number of different categories
    /// simultaneously. This is helpful when filtering for the appropriate affix for each stem or
    /// root. Soon to replace the <see cref="Category"/> enum.
    /// </remarks>
    [Flags]
    public enum PartOfSpeech
    {
        None = 0,

        /// <summary>
        /// Verbal - Stems that behave like verbs, or are verb-like and differentiate categories such
        /// as tense and aspect.
        /// </summary>
        Verbal = 0b0001,

        /// <summary>
        /// Nominal - Stems that behave like nouns, or are noun-like and can differentiate categories
        /// such as case and number.
        /// </summary>
        Nominal = 0b0010,

        /// <summary>
        /// Adjectival - Stems that behave like adjectives, or are adjective-like and can modify noun
        /// and/or noun phrases.
        /// </summary>
        Adjectival = 0b0100,

        /// <summary>
        /// Adverbial - Stems that modifies other adverbials, verbals and phrases.
        /// </summary>
        Adverbial = 0b1000,

        /// <summary>
        /// The quantifier
        /// </summary>
        Quantifier = 0b0001_0000,

        /// <summary>
        /// The determinative
        /// </summary>
        Determinative = 0b0010_0000,

        /// <summary>
        /// Pronominal - Stems that refer to an anaphor
        /// </summary>
        Pronominal = 0b0100_0000,

        /// <summary>
        /// Adpositional - Stems that show the relationship between two sentence constinuent
        /// </summary>
        Adpositional = 0b1000_0000,

        /// <summary>
        /// Interrogative - Stems that solicit information
        /// </summary>
        Interrogative = 0b0001_0000_0000,

        /// <summary>
        /// Interjection - A word that expresses emotion
        /// </summary>
        Interjection = 0b0010_0000_0000,

        /// <summary>
        /// Functional - Words that perform some function within the sentence or phrase
        /// </summary>
        Functional = 0b0100_0000_0000,

        /// <summary>
        /// Foreign - Foreign stems
        /// </summary>
        Foreign = 0b0001_0000_0000_0000,

        /// <summary>
        /// Other - Stems that do not fit under the other classifications
        /// </summary>
        Other = 0b1000_0000_0000_0000,
    }
}