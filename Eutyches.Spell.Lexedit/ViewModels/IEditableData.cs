//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
namespace Eutyches.Spell.Lexedit.ViewModels
{
    public interface IEditableData<TData> where TData : class

    {
        #region Properties

        TData Data { get; set; }

        #endregion Properties
    }
}