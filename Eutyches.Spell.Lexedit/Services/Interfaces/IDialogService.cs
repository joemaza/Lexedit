//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using Eutyches.Spell.Lexedit.Services.Models;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Eutyches.Spell.Lexedit.Services.Interfaces
{
    public interface IDialogService
    {
        #region Properties

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        Window Owner { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Shows the confirmation dialog with an "Affirmative", "Negative" and "Cancel" buttons.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The terms "Affirmative" and "Negative" are relative since the buttons' texts will depend
        /// on context. For example, the user might be asked to save a file before proceeding; the
        /// texts for the buttons might be "Save the file" (Affirmative) and "Close the file anyway" (Negative).
        /// </para>
        /// Clicking on one of the buttons results in the following:
        /// <list type="bullet">
        /// <listheader>Buttons</listheader>
        /// <item>Affirmative - The system performs the operation and contues with the next operation.</item>
        /// <item>
        /// Negative - The system does not perform the operation and continues with the next operation.
        /// </item>
        /// <item>
        /// Cancel - The system does not perform the operation AND does not continue with any
        /// subsequents operations.
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="confirmation">The confirmation.</param>
        /// <param name="callback">The callback.</param>
        void ShowConfirmationDialog(OperationConfirmation confirmation, Action<bool?> callback);

        /// <summary>
        /// Shows a dialog with information about an error the occurred.
        /// </summary>
        /// <param name="notification">The notification.</param>
        void ShowErrorDialog(ErrorNotification notification);

        /// <summary>
        /// Shows a dialog with information about an unahandled error.
        /// </summary>
        /// <param name="notification">The notification.</param>
        void ShowExceptionDialog(ExceptionNotification notification);

        /// <summary>
        /// Shows a dialog for the user to choose what file to open.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="callback">The callback.</param>
        void ShowOpenFileDialog(FileDialogConfig config, Action<string> callback);

        /// <summary>
        /// Shows a dialog for the user to choose how to save a file.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="callback">The callback.</param>
        void ShowSaveFileDialog(FileDialogConfig config, Action<string> callback);

        /// <summary>
        /// Shows the select directory dialog.
        /// </summary>
        /// <param name="startingPath">The starting path.</param>
        /// <param name="callback">The callback.</param>
        void ShowSelectDirectoryDialog(string startingPath, Action<string> callback);

        #endregion Methods
    }
}