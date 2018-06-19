//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using System.Collections.Generic;

namespace Eutyches.Spell.Hunspell.Utilities
{
    public class FlagGenerator
    {
        #region Fields

        private static readonly Queue<string> MainFlags = new Queue<string>();
        private static readonly Queue<string> SubFlags = new Queue<string>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="FlagGenerator"/> class.
        /// </summary>
        static FlagGenerator()
        {
            InitializeMainFlags();
            InitializeSubFlags();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the main flag.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetMain()
        {
            return MainFlags.Dequeue();
        }

        /// <summary>
        /// Gets the sub flag.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetOuter()
        {
            return SubFlags.Dequeue();
        }

        /// <summary>
        /// Initializes the main flags.
        /// </summary>
        private static void InitializeMainFlags()
        {
            string prefix = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string suffix = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            foreach(var p in prefix)
            {
                foreach(var s in suffix)
                {
                    MainFlags.Enqueue($"{p}{s}");
                }
            }
        }

        /// <summary>
        /// Initializes the sub flags.
        /// </summary>
        private static void InitializeSubFlags()
        {
            string prefix = "!\"$%&'()*+,-.<>[]^`_{|}";
            string suffix = "0123456789abcdefghijklmnopqrstuvwxyz{|}_`";

            foreach(var p in prefix)
            {
                foreach(var s in suffix)
                {
                    SubFlags.Enqueue($"{p}{s}");
                }
            }
        }

        #endregion Methods
    }
}