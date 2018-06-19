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
    public class OperationConfirmation : OperationNotification, IConfirmation
    {
        #region Properties

        /// <summary>
        /// Gets or sets the text for the affirmative (yes) button, the text that states what will be
        /// done, e.g., "Save the file"
        /// </summary>
        /// <value>The affirmation.</value>
        public string Affirmative { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can cancel.
        /// </summary>
        /// <value><c>true</c> if this instance can cancel; otherwise, <c>false</c>.</value>
        public bool CanCancel { get; set; }

        public string Caution { get; set; }

        /// <summary>
        /// Gets or sets a value indicating that the confirmation is confirmed.
        /// </summary>
        /// <value><c>true</c> if confirmed; otherwise, <c>false</c>.</value>
        public bool Confirmed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is a warning and that the operation
        /// can be harmful to data.
        /// </summary>
        /// <value><c>true</c> if this instance is warning; otherwise, <c>false</c>.</value>
        public bool IsWarning { get; set; }

        /// <summary>
        /// Gets or sets the text for the negative (no) buttons, the text that shows what will not be
        /// done, e.g. "Do not save the file".
        /// </summary>
        /// <value>The negation.</value>
        public string Negative { get; set; }

        #endregion Properties
    }
}