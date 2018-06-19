//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
namespace Eutyches.Spell.Lexedit.ViewModels
{
    /// <summary>
    /// Interface used to indicate that the object has changes (i.e., is dirty)
    /// </summary>
    public interface IHasChanges
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance has changes.
        /// </summary>
        /// <value><c>true</c> if this instance has changes; otherwise, <c>false</c>.</value>
        bool HasChanges { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Clears the has changes.
        /// </summary>
        void ClearHasChanges();

        /// <summary>
        /// Sets the has changes.
        /// </summary>
        void SetHasChanges();

        #endregion Methods
    }
}