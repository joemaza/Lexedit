﻿//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System;

namespace Eutyches.Spell.Hunspell
{
    [Serializable]
    public class Map : Conversion
    {
        #region Methods

        public override string ToText(bool addComments)
        {
            var line = $"MAP " + (From.Length > 1 ? $"({From})" : From) + (To.Length > 1 ? $"({To})" : To);

            if(addComments)
                return AddComment(line);
            else
                return line;
        }

        #endregion Methods
    }
}