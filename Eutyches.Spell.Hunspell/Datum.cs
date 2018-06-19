//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Newtonsoft.Json;
using System;

namespace Eutyches.Spell.Hunspell
{
    /// <summary>
    /// Class Datum.
    /// </summary>
    [Serializable]
    public class Datum
    {
        #region Fields

        private static char[] Separators = new char[] { ':' };

        #endregion Fields

        #region Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="Datum"/> class.
        /// </summary>
        public Datum() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Datum"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public Datum(string data)
        {
            Parse(data);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Datum"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        public Datum(DatumType type, string value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [JsonProperty("T")]
        public DatumType Type { get; protected set; } = DatumType.Unknown;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [JsonProperty("V")]
        public string Value { get; protected set; } = string.Empty;

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return $"{ToText()}";
        }

        /// <summary>
        /// To the text.
        /// </summary>
        /// <returns>System.String.</returns>
        public string ToText()
        {
            return $"{Type.ToId()}:{Value}";
        }

        /// <summary>
        /// Parses the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        protected void Parse(string s)
        {
            var fields = s.Split(Separators);
            Type = fields[0].FromId();
            Value = fields[1];
        }

        #endregion Properties
    }
}