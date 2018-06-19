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

namespace Eutyches.Spell.Lexedit.Services.Interfaces
{
    public interface IToolService
    {
        #region Properties

        Window Owner { get; }

        #endregion Properties

        #region Methods

        void ShowImportAffixesToolWindow(Action<IEnumerable<Affix>> callback);

        void ShowImportStemsToolWindow(Action<IEnumerable<Stem>> callback);

        void ShowStemRelationsToolWindow(bool isBase, Stem stem, Action<Guid?> callback);

        void ShowTestToolWindow(Stem stem);

        #endregion Methods
    }
}