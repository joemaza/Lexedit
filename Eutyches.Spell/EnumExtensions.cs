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
    /// Class EnumExtensions.
    /// </summary>
    /// <remarks>Base on code by Brian Lagunas (https://github.com/brianlagunas/BindingEnumsInWpf)</remarks>
    public static class EnumExtensions
    {
        #region Methods

        /// <summary>
        /// Retrieve the description on the enum, e.g. [Description("Bright Pink")] BrightPink = 2,
        /// Then when you pass in the enum, it will retrieve the description
        /// </summary>
        /// <param name="en">The Enumeration</param>
        /// <returns>A string representing the friendly name</returns>
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if(memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if(attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute) attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        /// <summary>
        /// Gets the value from description.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description">The description.</param>
        /// <returns>T.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException">Not found. - description</exception>
        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if(!type.IsEnum) throw new InvalidOperationException();
            foreach(var field in type.GetFields())
            {
                if(Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if(attribute.Description == description)
                        return (T) field.GetValue(null);
                }
                else
                {
                    if(field.Name == description)
                        return (T) field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
        }

        #endregion Methods
    }
}