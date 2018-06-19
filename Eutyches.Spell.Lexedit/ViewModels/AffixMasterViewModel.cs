//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Eutyches.Spell.Lexedit.Services.Interfaces;
using Eutyches.Spell.Lexedit.ViewModels.Details;
using Prism.Events;

namespace Eutyches.Spell.Lexedit.ViewModels
{
    public class AffixMasterViewModel : MasterViewModelBase<AffixDetailsViewModel>
    {
        #region Constructors

        public AffixMasterViewModel(IEventAggregator eventAggregator, IFileService fileService, IDialogService dialogService, IToolService toolService)
            : base(eventAggregator, fileService, dialogService, toolService)
        {
        }

        protected override string SortPropertyName => "Flag";

        protected override bool FilterItems(object obj)
        {
            if(obj is AffixDetailsViewModel data)
            {
                if(string.IsNullOrEmpty(ItemFilter))
                {
                    return true;
                }

                return Regex.IsMatch(data.Label, ItemFilter.ToRegex(), RegexOptions.Singleline | RegexOptions.IgnoreCase);
            }

            return false;
        }

        protected override void GetItems()
        {
            _fileService.Lexicon.Affixes
                 .DefaultIfEmpty(null)
                 .Select(a => CommonServiceLocator.ServiceLocator.Current.GetInstance<AffixDetailsViewModel>())
                 .ToList()
                 .ForEach(a => Items.Add(a));
        }

        protected override void OnFileClosed()
        {
            throw new NotImplementedException();
        }

        protected override void OnFileCreated()
        {
            throw new NotImplementedException();
        }

        protected override void OnFileDataChanged()
        {
            throw new NotImplementedException();
        }

        protected override void OnFileOpened()
        {
            throw new NotImplementedException();
        }

        protected override void OnFileSaved()
        {
            throw new NotImplementedException();
        }

        #endregion Constructors
    }
}