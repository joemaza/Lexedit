//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using Eutyches.Spell.Hunspell;
using Eutyches.Spell.Lexedit.Services.Interfaces;
using Prism.Events;
using System;

namespace Eutyches.Spell.Lexedit.ViewModels.Options
{
    public class ConversionOptionsViewModel : OptionViewModelBase<ConversionOptions>
    {
        #region Constructors

        protected ConversionOptionsViewModel(IEventAggregator eventAggregator, IFileService fileService, IDialogService dialogService, IToolService toolService)
            : base(eventAggregator, fileService, dialogService, toolService)
        {
        }

        #endregion Constructors

        #region Methods

        protected override void OnFileDataChanged()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}