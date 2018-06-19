//=============================================================================
// Copyright © 2018 Eutyches Enterprises, LLC. All Rights Reserved.
//=============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Eutyches.Spell
{
    public sealed class EnumValues : MarkupExtension
    {
        #region Fields

        private readonly Type _enumType;

        #endregion Fields

        #region Constructors

        public EnumValues(Type enumType)
        {
            _enumType = enumType;
        }

        #endregion Constructors

        #region Methods

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(_enumType);
        }

        #endregion Methods
    }
}