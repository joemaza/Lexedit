//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using Eutyches.Spell.Hunspell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eutyches.Spell.Lexedit.Services.Interfaces
{
    public interface IFileService
    {
        #region Properties

        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <value>The file path.</value>
        string FilePath { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has changes.
        /// </summary>
        /// <value><c>true</c> if this instance has changes; otherwise, <c>false</c>.</value>
        bool HasChanges { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has file name.
        /// </summary>
        /// <value><c>true</c> if this instance has file name; otherwise, <c>false</c>.</value>
        bool HasFileName { get; }

        /// <summary>
        /// Gets a value indicating whether a file is loaded.
        /// </summary>
        /// <value><c>true</c> if a file is loaded; otherwise, <c>false</c>.</value>
        bool IsLoaded { get; }

        /// <summary>
        /// Gets the lexicon.
        /// </summary>
        /// <value>The lexicon.</value>
        Lexicon Lexicon { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Clears the <c>HasChanges</c> value (i.e., <c>false</c>).
        /// </summary>
        void ClearHasChanges();

        /// <summary>
        /// Closes the lexicon.
        /// </summary>
        void CloseLexicon();

        /// <summary>
        /// Create a new lexicon.
        /// </summary>
        void NewLexicon();

        Task<IEnumerable<Affix>> OpenAffixFileAsync(string filePath);

        Task OpenLexiconFileAsync(string filePath);

        Task<IEnumerable<Stem>> OpenStemFileAsync(string filePath);

        Task SaveAffixFileAsync(string filePath);

        Task SaveLexiconFileAsync(string filePath);

        Task SaveStemFileAsync(string filePath);

        /// <summary>
        /// Sets the <c>HasChanges</c> value (i.e., <c>true</c>).
        /// </summary>
        void SetHasChanges();

        #endregion Methods
    }
}