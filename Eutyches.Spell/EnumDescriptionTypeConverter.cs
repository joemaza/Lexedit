//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
//=============================================================================
// Base on code by Brian Lagunas (https://github.com/brianlagunas/BindingEnumsInWpf)
//=============================================================================

using System;
using System.ComponentModel;
using System.Reflection;

namespace Eutyches.Spell
{
    /// <summary>
    /// Class EnumDescriptionTypeConverter.
    /// </summary>
    /// <remarks>Base on code by Brian Lagunas (https://github.com/brianlagunas/BindingEnumsInWpf)</remarks>
    /// <seealso cref="System.ComponentModel.EnumConverter"/>
    public class EnumDescriptionTypeConverter : EnumConverter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDescriptionTypeConverter"/> class.
        /// </summary>
        /// <param name="type">
        /// A <see cref="T:System.Type"/> that represents the type of enumeration to associate with
        /// this enumeration converter.
        /// </param>
        public EnumDescriptionTypeConverter(Type type)
        : base(type)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Converts the given value object to the specified destination type.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.
        /// </param>
        /// <param name="culture">
        /// An optional <see cref="T:System.Globalization.CultureInfo"/>. If not supplied, the
        /// current culture is assumed.
        /// </param>
        /// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
        /// <param name="destinationType">
        /// The <see cref="T:System.Type"/> to convert the value to.
        /// </param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the converted <paramref name="value"/>.
        /// </returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if(destinationType == typeof(string))
            {
                if(value != null)
                {
                    FieldInfo info = value.GetType().GetField(value.ToString());
                    if(info != null)
                    {
                        var attributes = (DescriptionAttribute[]) info.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        return ((attributes.Length > 0) && (!String.IsNullOrEmpty(attributes[0].Description))) ? attributes[0].Description : value.ToString();
                    }
                }
                return string.Empty;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion Methods
    }
}