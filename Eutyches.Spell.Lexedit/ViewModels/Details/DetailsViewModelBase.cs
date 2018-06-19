//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using CommonServiceLocator;
using Eutyches.Spell.Lexedit.Services.Interfaces;
using Prism.Events;

namespace Eutyches.Spell.Lexedit.ViewModels.Details
{
    public abstract class DetailsViewModelBase<TItem>
        : EditableViewModelBase<TItem> where TItem : class, new()
    {
        #region Constructors

        protected DetailsViewModelBase(IEventAggregator eventAggregator, IFileService fileService, IDialogService dialogService, IToolService toolService)
            : base(eventAggregator, fileService, dialogService, toolService)
        {
        }

        #endregion Constructors



        #region Constructors

        ///// <summary>
        ///// Initializes a new instance of the <see cref="DetailsViewModelBase{TItem}"/> class.
        ///// </summary>
        //protected DetailsViewModelBase()
        //    : base(ServiceLocator.Current.GetInstance<IEventAggregator>(), ServiceLocator.Current.GetInstance<IFileService>(),
        //         ServiceLocator.Current.GetInstance<IDialogService>(), ServiceLocator.Current.GetInstance<IToolService>())
        //{
        //}

        ///// <summary>
        ///// Initializes a new instance of the <see cref="DetailsViewModelBase{TItem}"/> class.
        ///// </summary>
        ///// <param name="data">The data.</param>
        ///// <param name="eventAggregator">The event aggregator.</param>
        ///// <param name="fileService">The file service.</param>
        ///// <param name="dialogService">The dialog service.</param>
        //protected DetailsViewModelBase(TItem data, IEventAggregator eventAggregator, IFileService fileService, IDialogService dialogService, IToolService toolService)
        //    : base(eventAggregator, fileService, dialogService, toolService)
        //{
        //    Data = data;
        //}

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance can save item.
        /// </summary>
        /// <value><c>true</c> if this instance can save item; otherwise, <c>false</c>.</value>
        protected bool CanSaveItem => HasChanges & !(HasErrors);

        /// <summary>
        /// Gets a value indicating whether this instance can undo item.
        /// </summary>
        /// <value><c>true</c> if this instance can undo item; otherwise, <c>false</c>.</value>
        protected bool CanUndoItem => HasChanges;

        #endregion Properties
    }
}