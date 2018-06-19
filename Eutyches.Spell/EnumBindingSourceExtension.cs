//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System;
using System.Windows.Markup;

namespace Eutyches.Spell
{
    /// <summary>
    /// Class EnumBindingSourceExtension.
    /// </summary>
    /// <remarks>Base on code by Brian Lagunas (https://github.com/brianlagunas/BindingEnumsInWpf)</remarks>
    /// <seealso cref="System.Windows.Markup.MarkupExtension"/>
    public class EnumBindingSourceExtension : MarkupExtension
    {
        #region Fields

        private Type _type;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBindingSourceExtension"/> class.
        /// </summary>
        public EnumBindingSourceExtension() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBindingSourceExtension"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public EnumBindingSourceExtension(Type type)
        {
            Type = type;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type
        {
            get { return _type; }

            set
            {
                if(value != _type)
                {
                    if(!(value is null))
                    {
                        Type enumType = Nullable.GetUnderlyingType(value) ?? value;

                        if(!enumType.IsEnum)
                            throw new ArgumentException("Type must be an Enum.");
                    }

                    _type = value;
                }
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// When implemented in a derived class, returns an object that is provided as the value of
        /// the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">
        /// A service provider helper that can provide services for the markup extension.
        /// </param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        /// <exception cref="InvalidOperationException">The EnumType must be specified.</exception>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if(null == _type)
            {
                throw new InvalidOperationException("The EnumType must be specified.");
            }

            Type actualEnumType = Nullable.GetUnderlyingType(_type) ?? _type;

            Array enumValues = Enum.GetValues(actualEnumType);

            if(actualEnumType == _type)
            {
                return enumValues;
            }

            Array tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);

            return tempArray;
        }

        #endregion Methods
    }
}