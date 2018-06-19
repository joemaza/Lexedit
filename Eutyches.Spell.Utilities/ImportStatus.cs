//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

namespace Eutyches.Spell.Utilities
{
    /// <summary>
    /// Enum ImportStatus
    /// </summary>
    public enum ImportStatus
    {
        /// <summary>
        /// The status is unknown
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// The information was parsed correctly
        /// </summary>
        Succeeded = 0,

        /// <summary>
        /// There is a warning
        /// </summary>
        Warning = 1,

        /// <summary>
        /// There is an error
        /// </summary>
        Error = 100
    }
}