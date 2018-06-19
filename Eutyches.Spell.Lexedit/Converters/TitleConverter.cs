//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Eutyches.Spell.Lexedit.Properties;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Markup;

namespace Eutyches.Spell.Lexedit.Converters
{
    public class TitleConverter : MarkupExtension, IMultiValueConverter
    {
        #region Methods

        /// <summary>
        /// Converts source values to a value for the binding target. The data binding engine calls
        /// this method when it propagates the values from source bindings to the binding target.
        /// </summary>
        /// <param name="values">
        /// The array of values that the source bindings in the <see
        /// cref="T:System.Windows.Data.MultiBinding"/> produces. The value <see
        /// cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the source binding
        /// has no value to provide for conversion.
        /// </param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value.If the method returns null, the valid null value is used.A return value
        /// of <see cref="T:System.Windows.DependencyProperty"/>. <see
        /// cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the converter did
        /// not produce a value, and that the binding will use the <see
        /// cref="P:System.Windows.Data.BindingBase.FallbackValue"/> if it is available, or else will
        /// use the default value.A return value of <see cref="T:System.Windows.Data.Binding"/>. <see
        /// cref="F:System.Windows.Data.Binding.DoNothing"/> indicates that the binding does not
        /// transfer the value or use the <see
        /// cref="P:System.Windows.Data.BindingBase.FallbackValue"/> or the default value.
        /// </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Expected order of value [0] IsActive, [1] HasChanges, [2] FilePath

            if((values[0] as bool?) == true)
            {
                // If the file has changes, add an asterisk (the typical signal that an item has changes)
                var hasChanges = (values[1] as bool?) == true ? "*" : string.Empty;

                // If the file has a name, prepend it to the AppTitle. If it does not have a name yet
                // (i.e., it has not been save yet), use something like "[new lexicon]" instead.
                var fileName = (string.IsNullOrWhiteSpace(values[2] as string))
                    ? $"{Resources.NewFileName}{hasChanges} - "
                    : $"{Path.GetFileName(values[2] as string)}{hasChanges} - ";

                return $"{fileName}{Resources.AppTitle}";
            }
            else
            {
                return Resources.AppTitle;
            }
        }

        /// <summary>
        /// Converts a binding target value to the source binding values.
        /// </summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">
        /// The array of types to convert to. The array length indicates the number and types of
        /// values that are suggested for the method to return.
        /// </param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// An array of values that have been converted from the target value back to the source values.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        #endregion Methods
    }
}