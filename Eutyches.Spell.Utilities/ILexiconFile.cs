//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Eutyches.Spell.Hunspell;

namespace Eutyches.Spell.Utilities
{
    public interface ILexiconFile
    {
        #region Methods

        /// <summary>
        /// Reads from the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>Lexicon.</returns>
        Lexicon Read(string filePath);

        /// <summary>
        /// Writes to the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="lexicon">The lexicon.</param>
        void Write(string filePath);

        #endregion Methods
    }
}