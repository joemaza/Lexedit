//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eutyches.Spell.Lexedit.Services.Models
{
    public class OperationNotification : Notification
    {
        #region Properties

        /// <summary>
        /// Gets or sets the operation performed. If it is a potentially harmful, this should be
        /// phrased as a question asking to continue.
        /// </summary>
        /// <value>The operation.</value>
        public string Operation { get; set; }

        #endregion Properties
    }
}