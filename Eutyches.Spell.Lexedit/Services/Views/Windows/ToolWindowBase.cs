//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using Eutyches.Spell.Lexedit.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Eutyches.Spell.Lexedit.Services.Views.Windows
{
    public class ToolWindowBase : Window
    {
        #region Constructors

        public ToolWindowBase()
        {
            ShowInTaskbar = false;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            WindowStyle = WindowStyle.ToolWindow;
        }

        #endregion Constructors

        #region Methods

        protected void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as ToolViewModelBase;

            vm.CancelClicked += (s, ev) => { DialogResult = false; };
            vm.AcceptClicked += (s, ev) => { DialogResult = true; };
        }

        #endregion Methods
    }
}