//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using Eutyches.Spell.Hunspell;
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

namespace Eutyches.Spell.Lexedit.Services.Views.Controls
{
    /// <summary>
    /// Interaction logic for StemRelationsView.xaml
    /// </summary>
    public partial class StemRelationsView : UserControl
    {
        #region Constructors

        public StemRelationsView()
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

        private void Available_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ScrollIntoView(sender);
        }

        private void Derived_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ScrollIntoView(sender);
        }

        private void ScrollIntoView(object sender)
        {
            var grid = sender as DataGrid;
            var selected = grid.SelectedItem;

            if(selected is null)
            {
                return;
            }

            grid.ScrollIntoView(selected);
        }
    }
}