//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System;
using System.ComponentModel;
using System.Linq;

namespace Eutyches.Spell.Hunspell
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum FlagType
    {
        [Description("ASCII (e.g, A, X)")]
        Ascii,

        [Description("UTF-8 (e.g, A, X, but encoded in UNICODE)")]
        Utf8,

        [Description("Two-Character (e.g., XY, AB)")]
        Long,

        [Description("Numeric (e.g., 1234, 987)")]
        Numeric,
    }

    public static class FlagTypeExtension
    {
        #region Methods

        public static string GetDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

            return attribute != null ? attribute.Description : value.ToString();
        }

        #endregion Methods

        #region Classes

        private static class Values
        {
            #region Fields

            internal const string Ascii = "ASCII";
            internal const string Long = "long";
            internal const string Numeric = "num";
            internal const string Utf8 = "UTF8";

            #endregion Fields
        }

        #endregion Classes

        #region Methods

        public static string ToText(this FlagType t)
        {
            string text;
            switch(t)
            {
                case FlagType.Ascii:
                    text = Values.Ascii;
                    break;

                case FlagType.Utf8:
                    text = Values.Utf8;
                    break;

                case FlagType.Long:
                    text = Values.Long;
                    break;

                case FlagType.Numeric:
                    text = Values.Numeric;
                    break;

                default:
                    text = Values.Ascii;
                    break;
            }

            return text;
        }

        #endregion Methods
    }
}