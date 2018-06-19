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
    public class ExceptionNotification : ErrorNotification
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionNotification"/> class.
        /// </summary>
        public ExceptionNotification() : base() { }

        /// <summary>
        /// Gets or sets the exception thrown.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; set; }

        #endregion Constructors
    }
}