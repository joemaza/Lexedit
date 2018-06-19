//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Eutyches.Spell.Lexedit.Properties;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Eutyches.Spell.Lexedit.Converters
{
    public class HeaderConverter : MarkupExtension, IMultiValueConverter
    {
        #region Methods

        public virtual object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var hasChanges = (values[0] as bool?) == true ? "*" : string.Empty;

            if(values[1] is null)
            {
                return $"{Resources.NewItem}{hasChanges}";
            }

            var content = values[1].ToString();

            return $"{content}{hasChanges}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #endregion Methods
    }
}