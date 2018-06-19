//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eutyches.Spell.Hunspell;
using Eutyches.Spell.Lexedit.Services.Interfaces;
using Prism.Events;

namespace Eutyches.Spell.Lexedit.ViewModels.Details
{
    /// <summary>
    /// A ViewModel that acts as a wrapper for <see cref="Eutyches.Spell.Hunspell.Affix"/> that
    /// implements <see cref="INotifyPropertyChanged"/> and contains its business logic.
    /// </summary>
    public class AffixDetailsViewModel : DetailsViewModelBase<Affix>
    {
        #region Constructors

        public AffixDetailsViewModel(IEventAggregator eventAggregator, IFileService fileService, IDialogService dialogService, IToolService toolService)
            : base(eventAggregator, fileService, dialogService, toolService)
        {
            Data = new Affix();
        }

        #endregion Constructors

        #region Constructors

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        public string Label
        {
            get => Data.Label;

            set
            {
                if(Data.Label == value) return;

                ValidateProperty(nameof(Label), value);

                Data.Label = value;

                RaisePropertyChanged(nameof(Label));
            }
        }

        protected override void OnFileDataChanged()
        {
            throw new NotImplementedException();
        }

        #endregion Constructors
    }
}