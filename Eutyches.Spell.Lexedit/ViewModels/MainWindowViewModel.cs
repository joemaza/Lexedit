//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using Eutyches.Spell.Lexedit.Properties;
using Eutyches.Spell.Lexedit.Services.Interfaces;
using Eutyches.Spell.Lexedit.Services.Models;
using Prism.Commands;
using Prism.Events;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Eutyches.Spell.Lexedit.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private bool _canSave;
        private string _filePath;
        private string _statusText = Resources.StatusReady;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="fileService">The file service.</param>
        /// <param name="dialogService">The dialog service.</param>
        public MainWindowViewModel(IEventAggregator eventAggregator, IFileService fileService, IDialogService dialogService, IToolService toolService)
        : base(eventAggregator, fileService, dialogService, toolService)
        {
            App.Current.MainWindow.Closing += OnViewClosing;
        }

        /// <summary>
        /// Handles the <see cref="E:ViewClosing"/> event and checks for any unsaved data. If there
        /// is anything not saved, the user is prompted to save the file.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void OnViewClosing(object sender, CancelEventArgs e)
        {
            if(!HasChanges)
            {
                return;
            }

            // The file has changes. Prompt the user to save the file.
            _dialogService.ShowConfirmationDialog(
                new OperationConfirmation
                {
                    Title = Resources.ExitAppDialogTitle,
                    Content = Resources.ExitAppDialogExtra,
                    Operation = Resources.ExitAppDialogQuestion,

                    // Buttons
                    Affirmative = Resources.ExitAppDialogAffirmative,
                    Negative = Resources.ExitAppDialogNegative,
                    CanCancel = true,
                },
                async (result) =>
                {
                    // User click Yes
                    if(result == true)
                    {
                        // Attempt to save the file. If the file does not yet have a file name, the
                        // user will be prompted. Even in that situation, if the user cancels, no
                        // further action will be taken.
                        if(await SaveLexiconAsync() != true)
                        {
                            e.Cancel = true;
                        }
                    }

                    // User click No
                    else if(result == false)
                    {
                        return;
                    }

                    // User click Cancel
                    else
                    {
                        e.Cancel = true;
                    }
                });
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance can save.
        /// </summary>
        /// <value><c>true</c> if this instance can save; otherwise, <c>false</c>.</value>
        public bool CanSave
        {
            get => _canSave;
            set => SetProperty(ref _canSave, value, nameof(CanSave));
        }

        public ICommand CloseCommand => new DelegateCommand(OnCloseCommand).ObservesCanExecute(() => IsActive);

        public ICommand ExitCommand => new DelegateCommand(OnExitCommand);

        public ICommand ExportAffixesToJsonFileCommand
            => new DelegateCommand(OnExportAffixesToJsonFileCommand).ObservesCanExecute(() => IsActive);

        public ICommand ExportAffixesToTextFileCommand
            => new DelegateCommand(OnExportAffixesToTextFileCommand).ObservesCanExecute(() => IsActive);

        public ICommand ExportStemsToJsonFileCommand
            => new DelegateCommand(OnExportStemsToJsonFileCommand).ObservesCanExecute(() => IsActive);

        public ICommand ExportStemsToTextFileCommand
            => new DelegateCommand(OnExportStemsToTextFileCommand).ObservesCanExecute(() => IsActive);

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath
        {
            get => _filePath;
            set => SetProperty(ref _filePath, value, nameof(FilePath));
        }

        public ICommand ImportAffixesFromJsonFileCommand
            => new DelegateCommand(OnImportAffixesFromJsonFileCommand).ObservesCanExecute(() => IsActive);

        public ICommand ImportAffixesFromLexiconFileCommand
            => new DelegateCommand(OnImportAffixesFromLexiconFileCommand).ObservesCanExecute(() => IsActive);

        public ICommand ImportAffixesFromTextFileCommand
            => new DelegateCommand(OnImportAffixesFromTextFileCommand).ObservesCanExecute(() => IsActive);

        public ICommand ImportStemsFromJsonFileCommand
            => new DelegateCommand(OnImportStemsFromJsonFileCommand).ObservesCanExecute(() => IsActive);

        public ICommand ImportStemsFromLexiconFileCommand
            => new DelegateCommand(OnImportStemsFromLexiconFileCommand).ObservesCanExecute(() => IsActive);

        public ICommand ImportStemsFromTextFileCommand
            => new DelegateCommand(OnImportStemsFromTextFileCommand).ObservesCanExecute(() => IsActive);

        public ICommand NewCommand => new DelegateCommand(async () => await OnNewCommandAsync());

        public ICommand OpenCommand => new DelegateCommand(async () => await OnOpenCommandAsync());

        public ICommand SaveAsCommand => new DelegateCommand(OnSaveAsCommand).ObservesCanExecute(() => IsActive);

        public ICommand SaveCommand => new DelegateCommand(async () => await OnSaveCommandAsync()).ObservesCanExecute(() => CanSave);

        /// <summary>
        /// Gets or sets the status text.
        /// </summary>
        /// <value>The status text.</value>
        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value, nameof(StatusText));
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Saves the lexicon as.
        /// </summary>
        /// <returns><c>true</c> if the lexicon was saved, <c>false</c> otherwise.</returns>
        public bool? SaveLexiconAs()
        {
            bool? result = null;

            _dialogService.ShowSaveFileDialog(
                new FileDialogConfig
                {
                    Title = Resources.SaveLexiconFileDialogTitle,
                    Filter = Resources.LexiconFileFilter,
                    Extension = Resources.LexiconFileDefaultExtension
                },
                async (filePath) =>
                {
                    try
                    {
                        await _fileService.SaveLexiconFileAsync(filePath);
                        result = true;
                    }
                    catch(Exception ex)
                    {
                        _dialogService.ShowErrorDialog(
                            new ErrorNotification
                            {
                                Operation = Resources.SaveLexiconFileDialogTitle,
                                Content = ex.Message
                            });
                    }
                });

            return result;
        }

        /// <summary>
        /// save lexicon as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;System.Nullable&lt;System.Boolean&gt;&gt;.</returns>
        public async Task<bool?> SaveLexiconAsync()
        {
            if(!_fileService.HasFileName)
            {
                return SaveLexiconAs();
            }

            try
            {
                await _fileService.SaveLexiconFileAsync(null);

                return true;
            }
            catch(Exception ex)
            {
                _dialogService.ShowExceptionDialog(
                    new ExceptionNotification
                    {
                        Operation = Resources.SaveLexiconFileDialogTitle,
                        Exception = ex.InnerException
                    });

                return null;
            }
        }

        /// <summary>
        /// Called when [file closed].
        /// </summary>
        protected override void OnFileClosed()
        {
            StatusText = Resources.StatusLexiconClosed;

            IsActive = _fileService.IsLoaded;
            FilePath = _fileService.FilePath;
            HasChanges = false;
        }

        /// <summary>
        /// Called when [file created].
        /// </summary>
        protected override void OnFileCreated()
        {
            StatusText = Resources.StatusLexiconCreated;

            IsActive = _fileService.IsLoaded;
            FilePath = _fileService.FilePath;
            HasChanges = false;
        }

        /// <summary>
        /// Called when file data changed.
        /// </summary>
        protected override void OnFileDataChanged()
        {
            StatusText = string.Format(Resources.StatusLexiconDataChangedFormat, _fileService.FilePath.CompactPath());

            IsActive = _fileService.IsLoaded;
            FilePath = _fileService.FilePath;
            HasChanges = true;
            CanSave = true;
        }

        /// <summary>
        /// Called when [file opened].
        /// </summary>
        protected override void OnFileOpened()
        {
            StatusText = string.Format(Resources.StatusLexiconOpenedFormat, _fileService.FilePath.CompactPath());

            IsActive = _fileService.IsLoaded;
            FilePath = _fileService.FilePath;
            HasChanges = false;
            CanSave = false;
        }

        /// <summary>
        /// Called when [file saved].
        /// </summary>
        protected override void OnFileSaved()
        {
            StatusText = string.Format(Resources.StatusLexiconSavedFormat, _fileService.FilePath.CompactPath());

            IsActive = _fileService.IsLoaded;
            FilePath = _fileService.FilePath;
            HasChanges = false;
            CanSave = false;
        }

        /// <summary>
        /// Called when [close command].
        /// </summary>
        private void OnCloseCommand()
        {
            if(!HasChanges)
            {
                _fileService.CloseLexicon();
            }

            // The file has changes. Prompt the user to save the file.
            _dialogService.ShowConfirmationDialog(
                new OperationConfirmation
                {
                    Title = Resources.CloseLexiconFileDialogTitle,
                    Content = Resources.CloseLexiconFileDialogExtra,
                    Operation = Resources.CloseLexiconFileDialogQuestion,

                    // Buttons
                    Affirmative = Resources.CloseLexiconFileDialogAffirmative,
                    Negative = Resources.CloseLexiconFileDialogNegative,
                    CanCancel = true,
                },
                async (result) =>
                {
                    if(result == true)
                    {
                        // Attempt to save the file. If the file does not yet have a file name, the
                        // user will be prompted. Even in that situation, if the user cancels, no
                        // further action will be taken.
                        if(await SaveLexiconAsync() == true)
                        {
                            _fileService.CloseLexicon();
                        }
                    }
                    else if(result == false)
                    {
                        _fileService.CloseLexicon();
                    }
                });
        }

        /// <summary>
        /// Called when [exit command].
        /// </summary>
        /// <returns>Task.</returns>
        private void OnExitCommand()
        {
            App.Current.MainWindow.Close();
        }

        /// <summary>
        /// Called when [export affixes to json file command].
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnExportAffixesToJsonFileCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [export affixes to text file command].
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnExportAffixesToTextFileCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [export stems to json file command].
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnExportStemsToJsonFileCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [export stems to text file command].
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnExportStemsToTextFileCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [import affixes from json file command].
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnImportAffixesFromJsonFileCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [import affixes from lexicon file command].
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnImportAffixesFromLexiconFileCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [import affixes from text file command].
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnImportAffixesFromTextFileCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [import stems from json file command].
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnImportStemsFromJsonFileCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [import stems from lexicon file command].
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnImportStemsFromLexiconFileCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [import stems from text file command].
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnImportStemsFromTextFileCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// on new command as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task OnNewCommandAsync()
        {
            // If the file has changes and an attempt to save the file was canceled or was not
            // successful, return doing nothing further.
            if(_fileService.HasChanges)
            {
                if(await SaveLexiconAsync() != true)
                {
                    return;
                }
            }

            _fileService.NewLexicon();
        }

        /// <summary>
        /// on open command as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task OnOpenCommandAsync()
        {
            // The current lexicon has changes. Attempt to save the file, prompting the user as
            // necessary. Should they cancel or an error occur resulting in NULL, return.
            if(_fileService.HasChanges)
            {
                if(await SaveLexiconAsync() == null)
                {
                    return;
                }
            }

            _dialogService.ShowOpenFileDialog(
                new FileDialogConfig
                {
                    Title = Resources.OpenLexiconFileDialogTitle,
                    Filter = Resources.LexiconFileFilter,
                    Extension = Resources.LexiconFileDefaultExtension
                },
                async (filePath) =>
                {
                    try
                    {
                        await _fileService.OpenLexiconFileAsync(filePath);
                    }
                    catch(Exception ex)
                    {
                        _dialogService.ShowErrorDialog(
                            new ErrorNotification
                            {
                                Operation = Resources.OpenLexiconFileDialogTitle,
                                Content = ex.Message
                            });
                    }
                });
        }

        /// <summary>
        /// Called when [save as command].
        /// </summary>
        private void OnSaveAsCommand()
        {
            SaveLexiconAs();
        }

        /// <summary>
        /// on save command as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task OnSaveCommandAsync()
        {
            await SaveLexiconAsync();
        }

        #endregion Methods
    }
}