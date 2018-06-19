//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Eutyches.Spell.Hunspell;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eutyches.Spell.Utilities
{
    public class JsonLexiconFile : ILexiconFile
    {
        #region Fields

        private static readonly JsonSerializer _serializer = new JsonSerializer()
        {
            //Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            //Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        #endregion Fields

        #region Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLexiconFile"/> class.
        /// </summary>
        /// <param name="lexicon">The lexicon.</param>
        public JsonLexiconFile(Lexicon lexicon)
        {
            if(lexicon is null)
            {
                Lexicon = new Lexicon();
            }
            else
            {
                Lexicon = lexicon;
            }
        }

        /// <summary>
        /// Gets or sets the lexicon.
        /// </summary>
        /// <value>The lexicon.</value>
        public Lexicon Lexicon
        {
            get; protected set;
        }

        /// <summary>
        /// Reads from the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>Lexicon.</returns>
        public Lexicon Read(string filePath)
        {
            var contents = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<Lexicon>(contents);
        }

        /// <summary>
        /// read as an asynchronous operation.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>Task&lt;Lexicon&gt;.</returns>
        public async Task<Lexicon> ReadAsync(string filePath)
        {
            Lexicon lexicon = null;

            using(var file = File.Open(filePath, FileMode.Open))
            {
                using(var reader = new StreamReader(file))
                {
                    string contents = await reader.ReadToEndAsync();

                    lexicon = JsonConvert.DeserializeObject<Lexicon>(contents);
                }
            }

            return lexicon;
        }

        /// <summary>
        /// Writes to the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="lexicon">The lexicon.</param>
        public void Write(string filePath)
        {
            var contents = JsonConvert.SerializeObject(Lexicon, _settings);

            File.WriteAllText(filePath, contents);
        }

        /// <summary>
        /// Write to the specified file path as an asynchronous operation.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="lexicon">The lexicon.</param>
        /// <returns>Task.</returns>
        public async Task WriteAsync(string filePath)
        {
            using(var file = File.Open(filePath, FileMode.Create))
            {
                using(var memoryStream = new MemoryStream())
                {
                    using(var writer = new StreamWriter(memoryStream))
                    {
                        var serializer = JsonSerializer.CreateDefault();

                        serializer.Serialize(writer, Lexicon);

                        await writer.FlushAsync().ConfigureAwait(false);

                        memoryStream.Seek(0, SeekOrigin.Begin);

                        await memoryStream.CopyToAsync(file).ConfigureAwait(false);
                    }
                }

                await file.FlushAsync().ConfigureAwait(false);
            }
        }

        #endregion Methods
    }
}