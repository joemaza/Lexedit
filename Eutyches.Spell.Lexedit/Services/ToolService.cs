//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using Eutyches.Spell.Hunspell;
using Eutyches.Spell.Lexedit.Services.Interfaces;
using Eutyches.Spell.Lexedit.Services.ViewModels;
using Eutyches.Spell.Lexedit.Services.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Eutyches.Spell.Lexedit.Services
{
    public class ToolService : IToolService
    {
        #region Fields

        private Window _owner;

        #endregion Fields

        #region Properties

        public Window Owner
        {
            get => _owner ?? (_owner = Application.Current.MainWindow);

            protected set => _owner = value;
        }

        #endregion Properties

        #region Methods

        public void ShowImportAffixesToolWindow(Action<IEnumerable<Affix>> callback)
        {
            throw new NotImplementedException();
        }

        public void ShowImportStemsToolWindow(Action<IEnumerable<Stem>> callback)
        {
            throw new NotImplementedException();
        }

        public void ShowStemRelationsToolWindow(bool isBase, Stem stem, Action<Guid?> callback)
        {
            var window = new StemRelationsToolWindow { Owner = Owner };
            var vm = window.DataContext as StemRelationsViewModel;

            vm.Initialize(isBase, stem);

            if(window.ShowDialog() == true)
            {
                if(vm.Parent is null)
                {
                    callback(null);
                }
                else
                {
                    callback(vm.Parent.Id);
                }
            }
        }

        public void ShowTestToolWindow(Stem stem)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}