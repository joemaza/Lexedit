//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommonServiceLocator;
using Eutyches.Spell.Hunspell;
using Eutyches.Spell.Lexedit.Properties;
using Eutyches.Spell.Lexedit.Services.Interfaces;
using Eutyches.Spell.Lexedit.Services.Models;
using Eutyches.Spell.Lexedit.ViewModels.Details;
using Eutyches.Spell.Lexedit.Views.Details;
using Prism.Events;

namespace Eutyches.Spell.Lexedit.ViewModels
{
    public class StemMasterViewModel : MasterViewModelBase<StemDetailsViewModel>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StemMasterViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="fileService">The file service.</param>
        /// <param name="dialogService">The dialog service.</param>
        public StemMasterViewModel(IEventAggregator eventAggregator, IFileService fileService, IDialogService dialogService, IToolService toolService)
            : base(eventAggregator, fileService, dialogService, toolService)
        {
            _eventAggregator.GetEvent<GoToStemEvent>().Subscribe((id) => GoToItem(id));
        }

        /// <summary>
        /// Gets the name of the sort property.
        /// </summary>
        /// <value>The name of the sort property.</value>
        protected override string SortPropertyName => "Form";

        /// <summary>
        /// Filters the items.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if the item(s) match the filter text, <c>false</c> otherwise.</returns>
        protected override bool FilterItems(object obj)
        {
            if(obj is StemDetailsViewModel data)
            {
                if(string.IsNullOrEmpty(_itemFilter))
                {
                    return true;
                }

                return Regex.IsMatch(data.Form, _itemFilter.ToRegex(), RegexOptions.Singleline | RegexOptions.IgnoreCase);
            }

            return false;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        protected override void GetItems()
        {
            _fileService.Lexicon.Stems
               .DefaultIfEmpty(null)
               .Select(s =>
               {
                   var vm = ServiceLocator.Current.GetInstance<StemDetailsViewModel>();
                   vm.Data = s;

                   return vm;
               })
               .ToList()
               .ForEach(s => Items.Add(s));
        }

        /// <summary>
        /// Called when the Remove button is clicked.
        /// </summary>
        protected override void OnRemoveItemCommand()
        {
            var content = Resources.RemoveStemDialogExtraNoRelations;

            int countAsRoot = _fileService.Lexicon.Stems.Count(s => s.RootId == SelectedItem.Id);
            int countAsBase = _fileService.Lexicon.Stems.Count(s => s.BaseId == SelectedItem.Id);

            if(countAsRoot > 0 | countAsBase > 0)
            {
                var sb = new StringBuilder();

                sb.AppendLine(Resources.RemoveStemDialogExtraHasRelations);
                sb.AppendFormat(Resources.RemoveStemDialogRelationCountFormat, countAsRoot, countAsBase);

                content = sb.ToString();
            }

            _dialogService.ShowConfirmationDialog(
                new OperationConfirmation
                {
                    Title = Resources.RemoveStemDialogTitle,
                    Operation = string.Format(Resources.RemoveStemDialogQuestionFormat2,
                        SelectedItem.Form, SelectedItem.Category),
                    Content = content,

                    Affirmative = Resources.RemoveStemDialogAffirmative,
                    Negative = Resources.RemoveStemDialogNegative,

                    Caution = Resources.RemoveStemDialogCaution,

                    IsWarning = true
                },
                (result) =>
                {
                    if(result == true)
                    {
                        // Reset the RootId's and/or the BaseId's of any related stems
                        ResetRootAndBaseIds();

                        // Remove the item
                        RemoveCurrentItem();
                    }
                });
        }

        /// <summary>
        /// Called when the "Save" command is clicked.
        /// </summary>
        protected override void OnSaveItemCommand()
        {
            if(ItemsView.IsAddingNew)
            {
                var item = ItemsView.CurrentAddItem as StemDetailsViewModel;

                _fileService.Lexicon.Stems.Add(item.Data);
            }

            base.OnSaveItemCommand();
        }

        /// <summary>
        /// Removes the current item from the ItemsView
        /// </summary>
        protected override void RemoveCurrentItem()
        {
            // Remove it from the lexicon
            var stem = SelectedItem.Data;

            if(_fileService.Lexicon.Stems.Remove(stem))
            {
                base.RemoveCurrentItem();
            }
        }

        /// <summary>
        /// Resets the root and base ids.
        /// </summary>
        protected void ResetRootAndBaseIds()
        {
            Items
                .Where(s => s.RootId == SelectedItem.Id | s.BaseId == SelectedItem.Id)
                .ToList()
                .ForEach(
                s =>
                {
                    s.RootId = null;
                    s.EndEdit();
                });
        }

        /// <summary>
        /// Goes to item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        private void GoToItem(Guid id)
        {
            try
            {
                var item = Items.Single(s => s.Id == id);

                ItemsView.MoveCurrentTo(item);
            }
            catch(Exception ex)
            {
                _dialogService.ShowExceptionDialog(
                    new ExceptionNotification
                    {
                        Operation = "Go To Stem",
                        Exception = ex
                    });
            }
        }

        #endregion Constructors
    }
}