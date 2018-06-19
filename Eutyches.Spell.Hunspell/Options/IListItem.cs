//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

namespace Eutyches.Spell.Hunspell
{
    public interface IListItem
    {
        #region Properties

        string Comments { get; set; }

        #endregion Properties

        #region Methods

        string ToText(bool addComments);

        #endregion Methods
    }
}