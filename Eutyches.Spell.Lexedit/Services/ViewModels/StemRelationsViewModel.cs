//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Input;
using Eutyches.Spell.Hunspell;
using Eutyches.Spell.Lexedit.Properties;
using Eutyches.Spell.Lexedit.Services.Interfaces;
using Prism.Commands;
using Prism.Events;

namespace Eutyches.Spell.Lexedit.Services.ViewModels
{
    public class StemRelationsViewModel : ToolViewModelBase
    {
        #region Fields

        protected static readonly SortDescription DefaultSortDescription = new SortDescription(SortPropertyName, ListSortDirection.Ascending);
        protected static readonly string SortPropertyName = "Form";
        private string _availableFilter;
        private string _derivedFilter;
        private Stem _parent;
        private Stem _selectedAvailable;
        private Stem _selectedDerived;

        #endregion Fields

        #region Constructors

        public StemRelationsViewModel(IEventAggregator eventAggregator, IFileService fileService, IDialogService dialogService, IToolService toolService)
                            : base(eventAggregator, fileService, dialogService, toolService)
        {
            AvailableView = new ListCollectionView(AvailableStems);
            AvailableView.SortDescriptions.Add(DefaultSortDescription);
            AvailableView.Filter += FilterAvailable;
            AvailableView.CurrentChanged += OnAvailableViewCurrentChanged;

            DerivedView = new ListCollectionView(DerivedStems);
            DerivedView.SortDescriptions.Add(DefaultSortDescription);
            DerivedView.Filter += FilterDerived;
            DerivedView.CurrentChanged += OnDerivedViewCurrentChanged;
        }

        #endregion Constructors

        #region Properties

        public ListCollectionView AvailableView { get; protected set; }

        public string AvailableViewFilter
        {
            get => _availableFilter;

            set
            {
                if(SetProperty(ref _availableFilter, value, nameof(AvailableViewFilter)))
                {
                    AvailableView.Refresh();
                }
            }
        }

        public ListCollectionView DerivedView { get; protected set; }

        public string DerivedViewFilter
        {
            get => _derivedFilter;

            set
            {
                if(SetProperty(ref _derivedFilter, value, nameof(DerivedViewFilter)))
                {
                    DerivedView.Refresh();
                }
            }
        }

        public bool IsBase { get; protected set; }

        public string Mode => IsBase ? Resources.StemRelationsToolWindowBaseMode
            : Resources.StemRelationsToolWindowRootMode;

        public ICommand MoveAvailableToDerivedCommand => new DelegateCommand(OnMoveAvailableToDerivedCommand, () => CanMoveAvailableToDerived).ObservesProperty(() => SelectedAvailable);

        public ICommand MoveAvailableToParentCommand => new DelegateCommand(OnMoveAvailableToParentCommand, () => CanMoveAvailableToParent).ObservesProperty(() => SelectedAvailable);

        public ICommand MoveDerivedToAvailableCommand => new DelegateCommand(OnMoveDerivedToAvailableCommand, () => CanMoveDerivedToAvailable).ObservesProperty(() => SelectedDerived);

        public ICommand MoveParentToAvailableCommand => new DelegateCommand(OnMoveParentToAvailableCommand, () => CanMoveParentToAvailable).ObservesProperty(() => Parent);

        public Stem Parent
        {
            get => _parent;

            set
            {
                if(SetProperty(ref _parent, value, nameof(Parent)))
                {
                    CanMoveParentToAvailable = !(_parent is null);
                }
            }
        }

        public Stem SelectedAvailable
        {
            get => _selectedAvailable;
            set => SetProperty(ref _selectedAvailable, value, nameof(SelectedAvailable));
        }

        public Stem SelectedDerived
        {
            get => _selectedDerived;
            set => SetProperty(ref _selectedDerived, value, nameof(SelectedDerived));
        }

        public Stem Stem { get; protected set; }

        public override string Title => $"{Resources.StemRelationsToolWindowTitle}: {Mode}";

        protected ObservableCollection<Stem> AvailableStems { get; private set; } = new ObservableCollection<Stem>();

        protected bool CanMoveAvailableToDerived { get; private set; } = false;

        protected bool CanMoveAvailableToParent { get; private set; } = false;

        protected bool CanMoveDerivedToAvailable { get; private set; } = false;

        protected bool CanMoveParentToAvailable { get; private set; } = false;

