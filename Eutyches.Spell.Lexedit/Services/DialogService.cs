//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using System;
using System.Windows;
using Eutyches.Spell.Lexedit.Services.Models;

using Ookii.Dialogs.Wpf;
using Microsoft.Win32;
using Eutyches.Spell.Lexedit.Properties;
using Eutyches.Spell.Lexedit.Services.Interfaces;

/// <summary>
/// The Services namespace.
/// </summary>
namespace Eutyches.Spell.Lexedit.Services
{
    public class DialogService : IDialogService
    {
        #region Fields

        /// <summary>
        /// The default dialog width
        /// </summary>
        private static readonly int DefaultDialogWidth = 200;

        /// <summary>
        /// The error dialog width
        /// </summary>
        private static readonly int ErrorDialogWidth = 464;

        private static readonly string StackHeader = "#============================== STACK TRACE ==============================#";
        private Window _owner;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogService"/> class.
        /// </summary>
        public DialogService() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogService"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public DialogService(Window owner)
        {
            Owner = owner;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public Window Owner
        {
            get => _owner ?? (_owner = Application.Current.MainWindow);

            protected set => _owner = value;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Shows the confirmation dialog with an "Affirmative", "Negative" and "Cancel" buttons.
        /// </summary>
        /// <param name="confirmation">The confirmation.</param>
        /// <param name="callback">The callback.</param>
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
        public void ShowConfirmationDialog(OperationConfirmation confirmation, Action<bool?> callback)
        {
            using(var dialog = new TaskDialog
            {
                WindowTitle = confirmation.Title,
                MainInstruction = confirmation.Operation,   // Text in bigger font
                Content = confirmation.Content.ToString(),  // Text in smaller font below. Additional information.
                AllowDialogCancellation = true,
                ExpandedByDefault = true,
                CenterParent = true,
                MainIcon = confirmation.IsWarning ? TaskDialogIcon.Warning : TaskDialogIcon.Information,
                Width = DefaultDialogWidth
            })
            {
                if(confirmation.IsWarning)
                {
                    dialog.FooterIcon = TaskDialogIcon.Information;
                    dialog.Footer = confirmation.Caution;
                }

                // Add the affirmative (Yes) and negative (No) buttons
                dialog.Buttons.Add(new TaskDialogButton(ButtonType.Custom) { Text = confirmation.Affirmative });
                dialog.Buttons.Add(new TaskDialogButton(ButtonType.Custom) { Text = confirmation.Negative, Default = true });

                // If this operation can be canceled, add the button
                if(confirmation.CanCancel)
                {
                    dialog.Buttons.Add(new TaskDialogButton(ButtonType.Cancel));
                }

                var result = dialog.ShowDialog(Owner);

                if(result.Text == confirmation.Affirmative)
                {
                    callback(true);
                }
                else if(result.Text == confirmation.Negative)
                {
                    callback(false);
                }
                else
                {
                    callback(null);
                }
            }
        }

        /// <summary>
        /// Shows a dialog with information about an error the occurred.
        /// </summary>
        /// <param name="notification">The notification.</param>
        public void ShowErrorDialog(ErrorNotification notification)
        {
            using(var dialog = new TaskDialog()
            {
                WindowTitle = notification.Title,
                MainInstruction = string.Format(Resources.ErrorDialogMessageFormat, notification.Operation),
                Content = notification.Content.ToString(),
                AllowDialogCancellation = true,
                ExpandedByDefault = true,
                CenterParent = true,
                MainIcon = TaskDialogIcon.Error,
                FooterIcon = TaskDialogIcon.Information,
                Width = (int) ErrorDialogWidth
            })
            {
                dialog.Buttons.Add(new TaskDialogButton(ButtonType.Close));

                dialog.ShowDialog(Owner);
            }
        }

        /// <summary>
        /// Shows a dialog with information about an unahandled error.
        /// </summary>
        /// <param name="notification">The notification.</param>
        public void ShowExceptionDialog(ExceptionNotification notification)
        {
            using(var dialog = new TaskDialog()
            {
                WindowTitle = notification.Title,
                MainInstruction = string.Format(Resources.ErrorDialogMessageFormat, notification.Operation),
                Content = notification.Exception.Message,
                AllowDialogCancellation = true,
                ExpandedByDefault = false,
                CenterParent = true,
                MainIcon = TaskDialogIcon.Error,
                Width = (int) ErrorDialogWidth,
                FooterIcon = TaskDialogIcon.Information,
                ExpandedControlText = $"Source: {notification.Exception.Source}",
                ExpandedInformation = $"{StackHeader}{Environment.NewLine}{notification.Exception.StackTrace}"
            })
            {
                dialog.Buttons.Add(new TaskDialogButton(ButtonType.Close));

                dialog.ShowDialog(Owner);
            };
        }

        /// <summary>
        /// Shows a dialog for the user to choose what file to open.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="callback">The callback.</param>
        public void ShowOpenFileDialog(FileDialogConfig config, Action<string> callback)
        {
            var dialog = new OpenFileDialog
            {
                Title = config.Title,
                Filter = config.Filter,
                DefaultExt = config.Extension,
                FileName = config.FilePath,
                ValidateNames = true,
                AddExtension = true,
                Multiselect = false,
                CheckFileExists = true,
            };

            if(dialog.ShowDialog(Owner) == true)
            {
                callback(dialog.FileName);
            }
        }

        /// <summary>
        /// Shows a dialog for the user to choose how to save a file.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="callback">The callback.</param>
        public void ShowSaveFileDialog(FileDialogConfig config, Action<string> callback)
        {
            var dialog = new SaveFileDialog
            {
                Title = config.Title,
                Filter = config.Filter,
                DefaultExt = config.Extension,
                FileName = config.FilePath,
                ValidateNames = true,
                AddExtension = true,
                CheckFileExists = false
            };

            if(dialog.ShowDialog(Owner) == true)
            {
                callback(dialog.FileName);
            }
        }

        /// <summary>
        /// Shows the select directory dialog.
        /// </summary>
        /// <param name="startingPath">The starting path.</param>
        /// <param name="callback">The callback.</param>
        public void ShowSelectDirectoryDialog(string startingPath, Action<string> callback)
        {
            var dialog = new VistaFolderBrowserDialog()
            {
                // UseDescriptionForTitle = true,
                ShowNewFolderButton = true,
                Description = Resources.ExportDictionaryFileDialogTitle,

                // If the path is empty, use a default path (MyDocuments)
                SelectedPath = (string.IsNullOrWhiteSpace(startingPath)
                    ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    : startingPath)
            };

            if(dialog.ShowDialog(Owner) == true)
            {
                callback(dialog.SelectedPath);
            }
        }

        #endregion Methods
    }
}