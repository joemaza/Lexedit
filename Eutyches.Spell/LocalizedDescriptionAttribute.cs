//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
//=============================================================================
// Base on code by Brian Lagunas (https://github.com/brianlagunas/BindingEnumsInWpf)
//=============================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Eutyches.Spell
{
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        #region Fields

        private readonly string _resourceKey;
        private readonly ResourceManager _resourceManager;

        #endregion Fields

        #region Constructors

        public LocalizedDescriptionAttribute(string resourceKey, Type resourceType)
        {
            _resourceManager = new ResourceManager(resourceType);
            _resourceKey = resourceKey;
        }

        #endregion Constructors

        #region Properties

        public override string Description
        {
            get
            {
                string description = _resourceManager.GetString(_resourceKey);

                return string.IsNullOrWhiteSpace(description) ? $"[[{_resourceKey}]]" : description;
            }
        }

        #endregion Properties
    }
}