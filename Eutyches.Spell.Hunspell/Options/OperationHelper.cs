//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

namespace Eutyches.Spell.Hunspell
{
    public static class OperationHelper
    {
        #region Methods

        public static void Clear(this int flags, int value)
        {
            flags &= ~(value);
        }

        public static bool IsSet(this int flags, int value)
        {
            return (flags & value) == value;
        }

        public static void Set(this int flags, int value)
        {
            flags |= value;
        }

        #endregion Methods
    }
}