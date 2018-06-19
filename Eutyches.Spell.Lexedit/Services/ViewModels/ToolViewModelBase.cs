//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using Eutyches.Spell.Lexedit.Services.Interfaces;
using Eutyches.Spell.Lexedit.Services.Models;
using Eutyches.Spell.Lexedit.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Windows.Input;

namespace Eutyches.Spell.Lexedit.Services.ViewModels
{
    public abstract class ToolViewModelBase : BindableBase, IHasChanges
    {
        #region Fields

        protected readonly IDialogService _dialogService;
        protected readonly IEventAggregator _eventAggregator;
        protected readonly IFileService _fileService;
        protected readonly IToolService _toolService;
        protected bool _hasChanges = false;

        #endregion Fields

        #region Constructors

        public ToolViewModelBase(IEventAggregator eventAggregator, IFileService fileService, IDialogService dialogService, IToolService toolService)
        {
            _eventAggregator = eventAggregator;
            _fileService = fileService;
            _dialogService = dialogService;
            _toolService = toolService;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the accept command.
        /// </summary>
        /// <value>The accept command.</value>
        public ICommand AcceptCommand => new DelegateCommand(() => OnAcceptClicked(), () => CanAccept).ObservesProperty(() => HasChanges);

        /// <summary>
        /// Gets the cancel command.
        /// </summary>
        /// <value>The cancel command.</value>
        public ICommand CancelCommand => new DelegateCommand(() => OnCancelClicked());

        /// <summary>
        /// Gets a value indicating whether this instance has changes.
        /// </summary>
        /// <value><c>true</c> if this instance has changes; otherwise, <c>false</c>.</value>
        public bool HasChanges
        {
            get => _hasChanges;
            set => SetProperty(ref _hasChanges, value, nameof(HasChanges));
        }

        public virtual string Title => $"TOOLWINDOW: {GetType().Name} ";

        /// <summary>
        /// Gets a value indicating whether this instance can accept.
        /// </summary>
        /// <value><c>true</c> if this instance can accept; otherwise, <c>false</c>.</value>
        protected virtual bool CanAccept => HasChanges;

        /// <summary>
        /// Called when Cancel is clicked. Raises the CancelClicked event closing the window.
        /// </summary>
        protected virtual void OnCancelClicked()
        {
            CancelClicked?.Invoke(this, null);
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Occurs when the "Accept (OK)" button is clicked.
        /// </summary>
        public event EventHandler AcceptClicked;

        /// <summary>
        /// Occurs when the "Cancel" button is clicked.
        /// </summary>
        public event EventHandler CancelClicked;

        /// <summary>
        /// Clears the has changes value.
        /// </summary>
        public void ClearHasChanges()
        {
            HasChanges = false;
        }

        /// <summary>
        /// Sets the has changes value.
        /// </summary>
        public void SetHasChanges()
        {
            HasChanges = true;
        }

        /// <summary>
        /// Called when Accept is clicked. Raises the AcceptClicked event closing the window.
        /// </summary>
        protected virtual void OnAcceptClicked()
        {
            if(HasChanges)
            {
                _eventAggregator.GetEvent<FileDataChangedEvent>().Publish();
            }

            AcceptClicked?.Invoke(this, null);
        }

        #endregion Methods
    }
}