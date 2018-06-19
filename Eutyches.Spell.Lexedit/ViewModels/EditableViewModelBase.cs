//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Eutyches.Spell.Lexedit.Services.Interfaces;
using Eutyches.Spell.Lexedit.Services.Models;
using Prism.Events;
using Prism.Mvvm;

namespace Eutyches.Spell.Lexedit.ViewModels
{
    public abstract class EditableViewModelBase<TData>
        : ViewModelBase, IEditableObject, INotifyDataErrorInfo, IEditableData<TData> where TData : class, new()
    {
        #region Fields

        protected ErrorsContainer<string> _errors;

        #endregion Fields

        #region Constructors

        protected EditableViewModelBase(IEventAggregator eventAggregator, IFileService fileService, IDialogService dialogService, IToolService toolService)
            : base(eventAggregator, fileService, dialogService, toolService)
        {
            _errors = new ErrorsContainer<string>(e => RaiseErrorsChanged(e));
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire entity.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = null;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>The data.</value>
        public TData Data { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors.
        /// </summary>
        /// <value><c>true</c> if this instance has errors; otherwise, <c>false</c>.</value>
        public bool HasErrors => _errors.HasErrors;

        /// <summary>
        /// Gets the backup.
        /// </summary>
        /// <value>The backup.</value>
        protected TData Backup { get; private set; }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>The errors.</value>
        protected ErrorsContainer<string> Errors => _errors;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is in edit.
        /// </summary>
        /// <value><c>true</c> if this instance is in edit; otherwise, <c>false</c>.</value>
        private bool IsInEdit { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Begins an edit on an object.
        /// </summary>
        public void BeginEdit()
        {
            if(IsInEdit) return;

            // Make a backup
            IsInEdit = true;
            Backup = ObjectExtensions.Clone(Data);

            SetHasChanges();
        }

        /// <summary>
        /// Discards changes since the last <see
        /// cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> call.
        /// </summary>
        public void CancelEdit()
        {
            if(!IsInEdit) return;

            // Restore from backup
            IsInEdit = false;
            Data = Backup;

            ClearHasChanges();
            Errors.ClearErrors();

            RaisePropertyChanged(string.Empty);
        }

        /// <summary>
        /// Pushes changes since the last <see
        /// cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> or <see
        /// cref="M:System.ComponentModel.IBindingList.AddNew"/> call into the underlying object.
        /// </summary>
        public void EndEdit()
        {
            if(!IsInEdit) return;

            IsInEdit = false;
            Backup = null;

            _eventAggregator.GetEvent<FileDataChangedEvent>().Publish();

            ClearHasChanges();
        }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property to retrieve validation errors for; or <see langword="null"/> or
        /// <see cref="F:System.String.Empty"/>, to retrieve entity-level errors.
        /// </param>
        /// <returns>The validation errors for the property or entity.</returns>
        public IEnumerable GetErrors(string propertyName) => _errors.GetErrors(propertyName);

        /// <summary>
        /// Raises the errors changed.
        /// </summary>
        /// <param name="name">The name.</param>
        protected void RaiseErrorsChanged(string name)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(name));
        }

        /// <summary>
        /// Validates the property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        protected virtual void ValidateProperty(string name, object value)
        {
        }

        #endregion Methods
    }
}