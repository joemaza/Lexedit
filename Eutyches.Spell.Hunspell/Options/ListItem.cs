//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Newtonsoft.Json;
using System;

namespace Eutyches.Spell.Hunspell
{
    [Serializable]
    public abstract class ListItem : IListItem
    {
        #region Fields

        public const int ValueWidth = -24;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        [JsonProperty("C")]
        public string Comments { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// To the text.
        /// </summary>
        /// <param name="addComments">if set to <c>true</c> adds comments.</param>
        /// <returns>A formatted string representation of the this instance.</returns>
        public abstract string ToText(bool addComments);

        /// <summary>
        /// Adds the comment.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A string with comments added.</returns>
        protected string AddComment(string value)
        {
            return string.IsNullOrWhiteSpace(Comments) ? value : $"{value,ValueWidth} # {Comments}";
        }

        #endregion Methods
    }
}