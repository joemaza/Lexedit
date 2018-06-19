//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using Eutyches.Spell.Lexedit.Services.Interfaces;
using Prism.Events;

namespace Eutyches.Spell.Lexedit.Services.ViewModels
{
    public class ImportStemsViewModel : ImportViewModelBase
    {
        #region Constructors

        public ImportStemsViewModel(IEventAggregator eventAggregator, IFileService fileService, IDialogService dialogService, IToolService toolService)
            : base(eventAggregator, fileService, dialogService, toolService)
        {
        }

        #endregion Constructors
    }
}