        protected ObservableCollection<Stem> DerivedStems { get; private set; } = new ObservableCollection<Stem>();

        protected override void OnAcceptClicked()
        {
            int changed = 0;

            if(IsBase)
            {
                DerivedStems.ToList()
                    .ForEach(s =>
                    {
                        s.BaseId = Stem.Id;
                        changed++;
                    });

                AvailableStems
                    .Where(s => s.BaseId == Stem.Id)
                    .ToList()
                    .ForEach(s =>
                    {
                        s.BaseId = null;
                        changed++;
                    });
            }
            else
            {
                DerivedStems
                    .ToList()
                    .ForEach(s =>
                    {
                        s.RootId = Stem.Id;
                        changed++;
                    });

                AvailableStems
                    .Where(s => s.RootId == Stem.Id)
                    .ToList()
                    .ForEach(s =>
                    {
                        s.RootId = null;
                        changed++;
                    });
            }

            if(changed > 0)
            {
                SetHasChanges();
            }

            base.OnAcceptClicked();
        }

        protected void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    SetHasChanges();
                    break;

                case NotifyCollectionChangedAction.Remove:
                    SetHasChanges();
                    break;
            }
        }

        private void OnMoveAvailableToParentCommand()
        {
            // If Parent has a stem, put it back into the available collection.
            if(!(Parent is null))
            {
                AvailableStems.Add(Parent);
            }

            Parent = SelectedAvailable;

            AvailableStems.Remove(SelectedAvailable);
        }

        private void OnMoveParentToAvailableCommand()
        {
            AvailableStems.Add(Parent);

            Parent = null;
        }

        #endregion Properties

        #region Methods

        public void Initialize(bool isBase, Stem stem)
        {
            IsBase = isBase;
            Stem = stem;

            GetStems();
        }

        protected bool Filter(object obj, string filter)
        {
            if(obj is Stem data)
            {
                if(string.IsNullOrEmpty(filter))
                {
                    return true;
                }

                return Regex.IsMatch(data.Form, filter.ToRegex(), RegexOptions.Singleline | RegexOptions.IgnoreCase);
            }

            return false;
        }

        protected bool FilterAvailable(object obj)
        {
            return Filter(obj, _availableFilter);
        }

        protected bool FilterDerived(object obj)
        {
            return Filter(obj, _derivedFilter);
        }

        protected void GetStems()
        {
            if(Stem is null)
            {
                return;
            }

            var parentId = IsBase ? Stem.BaseId : Stem.RootId;

            if(parentId is null)
            {
                Parent = null;
            }
            else
            {
                Parent = _fileService.Lexicon.Stems.Single(s => s.Id == parentId);
            }

            var stems = _fileService.Lexicon.Stems.Where(s => s.Id != Stem.Id && s.Id != Parent?.Id);
            var derived = IsBase ? stems.Where(s => s.BaseId == Stem.Id) : stems.Where(s => s.RootId == Stem.Id);

            DerivedStems.Clear();
            DerivedStems.AddRange(derived);

            var available = stems.Except(derived);

            AvailableStems.Clear();
            AvailableStems.AddRange(available);

            // Wire the CollectionChanged event
            AvailableStems.CollectionChanged += OnCollectionChanged;
            DerivedStems.CollectionChanged += OnCollectionChanged;
        }

        private void OnAvailableViewCurrentChanged(object sender, EventArgs e)
        {
            SelectedAvailable = (Stem) (sender as ListCollectionView).CurrentItem;

            CanMoveAvailableToDerived = !(SelectedAvailable is null);
            CanMoveAvailableToParent = !(SelectedAvailable is null);
        }

        private void OnDerivedViewCurrentChanged(object sender, EventArgs e)
        {
            SelectedDerived = (Stem) (sender as ListCollectionView).CurrentItem;

            CanMoveDerivedToAvailable = !(SelectedDerived is null);
        }

        private void OnMoveAvailableToDerivedCommand()
        {
            // Add to Derived
            DerivedStems.Add(SelectedAvailable);

            // Remove from available
            AvailableStems.Remove(SelectedAvailable);
        }

        private void OnMoveDerivedToAvailableCommand()
        {
            // Add to available
            AvailableStems.Add(SelectedDerived);

            // Remove from derived
            DerivedStems.Remove(SelectedDerived);
        }

        #endregion Methods
    }
}