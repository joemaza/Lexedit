//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Eutyches.Spell.Lexedit.Converters
{
    public class PercentageConverter : MarkupExtension, IMultiValueConverter
    {
        #region Methods

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // If no values are present or there aren't two items
            if(values is null | values?.Length < 2)
            {
                return "N/A";
            }

            int divisor = System.Convert.ToInt32(values[1]);

            // Not going to try to divide by zero.
            if(divisor == 0)
            {
                return "N/A";
            }

            int dividend = System.Convert.ToInt32(values[0]);

            int ratio = (int) (dividend / (double) divisor * 100d);

            // Display: "dividend (~ ratio%)"
            return $"{dividend} (~ {ratio}%)";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        #endregion Methods
    }
}