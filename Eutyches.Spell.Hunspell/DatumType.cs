//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System.ComponentModel;

namespace Eutyches.Spell.Hunspell
{
    /// <summary>
    /// Enum DatumType
    /// </summary>
    public enum DatumType
    {
        [Description("???")]
        Unknown,

        [Description("po")]
        Category,

        [Description("ph")]
        Phonetic,

        [Description("st")]
        Stem,

        [Description("al")]
        Allomorph,

        [Description("ds")]
        DerivationSuffix,

        [Description("is")]
        InflexionalSuffix,

        [Description("ts")]
        TerminalSuffix,

        [Description("sp")]
        SurfacePrefix,

        [Description("pa")]
        CompoundPart,

        [Description("dp")]
        DerivationalPrefix,

        [Description("ip")]
        InflectionalPrefix,

        [Description("tp")]
        TerminalPrefix
    }

    /// <summary>
    /// Class DatumTypeExtensions.
    /// </summary>
    public static class DatumTypeExtensions
    {
        #region Methods

        /// <summary>
        /// Froms the identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>DatumType.</returns>
        public static DatumType FromId(this string id)
        {
            return EnumExtensions.GetValueFromDescription<DatumType>(id);
        }

        /// <summary>
        /// To the identifier.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.String.</returns>
        public static string ToId(this DatumType type)
        {
            return EnumExtensions.GetDescription(type);
        }

        #endregion Methods
    }
}