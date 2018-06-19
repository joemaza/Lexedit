//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System;

namespace Eutyches.Spell.Hunspell
{
    [Serializable]
    public class OutputConversion : Conversion
    {
        #region Methods

        public override string ToText(bool addComments)
        {
            var line = $"OCONV {To} {From}";

            if(addComments)
                return AddComment(line);
            else
                return line;
        }

        #endregion Methods
    }
}