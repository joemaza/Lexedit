//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonServiceLocator;
using Eutyches.Spell.Hunspell;
using Eutyches.Spell.Lexedit.Properties;
using Eutyches.Spell.Lexedit.Services.Interfaces;
using Eutyches.Spell.Lexedit.Services.Models;
using Prism.Commands;
using Prism.Events;

namespace Eutyches.Spell.Lexedit.ViewModels.Details
{
    /// <summary>
    /// A ViewModel that acts as a wrapper for <see cref="Eutyches.Spell.Hunspell.Stem"/> that
    /// implements <see cref="INotifyPropertyChanged"/> and contains its business logic.
    /// </summary>
    public class StemDetailsViewModel : MorphemeViewModelBase<Stem>
    {
        #region Fields

        protected List<string> _affixClipboard = new List<string>();

        public StemDetailsViewModel(IEventAggregator eventAggregator, IFileService fileService, IDialogService dialogService, IToolService toolService)
            : base(eventAggregator, fileService, dialogService, toolService)
        {
            Data = new Stem();
        }

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Gets the base.
        /// </summary>
        /// <value>The base.</value>
        public dynamic Base
        {
            get
            {
                if(Data.BaseId == null)
                {
                    return null;
                }

                var stems = _fileService.Lexicon.Stems?.FindAll(s => s.Id == Data.BaseId);

                if(stems.Count == 1)
                {
                    return stems[0];
                }
                else if(stems.Count == 0)
                {
                    return Resources.ErrorStemNotFound;
                }
                else
                {
                    return Resources.ErrorStemDuplicates;
                }
            }
        }

        /// <summary>
        /// Gets the base count.
        /// </summary>
        /// <value>The base count.</value>
        public int? BaseCount => _fileService.Lexicon.Stems.DefaultIfEmpty(null).Count(s => s.BaseId == Data.Id);

        /// <summary>
        /// Gets or sets the base identifier.
        /// </summary>
        /// <value>The base identifier.</value>
        public Guid? BaseId
        {
            get => Data.BaseId;

            set
            {
                if(Data.BaseId == value) return;

                BeginEdit();
                Data.BaseId = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can go to base.
        /// </summary>
        /// <value><c>true</c> if this instance can go to base; otherwise, <c>false</c>.</value>
        public bool CanGoToBase => !(BaseId is null);

        /// <summary>
        /// Gets a value indicating whether this instance can go to root.
        /// </summary>
        /// <value><c>true</c> if this instance can go to root; otherwise, <c>false</c>.</value>
        public bool CanGoToRoot => !(RootId is null);

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public Category Category
        {
            get => Data.Category;

            set
            {
                if(Data.Category == value) return;

                BeginEdit();
                Data.Category = value;

                RaisePropertyChanged(nameof(Category));
            }
        }

        /// <summary>
        /// Gets the edit base command.
        /// </summary>
        /// <value>The edit base command.</value>
        public ICommand EditBaseCommand => new DelegateCommand(OnEditBaseCommand);

        /// <summary>
        /// Gets the edit root command.
        /// </summary>
        /// <value>The edit root command.</value>
        public ICommand EditRootCommand => new DelegateCommand(OnEditRootCommand);

        /// <summary>
        /// Gets the go to base command.
        /// </summary>
        /// <value>The go to base command.</value>
        public ICommand GoToBaseCommand => new DelegateCommand(OnGoToBaseCommand, () => CanGoToBase).ObservesProperty(() => BaseId);

        /// <summary>
        /// Gets the go to root command.
        /// </summary>
        /// <value>The go to root command.</value>
        public ICommand GoToRootCommand => new DelegateCommand(OnGoToRootCommand, () => CanGoToRoot).ObservesProperty(() => RootId);

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id => Data.Id;

        /// <summary>
        /// Gets the root.
        /// </summary>
        /// <value>The root.</value>
        public dynamic Root
        {
            get
            {
                if(Data.RootId == null)
                {
                    return null;
                }

                var stems = _fileService.Lexicon.Stems.FindAll(s => s.Id == Data.RootId);

                if(stems.Count == 1)
                {
                    return stems[0];
                }
                else if(stems.Count <= 0)
                {
                    return Resources.ErrorStemNotFound;
                }
                else
                {
                    return Resources.ErrorStemDuplicates;
                }
            }
        }

        /// <summary>
        /// Gets the root count.
        /// </summary>
        /// <value>The root count.</value>
        public int? RootCount => _fileService.Lexicon.Stems.DefaultIfEmpty(null).Count(s => s.RootId == Data.Id);

        /// <summary>
        /// Gets or sets the root identifier.
        /// </summary>
        /// <value>The root identifier.</value>
        public Guid? RootId
        {
            get => Data.RootId;

            set
            {
                if(Data.RootId == value) return;

                BeginEdit();
                Data.RootId = value;
            }
        }

        /// <summary>
        /// Gets or sets the sense.
        /// </summary>
        /// <value>The sense.</value>
        public string Sense
        {
            get => Data.Sense;

            set
            {
                if(Data.Sense == value) return;

                BeginEdit();
                Data.Sense = value;

                RaisePropertyChanged(nameof(Sense));
            }
        }

        /// <summary>
        /// Validates the property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        protected override void ValidateProperty(string name, object value)
        {
            if(name == nameof(Form))
            {
                ValidateForm(value as string, out List<string> errors);
                _errors.SetErrors(nameof(Form), errors);
            }
            else
            {
                base.ValidateProperty(name, value);
            }
        }

        /// <summary>
        /// Called when [edit base command].
        /// </summary>
        private void OnEditBaseCommand()
        {
            _toolService.ShowStemRelationsToolWindow(
                true, Data, (id) =>
                {
                    BaseId = id;
                });

            RaisePropertyChanged(nameof(Base));
            RaisePropertyChanged(nameof(BaseCount));
        }

        /// <summary>
        /// Called when [edit root command].
        /// </summary>
        private void OnEditRootCommand()
        {
            // Show the dialog
            _toolService.ShowStemRelationsToolWindow(
                false, Data, (id) =>
                {
                    RootId = id;
                });

            RaisePropertyChanged(nameof(Root));
            RaisePropertyChanged(nameof(RootCount));
        }

        /// <summary>
        /// Called when [go to base command].
        /// </summary>
        private void OnGoToBaseCommand()
        {
            _eventAggregator.GetEvent<GoToStemEvent>().Publish(BaseId.Value);
        }

        /// <summary>
        /// Called when [go to root command].
        /// </summary>
        private void OnGoToRootCommand()
        {
            _eventAggregator.GetEvent<GoToStemEvent>().Publish(RootId.Value);
        }

        /// <summary>
        /// Validates the form.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="errors">The errors.</param>
        private void ValidateForm(string form, out List<string> errors)
        {
            errors = new List<string>();

            if(string.IsNullOrWhiteSpace(form))
            {
                errors.Add(Resources.ErrorStemFormEmpty);
            }

            if(form.Contains(" "))
            {
                errors.Add(Resources.ErrorStemFormContainsWhiteSpace);
            }
        }

        #endregion Constructors
    }
}