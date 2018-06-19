//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Eutyches.Spell.Lexedit.Converters
{
    [ValueConversion(sourceType: typeof(IEnumerable<string>), targetType: typeof(string))]
    public class ListToStringConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is null)
            {
                return null;
            }

            if(!(value is IEnumerable<string>))
            {
                throw new ArgumentException("Parameter must be of type IEnumerable{string}.", nameof(value));
            }

            var collection = value as IEnumerable<string>;

            if(collection.Count() == 0)
            {
                return null;
            }

            return string.Join(", ", value as IEnumerable<string>);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}