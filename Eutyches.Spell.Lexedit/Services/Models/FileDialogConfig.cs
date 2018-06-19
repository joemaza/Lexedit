//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eutyches.Spell.Lexedit.Services.Models
{
    public class FileDialogConfig
    {
        #region Properties

        public string Extension { get; set; }

        public string FilePath { get; set; }

        public string Filter { get; set; }

        public string Title { get; set; }

        #endregion Properties
    }
}