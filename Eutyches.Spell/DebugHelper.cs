//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Eutyches.Spell
{
    public static class DebugHelper
    {
        #region Fields

        public static void Error(string message, [CallerMemberName]string caller = "") => Debug.WriteLine($"{message}", $"{caller}.ERROR");

        public static void Info(string message, [CallerMemberName]string caller = "") => Debug.WriteLine($"{message}", $"{caller}.INFO");

        public static void NotImplemented([CallerMemberName]string caller = "") => Debug.WriteLine($"Method not implemented.", $"{caller}.WARNING");

        public static void Warning(string message, [CallerMemberName]string caller = "") => Debug.WriteLine($"{message}", $"{caller}.WARNING");

        #endregion Fields
    }
}