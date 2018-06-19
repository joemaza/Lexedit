//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Eutyches.Spell.Lexedit.Properties;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eutyches.Spell.Lexedit.Services.Models
{
    public class ErrorNotification : OperationNotification
    {
        #region Constructors

        public ErrorNotification()
        {
            Title = Resources.ErrorDialogTitle;
        }

        #endregion Constructors
    }
}