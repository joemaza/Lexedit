//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using CommonServiceLocator;
using Eutyches.Spell.Hunspell;
using Eutyches.Spell.Lexedit.Services.Interfaces;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eutyches.Spell.Lexedit.ViewModels.Details
{
    /// <summary>
    /// Class RuleDetailsViewModel.
    /// </summary>
    /// <seealso cref="Eutyches.Spell.Lexedit.ViewModels.Details.DetailsViewModelBase{Eutyches.Spell.Hunspell.Rule}"/>
    public class RuleDetailsViewModel : MorphemeViewModelBase<Rule>
    {
        #region Constructors

        protected RuleDetailsViewModel(IEventAggregator eventAggregator, IFileService fileService, IDialogService dialogService, IToolService toolService)
            : base(eventAggregator, fileService, dialogService, toolService)
        {
        }

        #endregion Constructors



        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleDetailsViewModel"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="fileService">The file service.</param>
        /// <param name="dialogService">The dialog service.</param>
        //public RuleDetailsViewModel(Rule data)
        //    : base(
        //          data,
        //          ServiceLocator.Current.GetInstance<IEventAggregator>(),
        //          ServiceLocator.Current.GetInstance<IFileService>(),
        //          ServiceLocator.Current.GetInstance<IDialogService>(),
        //          ServiceLocator.Current.GetInstance<IToolService>()
        //          )
        //{
        //}

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Called when file data changed.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        protected override void OnFileDataChanged()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}