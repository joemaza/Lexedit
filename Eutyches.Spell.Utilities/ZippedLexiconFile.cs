//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Eutyches.Spell.Hunspell;

namespace Eutyches.Spell.Utilities
{
    public class ZippedLexiconFile : ILexiconFile
    {
        #region Fields

        /// <summary>
        /// The license file name: "License.txt"
        /// </summary>
        private const string LicenseFileName = "License.txt";

        /// <summary>
        /// The read me file name: "ReadMe.txt"
        /// </summary>
        private const string ReadMeFileName = "ReadMe.txt";

        /// <summary>
        /// The properties
        /// </summary>
        private static readonly PropertyInfo[] Properties = typeof(Lexicon).GetProperties().ToArray();

        /// <summary>
        /// The serializer
        /// </summary>
        private JsonSerializer _serializer = new JsonSerializer()
        {
            //Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        /// The settings
        /// </summary>
        private JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            //Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        #endregion Fields

        #region Methods

        public ZippedLexiconFile(Lexicon lexicon)
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

        public Lexicon Lexicon { get; protected set; }

        /// <summary>
        /// Reads the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>Lexicon.</returns>
        public Lexicon Read(string filePath)
        {
            Lexicon lexicon = new Lexicon();

            using(var zip = ZipFile.OpenRead(filePath))
            {
                foreach(var entry in zip.Entries)
                {
                    // Cycle through the entries of the zip file. *.txt files in the zip file are
                    // ignored because they are only written out should someone inspect the zip file.
                    // Both files are generated only when writing the zip file. Their content is in
                    // the FileMetaInfo property of the Lexicon.
                    if(Path.GetExtension(entry.Name) != ".txt")
                    {
                        // Get the info of the property that matches the entry without the file extension.
                        var info = Properties.Single(p => p.Name == Path.GetFileNameWithoutExtension(entry.Name));

                        // Can the property be written to? If yes, read the entry and set the
                        // property. If not, skip it.
                        if(info.CanWrite)
                        {
                            using(var reader = new StreamReader(zip.GetEntry(entry.Name).Open(), Encoding.UTF8))
                            {
                                var contents = JsonConvert.DeserializeObject(reader.ReadToEnd(), info.PropertyType, _settings);

                                info.SetValue(lexicon, contents);
                            }
                        }
                    }
                    else
                    {
                        // Ignorable *.txt file
                        continue;
                    }
                }
            }

            Lexicon = lexicon;

            return lexicon;
        }

        /// <summary>
        /// read as an asynchronous operation.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>Task&lt;Lexicon&gt;.</returns>
        public async Task<Lexicon> ReadAsync(string filePath)
        {
            Lexicon lexicon = new Lexicon();

            using(ZipArchive zip = await Task.Run(() => ZipFile.OpenRead(filePath)))
            {
                foreach(var entry in zip.Entries)
                {
                    // Cycle through the entries of the zip file. *.txt files in the zip file are
                    // ignored because they are only written out should someone inspect the zip file.
                    // Both files are generated only when writing the zip file. Their content is in
                    // the FileMetaInfo property of the Lexicon.
                    if(Path.GetExtension(entry.Name) != ".txt")
                    {
                        // Get the info of the property that matches the entry without the file extension.
                        var info = Properties.Single(p => p.Name == Path.GetFileNameWithoutExtension(entry.Name));

                        // Can the property be written to? If yes, read the entry and set the
                        // property. If not, skip it.
                        if(info.CanWrite)
                        {
                            using(var reader = new StreamReader(zip.GetEntry(entry.Name).Open(), Encoding.UTF8))
                            {
                                string contents = await reader.ReadToEndAsync();

                                var instance = JsonConvert.DeserializeObject(contents, info.PropertyType, _settings);

                                info.SetValue(lexicon, instance);
                            }
                        }
                    }
                    else
                    {
                        // Ignorable *.txt file
                        continue;
                    }
                }
            }

            Lexicon = lexicon;

            return lexicon;
        }

        /// <summary>
        /// Writes the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="lexicon">The lexicon.</param>
        public void Write(string filePath)
        {
            var mode = File.Exists(filePath) ? ZipArchiveMode.Update : ZipArchiveMode.Create;

            using(var zip = ZipFile.Open(filePath, mode, Encoding.UTF8))
            {
                // Delete all entries if the mode is update
                if(mode == ZipArchiveMode.Update)
                {
                    foreach(var entry in zip.Entries.ToList())
                    {
                        entry.Delete();
                    }
                }

                // License.txt
                using(var writer = new StreamWriter(zip.CreateEntry(LicenseFileName, CompressionLevel.Optimal).Open()))
                {
                    writer.Write(Lexicon.MetaInfo.License);
                }

                // ReadMe.txt
                using(var writer = new StreamWriter(zip.CreateEntry(ReadMeFileName, CompressionLevel.Optimal).Open()))
                {
                    writer.Write(Lexicon.MetaInfo.ReadMe);
                }

                foreach(var info in Properties)
                {
                    if(!info.CanWrite)
                    {
                        continue;
                    }

                    using(var writer = new StreamWriter(zip.CreateEntry($"{info.Name}.json", CompressionLevel.Optimal).Open()))
                    {
                        var contents = info.GetValue(Lexicon);

                        _serializer.Serialize(writer, contents);
                    }
                }
            }
        }

        /// <summary>
        /// write as an asynchronous operation.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="lexicon">The lexicon.</param>
        /// <returns>Task.</returns>
        public async Task WriteAsync(string filePath)
        {
            using(var zip = await Task.Run(() => ZipFile.Open(filePath, ZipArchiveMode.Update, Encoding.UTF8)))
            {
                // License.txt
                using(var writer = new StreamWriter(zip.CreateEntry(LicenseFileName, CompressionLevel.Optimal).Open()))
                {
                    writer.Write(Lexicon.MetaInfo.License);
                }

                // ReadMe.txt
                using(var writer = new StreamWriter(zip.CreateEntry(ReadMeFileName, CompressionLevel.Optimal).Open()))
                {
                    writer.Write(Lexicon.MetaInfo.ReadMe);
                }

                foreach(var info in Properties)
                {
                    if(!info.CanWrite)
                    {
                        continue;
                    }

                    using(var writer = new StreamWriter(zip.CreateEntry($"{info.Name}.json", CompressionLevel.Optimal).Open()))
                    {
                        var contents = info.GetValue(Lexicon);

                        _serializer.Serialize(writer, contents);
                    }
                }
            }
        }

        #endregion Methods
    }
}