//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Eutyches.Spell.Hunspell
{
    [Serializable]
    public class FileMetaInfo : AffixOptions, IDeepClone<FileMetaInfo>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the contributors.
        /// </summary>
        /// <value>The contributors.</value>
        [JsonProperty(Order = 5)]
        public List<ContributorInfo> Contributors { get; set; } = new List<ContributorInfo>();

        /// <summary>
        /// Gets or sets the creation.
        /// </summary>
        /// <value>The creation.</value>
        [JsonProperty(Order = 1)]
        public DateTime Creation { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the creator.
        /// </summary>
        /// <value>The creator.</value>
        [JsonProperty(Order = 4)]
        public ContributorInfo Creator { get; set; } = new ContributorInfo();

        /// <summary>
        /// Gets or sets the name of the english.
        /// </summary>
        /// <value>The name of the english.</value>
        [JsonProperty(Order = 3)]
        public string EnglishName { get; set; }

        /// <summary>
        /// Gets or sets the name of the language.
        /// </summary>
        /// <value>The name of the language.</value>
        [JsonProperty(Order = 2)]
        public string LanguageName { get; set; }

        /// <summary>
        /// Gets or sets the license.
        /// </summary>
        /// <value>The license.</value>
        [JsonProperty(Order = 100)]
        public string License { get; set; }

        /// <summary>
        /// Gets or sets the long description.
        /// </summary>
        /// <value>The long description.</value>
        [JsonProperty(Order = 7)]
        public string LongDescription { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty(Order = 0)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the read me.
        /// </summary>
        /// <value>The read me.</value>
        [JsonProperty(Order = 8)]
        public string ReadMe { get; set; }

        /// <summary>
        /// Gets or sets the short description.
        /// </summary>
        /// <value>The short description.</value>
        [JsonProperty(Order = 6)]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>FileMetaInfo.</returns>
        public FileMetaInfo Clone()
        {
            return ObjectExtensions.Clone<FileMetaInfo>(this);
        }

        #endregion Properties
    }
}