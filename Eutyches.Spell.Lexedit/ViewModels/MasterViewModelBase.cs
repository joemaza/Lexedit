//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using Eutyches.Spell.Lexedit.Services.Interfaces;
using Eutyches.Spell.Lexedit.Services.Models;
using Eutyches.Spell.Lexedit.Properties;
using Prism.Commands;
using Prism.Events;
using CommonServiceLocator;
using Eutyches.Spell.Lexedit.ViewModels.Details;
using Eutyches.Spell.Hunspell;

namespace Eutyches.Spell.Lexedit.ViewModels
{
    /// <summary>
    /// Base class for ViewModels that participate in a Master-Detail relationship as a master.
    /// </summary>
    /// <typeparam name="TDetailsViewModel">The Type of ViewModel in the collection</typeparam>
    /// <seealso cref="Eutyches.Spell.Lexedit.ViewModels.CommonViewModelBase"/>
    public abstract class MasterViewModelBase<TDetailsViewModel> : ViewModelBase where TDetailsViewModel
        : class, INotifyPropertyChanged, IHasChanges, IEditableObject, INotifyDataErrorInfo

    {
        #region Fields

        protected bool _addNew = false;
        protected bool _canAddItem = false;
        protected bool _canFilter = false;
        protected bool _canGoToFirst = false;
        protected bool _canGoToLast = false;
        protected bool _canGoToNext = false;
        protected bool _canGoToPrevious = false;
        protected bool _canRemoveItem = false;
        protected bool _canSaveItem = false;
        protected bool _canUndoItem = false;
        protected string _itemFilter;
        protected TDetailsViewModel _selectedItem;

        #endregion Fields

        #region Constructors

        public MasterViewModelBase(IEventAggregator eventAggregator, IFileService fileService, IDialogService dialogService, IToolService toolService)
            : base(eventAggregator, fileService, dialogService, toolService)
        {
            Items.CollectionChanged += OnItemsChanged;

            ItemsView = new ListCollectionView(Items);

            ItemsView.SortDescriptions.Add(new SortDescription(SortPropertyName, ListSortDirection.Ascending));

            ItemsView.CurrentChanging += OnCurrentChanging;
            ItemsView.CurrentChanged += OnCurrentChanged;

            ItemsView.Filter += FilterItems;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the add item command.
        /// </summary>
        /// <value>The add item command.</value>
        public ICommand AddItemCommand => new DelegateCommand(OnAddItemCommand, () => CanAddItem).ObservesCanExecute(() => IsActive);

        /// <summary>
        /// Gets or sets a value indicating whether this instance can add item.
        /// </summary>
        /// <value><c>true</c> if this instance can add item; otherwise, <c>false</c>.</value>
        public bool CanAddItem
        {
            get => _canAddItem;
            set => SetProperty(ref _canAddItem, value, nameof(CanAddItem));
        }

        /// <summary>
        /// Gets a value indicating whether this instance can filter.
        /// </summary>
        /// <value><c>true</c> if this instance can filter; otherwise, <c>false</c>.</value>
        public bool CanFilter => Items?.Count > 0;

        /// <summary>
        /// Gets or sets a value indicating whether this instance can go to first.
        /// </summary>
        /// <value><c>true</c> if this instance can go to first; otherwise, <c>false</c>.</value>
        public bool CanGoToFirst
        {
            get => _canGoToFirst;
            set => SetProperty(ref _canGoToFirst, value, nameof(CanGoToFirst));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can go to last.
        /// </summary>
        /// <value><c>true</c> if this instance can go to last; otherwise, <c>false</c>.</value>
        public bool CanGoToLast
        {
            get => _canGoToLast;
            set => SetProperty(ref _canGoToLast, value, nameof(CanGoToLast));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can go to next.
        /// </summary>
        /// <value><c>true</c> if this instance can go to next; otherwise, <c>false</c>.</value>
        public bool CanGoToNext
        {
            get => _canGoToNext;
            set => SetProperty(ref _canGoToNext, value, nameof(CanGoToNext));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can go to previous.
        /// </summary>
        /// <value><c>true</c> if this instance can go to previous; otherwise, <c>false</c>.</value>
        public bool CanGoToPrevious
        {
            get => _canGoToPrevious;
            set => SetProperty(ref _canGoToPrevious, value, nameof(CanGoToPrevious));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can remove item.
        /// </summary>
        /// <value><c>true</c> if this instance can remove item; otherwise, <c>false</c>.</value>
        public bool CanRemoveItem
        {
            get => _canRemoveItem;
            set => SetProperty(ref _canRemoveItem, value, nameof(CanRemoveItem));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can save item.
        /// </summary>
        /// <value><c>true</c> if this instance can save item; otherwise, <c>false</c>.</value>
        public bool CanSaveItem
        {
            get => _canSaveItem;
            set => SetProperty(ref _canSaveItem, value, nameof(CanSaveItem));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can undo item.
        /// </summary>
        /// <value><c>true</c> if this instance can undo item; otherwise, <c>false</c>.</value>
        public bool CanUndoItem
        {
            get => _canUndoItem;
            set => SetProperty(ref _canUndoItem, value, nameof(CanUndoItem));
        }

        /// <summary>
        /// Gets the go to first command.
        /// </summary>
        /// <value>The go to first command.</value>
        public ICommand GoToFirstCommand => new DelegateCommand(OnGoToFirstCommand, () => CanGoToFirst)
            .ObservesCanExecute(() => IsActive);

        /// <summary>
        /// Gets the go to last command.
        /// </summary>
        /// <value>The go to last command.</value>
        public ICommand GoToLastCommand => new DelegateCommand(OnGoToLastCommand, () => CanGoToLast)
            .ObservesCanExecute(() => IsActive);

        /// <summary>
        /// Gets the go to next command.
        /// </summary>
        /// <value>The go to next command.</value>
        public ICommand GoToNextCommand => new DelegateCommand(OnGoToNextCommand, () => CanGoToNext)
            .ObservesProperty(() => SelectedItem);

        /// <summary>
        /// Gets the go to previous command.
        /// </summary>
        /// <value>The go to previous command.</value>
        public ICommand GoToPreviousCommand => new DelegateCommand(OnGoToPreviousCommand, () => CanGoToPrevious)
            .ObservesProperty(() => SelectedItem);

        /// <summary>
        /// Gets or sets the item filter.
        /// </summary>
        /// <value>The item filter.</value>
        public string ItemFilter
        {
            get => _itemFilter;

            set
            {
                if(SetProperty(ref _itemFilter, value, nameof(ItemFilter)))
                {
                    ItemsView.Refresh();
                }
            }
        }

        /// <summary>
        /// Gets or sets the items view.
        /// </summary>
        /// <value>The items view.</value>
        public ListCollectionView ItemsView { get; protected set; }

        /// <summary>
        /// Gets the remove item command.
        /// </summary>
        /// <value>The remove item command.</value>
        public ICommand RemoveItemCommand => new DelegateCommand(OnRemoveItemCommand, () => CanRemoveItem).ObservesProperty(() => SelectedItem);

        /// <summary>
        /// Gets the save item command.
        /// </summary>
        /// <value>The save item command.</value>
        public ICommand SaveItemCommand => new DelegateCommand(OnSaveItemCommand, () => CanSaveItem).ObservesProperty(() => SelectedItem.HasChanges);

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public TDetailsViewModel SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value, nameof(SelectedItem));
        }

        /// <summary>
        /// Gets the undo item command.
        /// </summary>
        /// <value>The undo item command.</value>
        public ICommand UndoItemCommand => new DelegateCommand(OnUndoItemCommand, () => CanUndoItem).ObservesProperty(() => SelectedItem.HasChanges);

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        protected ObservableCollection<TDetailsViewModel> Items { get; private set; } = new ObservableCollection<TDetailsViewModel>();

        /// <summary>
        /// Gets the name of the sort property.
        /// </summary>
        /// <value>The name of the sort property.</value>
        protected abstract string SortPropertyName { get; }

        #endregion Properties

        #region Methods

        protected virtual void ClearItems()
        {
            // Perform the clear if the list has items
            if(Items.Count > 0)
            {
                Items.Clear();
            }
        }

        /// <summary>
        /// Enables the navigation buttons.
        /// </summary>
        protected void EnableNavigationButtons()
        {
            // Update Navigation buttons

            CanGoToFirst = (ItemsView.Count > 0) & (ItemsView.CurrentPosition != 0);
            CanGoToPrevious = (SelectedItem != null) & (ItemsView.Count > 0) & (ItemsView.CurrentPosition != 0);

            CanGoToNext = (SelectedItem != null) & (ItemsView.Count > 0) & (ItemsView.CurrentPosition != (ItemsView.Count - 1));
            CanGoToLast = (ItemsView.Count > 0) & (ItemsView.CurrentPosition != (ItemsView.Count - 1));
        }

        /// <summary>
        /// Filters the items.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if the item(s) match the filter text, <c>false</c> otherwise.</returns>
        protected abstract bool FilterItems(object obj);

        /// <summary>
        /// Gets the items.
        /// </summary>
        protected abstract void GetItems();

        /// <summary>
        /// Called when the AddItemCommand is invoked.
        /// </summary>
        protected void OnAddItemCommand()
        {
            // Create a new item and set it as the selected item
            var item = ServiceLocator.Current.GetInstance<TDetailsViewModel>();
            SelectedItem = (TDetailsViewModel) ItemsView.AddNewItem(item);
        }

        /// <summary>
        /// Handles the <see cref="E:CurrentChanged"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void OnCurrentChanged(object sender, EventArgs e)
        {
            SelectedItem = ItemsView.CurrentItem as TDetailsViewModel;

            CanRemoveItem = SelectedItem != null;
            CanUndoItem = SelectedItem != null ? SelectedItem.HasChanges : false;
            CanSaveItem = SelectedItem != null ? SelectedItem.HasChanges : false;

            EnableNavigationButtons();
        }

        /// <summary>
        /// Called when the file is closed.
        /// </summary>
        protected override void OnFileClosed()
        {
            ClearItems();

            base.OnFileClosed();
        }

        /// <summary>
        /// Called when the file is created.
        /// </summary>
        protected override void OnFileCreated()
        {
            ClearItems();

            base.OnFileCreated();
        }

        /// <summary>
        /// Called when the file is opened.
        /// </summary>
        protected override void OnFileOpened()
        {
            ClearItems();
            GetItems();

            base.OnFileOpened();
        }

        /// <summary>
        /// Called when [go to first command].
        /// </summary>
        protected void OnGoToFirstCommand()
        {
            ItemsView.MoveCurrentToFirst();
        }

        /// <summary>
        /// Called when [go to last command].
        /// </summary>
        protected void OnGoToLastCommand()
        {
            ItemsView.MoveCurrentToLast();
        }

        /// <summary>
        /// Called when [go to next command].
        /// </summary>
        protected void OnGoToNextCommand()
        {
            ItemsView.MoveCurrentToNext();
        }

        /// <summary>
        /// Called when [go to previous command].
        /// </summary>
        protected void OnGoToPreviousCommand()
        {
            ItemsView.MoveCurrentToPrevious();
        }

        /// <summary>
        /// Called when Remove button is clicked.
        /// </summary>
        protected virtual void OnRemoveItemCommand()
        {
            ItemsView.Remove(SelectedItem);
        }

        /// <summary>
        /// Called when Save button is clicked.
        /// </summary>
        protected virtual void OnSaveItemCommand()
        {
            if(ItemsView.IsAddingNew)
            {
                ItemsView.CommitNew();
                _addNew = false;
            }
            else
            {
                SelectedItem.EndEdit();
            }

            CanSaveItem = false;
            CanUndoItem = false;
        }

        /// <summary>
        /// Called when the Undo button is clicked.
        /// </summary>
        protected virtual void OnUndoItemCommand()
        {
            if(ItemsView.IsAddingNew)
            {
                ItemsView.CancelNew();
                SelectedItem = null;
                _addNew = false;
            }
            else
            {
                SelectedItem.CancelEdit();
            }

            CanSaveItem = false;
            CanUndoItem = false;
        }

        /// <summary>
        /// Removes the current item from the ItemsView
        /// </summary>
        protected virtual void RemoveCurrentItem()
        {
            ItemsView.Remove(SelectedItem);
        }

        /// <summary>
        /// Handles the <see cref="E:CurrentChanging"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="CurrentChangingEventArgs"/> instance containing the event data.
        /// </param>
        private void OnCurrentChanging(object sender, CurrentChangingEventArgs e)
        {
            var item = (sender as ListCollectionView).CurrentItem as TDetailsViewModel;

            // If the current item has changes, prompt the user to save or discard the changes.
            if(item?.HasChanges == true)
            {
                _dialogService.ShowConfirmationDialog(
                    new OperationConfirmation
                    {
                        Title = Resources.MoveCurrentItemDialogTitle,
                        Content = Resources.MoveCurrentItemDialogOperation,
                        Operation = Resources.MoveCurrentItemDialogQuestion,

                        Affirmative = Resources.MoveCurrentItemDialogAffirmative,
                        Negative = Resources.MoveCurrentItemDialogNegative
                    },
                    (result) =>
                    {
                        if(result == true)
                        {
                            item.EndEdit();
                        }
                        else if(result == false)
                        {
                            item.CancelEdit();
                        }
                    });
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ItemDetailChanged"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="PropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        private void OnItemDetailChanged(object sender, PropertyChangedEventArgs e)
        {
            CanSaveItem = !(sender as TDetailsViewModel).HasErrors;
            CanUndoItem = true;
        }

        /// <summary>
        /// Handles the <see cref="E:ItemErrorsChanged"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="DataErrorsChangedEventArgs"/> instance containing the event data.
        /// </param>
        private void OnItemErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            var item = sender as TDetailsViewModel;

            CanSaveItem = !item.HasErrors;
        }

        /// <summary>
        /// Handles the <see cref="E:ItemsChanged"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.
        /// </param>
        private void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach(TDetailsViewModel item in e.NewItems)
                    {
                        item.PropertyChanged += OnItemDetailChanged;
                        item.ErrorsChanged += OnItemErrorsChanged;
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach(TDetailsViewModel item in e.OldItems)
                    {
                        item.PropertyChanged -= OnItemDetailChanged;
                        item.ErrorsChanged -= OnItemErrorsChanged;
                    }
                    break;
            }
        }

        #endregion Methods
    }
}