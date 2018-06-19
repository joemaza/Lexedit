//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using Eutyches.Spell.Lexedit.Services;
using Eutyches.Spell.Lexedit.Services.Interfaces;
using Eutyches.Spell.Lexedit.Services.Models;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eutyches.Spell.Lexedit.ViewModels
{
    /// <summary>
    /// Base ViewModel with common dependencies and properties used in the application.
    /// </summary>
    /// <seealso cref="Prism.Mvvm.BindableBase"/>
    public abstract class ViewModelBase : BindableBase, IHasChanges
    {
        #region Fields

        protected readonly IDialogService _dialogService;
        protected readonly IEventAggregator _eventAggregator;
        protected readonly IFileService _fileService;
        protected readonly IToolService _toolService;

        protected bool _hasChanges;
        protected string _title;
        private bool _isActive;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="fileService">The file service.</param>
        /// <param name="dialogService">The dialog service.</param>
        public ViewModelBase(IEventAggregator eventAggregator, IFileService fileService, IDialogService dialogService, IToolService toolService)
        {
            _eventAggregator = eventAggregator;
            _fileService = fileService;
            _dialogService = dialogService;
            _toolService = toolService;

            _eventAggregator.GetEvent<FileCreatedEvent>().Subscribe(() => OnFileCreated(), ThreadOption.UIThread, true);
            _eventAggregator.GetEvent<FileOpenedEvent>().Subscribe(() => OnFileOpened(), ThreadOption.UIThread, true);
            _eventAggregator.GetEvent<FileSavedEvent>().Subscribe(() => OnFileSaved(), ThreadOption.UIThread, true);
            _eventAggregator.GetEvent<FileClosedEvent>().Subscribe(() => OnFileClosed(), ThreadOption.UIThread, true);
            _eventAggregator.GetEvent<FileDataChangedEvent>().Subscribe(() => OnFileDataChanged(), ThreadOption.UIThread, true);
        }

        /// <summary>
        /// Called when file data changed.
        /// </summary>
        protected virtual void OnFileDataChanged()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance has changes.
        /// </summary>
        /// <value><c>true</c> if this instance has changes; otherwise, <c>false</c>.</value>
        public bool HasChanges
        {
            get => _hasChanges;
            protected set => SetProperty(ref _hasChanges, value, nameof(HasChanges));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has an active lexicon. The lexicon
        /// can be loaded from a file or it is a newly created lexicon that has not been saved to
        /// file yet.
        /// </summary>
        /// <value><c>true</c> if this instance is lexicon active; otherwise, <c>false</c>.</value>
        public bool IsActive
        {
            get => _isActive;
            protected set => SetProperty(ref _isActive, value, nameof(IsActive));
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value, nameof(Title));
        }

        /// <summary>
        /// Clears the has changes value;
        /// </summary>
        public void ClearHasChanges()
        {
            HasChanges = false;

            _eventAggregator.GetEvent<FileDataChangedEvent>().Publish();
        }

        /// <summary>
        /// Sets the has changes value;
        /// </summary>
        public void SetHasChanges()
        {
            HasChanges = true;

            _eventAggregator.GetEvent<FileDataChangedEvent>().Publish();
        }

        /// <summary>
        /// Called when the file is closed.
        /// </summary>
        protected virtual void OnFileClosed()
        {
            IsActive = _fileService.IsLoaded;
        }

        /// <summary>
        /// Called when the file is created.
        /// </summary>
        protected virtual void OnFileCreated()
        {
            IsActive = _fileService.IsLoaded;
        }

        /// <summary>
        /// Called when the file is opened.
        /// </summary>
        protected virtual void OnFileOpened()
        {
            IsActive = _fileService.IsLoaded;
        }

        /// <summary>
        /// Called when the file is saved.
        /// </summary>
        protected virtual void OnFileSaved()
        {
            IsActive = _fileService.IsLoaded;
        }

        #endregion Properties
    }
}