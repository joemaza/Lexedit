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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Eutyches.Spell.Lexedit.Services.Views.Windows
{
    /// <summary>
    /// Interaction logic for DerivativesWindow.xaml
    /// </summary>
    public partial class StemRelationsToolWindow : ToolWindowBase
    {
        #region Constructors

        public StemRelationsToolWindow()
        {
            InitializeComponent();

            DataContext = CommonServiceLocator.ServiceLocator.Current.GetInstance<StemRelationsViewModel>();
        }

        #endregion Constructors
    }
}