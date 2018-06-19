//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System;
using System.Collections.Generic;

namespace Eutyches.Spell.Utilities
{
    /// <summary>
    /// Class ImportResult.
    /// </summary>
    public class ImportResult
    {
        #region Fields

        private List<string> _messages = new List<string>();
        private ImportStatus _status = ImportStatus.Unknown;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message => string.Join(Environment.NewLine, _messages);

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public ImportStatus Status
        {
            get => _status;

            set
            {
                // If the status is "less" than what is already set, return. I.e., if the status
                // worsens, we would like to record that instead.
                if(_status > value)
                {
                    return;
                }

                _status = value;
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Adds the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void AddMessage(string message)
        {
            if(string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            // Don't add duplicates
            if(_messages.Contains(message))
            {
                return;
            }

            _messages.Add(message);
        }

        #endregion Properties
    }
}