//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eutyches.Spell.Lexedit.Services.Interfaces;
using Eutyches.Spell.Lexedit.ViewModels.Details;
using Prism.Events;

namespace Eutyches.Spell.Lexedit.ViewModels.Options
{
    public abstract class OptionViewModelBase<TOption> : EditableViewModelBase<TOption> where TOption : class, new()
    {
        #region Constructors

        protected OptionViewModelBase(IEventAggregator eventAggregator, IFileService fileService, IDialogService dialogService, IToolService toolService)
            : base(eventAggregator, fileService, dialogService, toolService)
        {
        }

        #endregion Constructors
    }
}