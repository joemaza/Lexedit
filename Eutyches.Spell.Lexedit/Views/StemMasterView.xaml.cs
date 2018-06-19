//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Eutyches.Spell.Lexedit.Views
{
    /// <summary>
    /// Interaction logic for StemCollectionView.xaml
    /// </summary>
    public partial class StemMasterView : UserControl
    {
        #region Constructors

        public StemMasterView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dg = sender as DataGrid;

            if(dg?.SelectedItem is null) return;

            dg.ScrollIntoView(dg.SelectedItem);
        }

        #endregion Methods
    }
}