//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Eutyches.Spell.Hunspell;

namespace Eutyches.Spell.Utilities
{
    public class StemParser : ILexiconFile
    {
        #region Classes

        public static class Extension
        {
            #region Fields

            public const string TaggedFile = ".txt";

            #endregion Fields
        }

        #endregion Classes

        #region Fields

        private Dictionary<Guid, string> _lookingForBase = new Dictionary<Guid, string>();
        private Dictionary<Guid, string> _lookingForRoot = new Dictionary<Guid, string>();
        private List<Affix> _referenceAffixes = new List<Affix>();
        private List<Stem> _referenceStems = new List<Stem>();

        public List<ImportResult> Results { get; protected set; } = new List<ImportResult>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StemParser"/> class.
        /// </summary>
        /// <param name="lexicon">The lexicon.</param>
        public StemParser(Lexicon lexicon)
        {
            SetLexicon(lexicon);
        }

        public Lexicon Lexicon { get; protected set; }

        /// <summary>
        /// Sets the lexicon.
        /// </summary>
        /// <param name="lexicon">The lexicon.</param>
        private void SetLexicon(Lexicon lexicon = null)
        {
            if(!(lexicon is null))
            {
                Lexicon = lexicon;

                // Get the collection of affixes. These will be used to check the flags of the new stems
                _referenceAffixes = new List<Affix>(lexicon.Affixes.Select(a => a));

                // Make an internal copy of the stems. These will be used as a reference collection
                // for associating roots and bases. New stems will be added the copy will prevent
                // modifying the original should the import NOT go forward.
                _referenceStems = new List<Stem>(lexicon.Stems.Select(s => s));
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>The errors.</value>
        public IEnumerable<ImportResult> Errors => Results.Where(r => r.Status == ImportStatus.Error);

        /// <summary>
        /// Gets the imported stems.
        /// </summary>
        /// <value>The imported stems.</value>
        public List<Stem> ImportedStems => new List<Stem>(FilterStems(ImportStatus.Succeeded));

        /// <summary>
        /// Gets the stems with warnings.
        /// </summary>
        /// <value>The stems with warnings.</value>
        public List<Stem> StemsWithWarnings => new List<Stem>(FilterStems(ImportStatus.Warning));

        /// <summary>
        /// Gets the succeeded.
        /// </summary>
        /// <value>The succeeded.</value>
        public IEnumerable<ImportResult> Succeeded => Results.Where(r => r.Status == ImportStatus.Succeeded);

        /// <summary>
        /// Gets the warnings.
        /// </summary>
        /// <value>The warnings.</value>
        public IEnumerable<ImportResult> Warnings => Results.Where(r => r.Status == ImportStatus.Warning);

        public static string FormatStems(Lexicon lexicon)
        {
            return FormatStems(lexicon.Stems);
        }

        /// <summary>
        /// Formats the stems.
        /// </summary>
        /// <param name="stems">The stems.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="ArgumentNullException">stems</exception>
        public static string FormatStems(IEnumerable<Stem> stems)
        {
            if(stems is null)
            {
                throw new ArgumentNullException(nameof(stems));
            }

            var sb = new StringBuilder();

            var local = DateTime.Now.ToLocalTime();
            var utc = DateTime.Now.ToUniversalTime();

            sb.AppendLine($"{Delimiter.Comment} {DateTime.Now.ToLocalTime().ToString("R")} [LOC: {local.ToString("O")}; UTC: {utc.ToString("O")}]");

            foreach(var stem in stems.OrderBy(s => s.Form))
            {
                sb.AppendLine(Format(stem));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Compiles the specified file paths.
        /// </summary>
        /// <param name="filePaths">The file paths.</param>
        /// <returns>System.Int32.</returns>
        public int Compile(IEnumerable<string> filePaths)
        {
            int count = 0;

            foreach(var file in filePaths)
            {
                if(!File.Exists(file))
                {
                    var result = new ImportResult { Status = ImportStatus.Error };
                    result.AddMessage($"{MessageType.ItemError} File not found ('{file}').");
                    continue;
                }

                var content = TextFile.Parse(file);

                count += ParseLines(content);
            }

            return count;
        }

        /// <summary>
        /// Parses the lines.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <returns>System.Int32.</returns>
        public int ParseLines(IEnumerable<string> lines, bool reset = false)
        {
            int parsedCount = 0;

            if(reset)
            {
                // Reset the collections
                Results.Clear();
            }

            foreach(var line in lines)
            {
                var trimmed = line.Trim();

                // Don't process lines beginning with a comment or blank lines.
                if(trimmed.StartsWith(Delimiter.Comment) | string.IsNullOrWhiteSpace(trimmed))
                {
                    continue;
                }

                ParseItem(trimmed);
                parsedCount++;
            }

            LinkStems(_lookingForRoot);
            LinkStems(_lookingForBase, true);

            if(parsedCount == 0)
            {
                var error = new ImportResult { Status = ImportStatus.Error };
                error.AddMessage($"{MessageType.ItemError} File does not contain recognizable data (lines: {lines.Count()}).");

                Results.Add(error);
            }

            return parsedCount;
        }

        /// <summary>
        /// Reads from the specified file path and parses the file contents.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>A <see cref="Lexicon"/> with only stems , no affixes or other options.</returns>
        public Lexicon Read(string filePath)
        {
            var content = File.ReadAllLines(filePath, Encoding.UTF8);

            ParseLines(content, true);

            var lexicon = new Lexicon();

            lexicon.Stems.AddRange(_referenceStems);

            return lexicon;
        }

        /// <summary>
        /// Writes to the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="lexicon">The lexicon.</param>
        public void Write(string filePath)
        {
            File.WriteAllText(filePath, FormatStems(Lexicon));
        }

        /// <summary>
        /// Converts an instance of <see cref="Stem"/> to formatted string representation.
        /// </summary>
        /// <param name="stem">The stem.</param>
        /// <returns>
        /// A formatted <see cref="String"/> representation of this <see cref="Stem"/> instance.
        /// </returns>
        private static string Format(Stem stem)
        {
            var tokens = new List<Token> {
                new Token{ Key = Key.Text, Value = stem.Form }
            };

            if(stem.Category != Category.None)
            {
                tokens.Add(new Token { Key = Key.Category, Value = stem.Category.ToTag() });
            }

            tokens.Add(new Token { Key = Key.Id, Value = stem.Id.ToString() });

            if(!(stem.RootId is null))
            {
                tokens.Add(new Token { Key = Key.RootId, Value = stem.RootId.ToString() });
            }

            if(!(stem.BaseId is null))
            {
                tokens.Add(new Token { Key = Key.BaseId, Value = stem.BaseId.ToString() });
            }

            if(stem.RequisiteValues != Requisites.Value.None)
            {
                tokens.Add(new Token { Key = Key.Requisite, Value = stem.RequisiteValues.ToTokens() });
            }

            if(stem.SuggestionValues != Suggestion.Value.Default)
            {
                tokens.Add(new Token { Key = Key.Suggestion, Value = stem.SuggestionValues.ToTokens() });
            }

            if(stem.CompoundingValues != Compounding.Value.Default)
            {
                tokens.Add(new Token { Key = Key.Compounding, Value = stem.CompoundingValues.ToTokens() });
            }

            if(stem.Affixes?.Count > 0)
            {
                tokens.Add(new Token { Key = Key.Affixes, Value = string.Join(Delimiter.Item, stem.Affixes) });
            }

            if(!string.IsNullOrWhiteSpace(stem.Sense))
            {
                tokens.Add(new Token
                {
                    Key = Key.Sense,

                    // Replace special characters
                    Value = stem.Sense?.Escape(Delimiter.NeedToEscape).Replace(Delimiter.SemiColon, Delimiter.Pipe)
                });
            }

            if(!string.IsNullOrWhiteSpace(stem.Comments))
            {
                tokens.Add(new Token
                {
                    Key = Key.Comments,
                    Value = stem.Comments.Escape(Delimiter.NeedToEscape).Replace(Delimiter.SemiColon, Delimiter.Pipe)
                });
            }

            return string.Join(Delimiter.Token, tokens);
        }

        /// <summary>
        /// Affixes the exists.
        /// </summary>
        /// <param name="flag">The flag.</param>
        /// <returns>
        /// <c>true</c> if the <see cref="Affix"/> with the flag exists in the collection,
        /// <c>false</c> otherwise.
        /// </returns>
        private bool AffixExists(string flag)
        {
            var affix = _referenceAffixes.SingleOrDefault(a => a.Flag == flag);
            return !(affix is null);
        }

        /// <summary>
        /// Filters the stems.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>IEnumerable&lt;Stem&gt;.</returns>
        private IEnumerable<Stem> FilterStems(ImportStatus status)
        {
            var results = Results.Where(r => r.Status == status);

            return _referenceStems.Join(
                results,
                stem => stem.Id,
                result => result.Id,
                (stem, result) => stem);
        }

        /// <summary>
        /// Gets the stem identifier.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="category">The category.</param>
        /// <param name="sense">The sense.</param>
        /// <returns>System.Nullable&lt;System.Guid&gt;.</returns>
        private Guid? GetStemId(string text, string category, string sense = "")
        {
            Guid? id = null;
            Stem stem = null;
            Category cat = string.IsNullOrWhiteSpace(category) ? Category.None : category.ParseTag();

            if(string.IsNullOrWhiteSpace(sense))
            {
                stem = _referenceStems.FirstOrDefault(s => s.Form == text & s.Category == cat);
            }
            else
            {
                stem = _referenceStems.FirstOrDefault(s => s.Form == text & s.Category == cat & s.Sense == sense);
            }

            if(!(stem is null))
            {
                id = stem.Id;
            }

            if(id is null)
            {
                DebugHelper.Warning($"Could not find matching stem ('{text}', '{category}', '{sense}'). ID still null; OK.");
            }

            return id;
        }

        /// <summary>
        /// Associates the stems.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="isBase">if set to <c>true</c> [is base].</param>
        private void LinkStems(Dictionary<Guid, string> dictionary, bool isBase = false)
        {
            foreach(var id in dictionary.Keys)
            {
                // Get the derived stem and the data for its target.
                var stem = _referenceStems.SingleOrDefault(s => s.Id == id);

                Debug.Assert(!(stem is null), $"Stem is null when it should have been found!");

                var result = Results.SingleOrDefault(r => r.Id == id);

                Debug.Assert(!(result is null), $"Result is null when it should have been found!");

                // Get the target id
                if(TryParseDependency(dictionary[id], out Stem dep, ref result))
                {
                    // Make the associations
                    if(isBase)
                    {
                        stem.BaseId = dep.Id;
                    }
                    else
                    {
                        stem.RootId = dep.Id;
                    }

                    // Update the dependency
                    if(dep.Data is null)
                    {
                        dep.Data = new List<Datum>();
                    }

                    dep.Data.Add(new Datum(DatumType.Allomorph, stem.Form));
                }
            }
        }

        private void ParseItem(string text)
        {
            // Start off optimistic with the results. Change the properties as necessary.
            var result = new ImportResult
            {
                Status = ImportStatus.Succeeded,
                Text = text.Trim()
            };

            // Add the result to the results list.
            Results.Add(result);

            // See if there might be at least a token to parse, i.e., "t:text", the bare minimum.
            if(!text.ToLowerInvariant().Contains($"{Key.Text}{Delimiter.KeyValue}"))
            {
                // "t:" was not found, does it contain "e:", the alt?
                if(!text.ToLowerInvariant().Contains($"{Key.Text}{Delimiter.KeyValue}"))
                {
                    result.Status = ImportStatus.Error;
                    result.AddMessage($"{MessageType.ItemError} There were no recognizable tokens found.");
                    result.AddMessage($" The mandatory token ('t:' or 'e:') was not found.");

                    return;
                }

                // If we get here, the "e:" was used to specify the text
            }

            // Extract token of data from the text. Return if an error was found.
            var tokens = Tokenize(text, ref result);

            if(result.Status == ImportStatus.Error)
            {
                return;
            }

            // Now we can bother to create a stem since the text is passed error checks. Anything
            // found beyond this point is a warning, except for a missing value for the text key.
            Stem stem = new Stem();

            foreach(var token in tokens)
            {
                switch(token.Key)
                {
                    case Key.Id:
                        if(Guid.TryParse(token.Value, out Guid guid))
                        {
                            result.Id = stem.Id = guid;
                        }
                        break;

                    case Key.RootId:
                        if(Guid.TryParse(token.Value, out Guid rid))
                        {
                            stem.RootId = rid;
                        }
                        break;

                    case Key.BaseId:
                        if(Guid.TryParse(token.Value, out Guid bid))
                        {
                            stem.BaseId = bid;
                        }
                        break;

                    case Key.Text:
                    case Key.TextAlt:
                        stem.Form = token.Value;
                        break;

                    case Key.Category:
                        stem.Category = token.Value.ParseTag();
                        break;

                    case Key.Root:

                        // Put the Id of this stem in the lookingForRoot
                        _lookingForRoot.Add(stem.Id, token.Value);
                        break;

                    case Key.Base:

                        // Put the Id of this stem in the lookingForBase
                        _lookingForBase.Add(stem.Id, token.Value);
                        break;

                    case Key.Requisite:
                        stem.RequisiteValues |= Requisites.ToValue(token.Value);
                        break;

                    case Key.Suggestion:
                        stem.SuggestionValues |= Suggestion.ToValue(token.Value);
                        break;

                    case Key.Compounding:
                        stem.CompoundingValues |= Compounding.ToValue(token.Value);
                        break;

                    case Key.Affixes:
                    case Key.AffixesAlt:
                        if(string.IsNullOrWhiteSpace(token.Value))
                        {
                            break;
                        }

                        stem.Affixes = new List<string>(token
                            .Value
                            .Split(new string[] { Delimiter.Item }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(t => t.Trim())
                            .Distinct());

                        foreach(var affix in stem.Affixes)
                        {
                            if(!AffixExists(affix))
                            {
                                result.Status = ImportStatus.Warning;
                                result.AddMessage($"{MessageType.TokenWarning} Affix not found ('{affix}').");
                            }
                        }

                        break;

                    case Key.Sense:

                        // Replace any space, ' ', with a period, '.'.
                        stem.Sense = token.Value?.Replace(Delimiter.Space, Delimiter.Morpheme)
                            .Unescape()
                            .Replace(Delimiter.Pipe, Delimiter.SemiColon);
                        break;

                    case Key.Comments:
                        stem.Comments = token.Value?.Unescape()
                            .Replace(Delimiter.Pipe, Delimiter.SemiColon);
                        break;

                    default:
                        result.Status = ImportStatus.Warning;
                        result.AddMessage($"{MessageType.TokenWarning} Unrecognized key ('{token}').");
                        break;
                }
            }

            // Set the ID for the results for tracking. This is late as the ID might have been parsed
            // from the text.
            if(result.Id == Guid.Empty)
            {
                result.Id = stem.Id;
            }

            // Duplicate check
            if(_referenceStems.Where(s => s.Form == stem.Form & s.Category == stem.Category).Count() > 0)
            {
                result.Status = ImportStatus.Warning;
                result.AddMessage($"{MessageType.TokenWarning} Possible duplicate ('{stem.Form}').");

                DebugHelper.Warning(result.Message);
            }

            DebugHelper.Info($"Adding stem (text: '{stem.Form}')...");

            _referenceStems.Add(stem);
        }

        /// <summary>
        /// Tokenizes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="result">The result.</param>
        /// <returns>IEnumerable&lt;Token&gt;.</returns>
        private IEnumerable<Token> Tokenize(string item, ref ImportResult result)
        {
            var tokens = new List<Token>();

            var segments = item
                .Trim()
                .Split(new string[] { Delimiter.Token }, StringSplitOptions.RemoveEmptyEntries);

            if(segments.Length < 1)
            {
                result.Status = ImportStatus.Error;
                result.AddMessage($"{MessageType.ItemError} There were no recognizable tokens found.");

                return null;
            }

            foreach(var text in segments)
            {
                if(Token.TryParse(text, out Token token, out string message))
                {
                    tokens.Add(token);
                }
                else
                {
                    result.Status = ImportStatus.Warning;
                    result.AddMessage(message);
                }
            }

            // Check the text token
            var textTokenCount = tokens.Count(t => t.Key == Key.Text | t.Key == Key.TextAlt);

            if(textTokenCount < 1)
            {
                result.Status = ImportStatus.Error;
                result.AddMessage($"{MessageType.TokenError} Text token was not specified.");

                return null;
            }
            else if(textTokenCount > 1)
            {
                result.Status = ImportStatus.Error;
                result.AddMessage($"{MessageType.TokenError} Text token was specified multiple times (count: {textTokenCount}).");

                return null;
            }

            var idTokenCount = tokens.Count(t => t.Key == Key.Id);
            if(idTokenCount == 1)
            {
                // Check if the id supplied clashes with another
                var idToken = tokens.SingleOrDefault(t => t.Key == Key.Id);

                if(idToken is null)
                {
                    // Odd... You just said there was one.
                    var message = $"ID token count == 1; yet, it was not found.";

                    DebugHelper.Warning(message);

                    result.Status = ImportStatus.Error;
                    result.AddMessage($"{MessageType.TokenError} {message}");

                    return null;
                }
                else
                {
                    if(Guid.TryParse(idToken.Value, out Guid id))
                    {
                        var originalCount = _referenceStems.Count(s => s.Id == id);

                        if(originalCount > 0)
                        {
                            var newId = Guid.NewGuid().ToString();

                            result.Status = ImportStatus.Error;
                            result.AddMessage($"{MessageType.TokenError} ID already exits ('{idToken.Value}')!");

                            return null;
                        }
                    }
                }
            }
            else if(idTokenCount > 1)
            {
                result.Status = ImportStatus.Error;
                result.AddMessage($"{MessageType.TokenError} ID specified multiple times (count: {idTokenCount})!");

                return null;
            }

            var affixesTokenCount = tokens.Count(t => t.Key == Key.Affixes | t.Key == Key.AffixesAlt);

            if(affixesTokenCount > 1)
            {
                result.Status = ImportStatus.Warning;
                result.AddMessage($"{MessageType.TokenWarning} Affix token specified multiple times (count: {affixesTokenCount}).");
            }

            // Check other keys for duplicates. Mark them as warnings.
            foreach(var key in Key.OptionalKeys)
            {
                var count = tokens.Count(t => t.Key == key);

                if(count > 1)
                {
                    result.Status = ImportStatus.Warning;
                    result.AddMessage($"{MessageType.TokenWarning} Token ('{key}') specified multiple times (count: {count}).");
                }
            }

            return tokens;
        }

        /// <summary>
        /// Tries to parse for the dependency.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="stem">The stem.</param>
        /// <param name="message">The message.</param>
        /// <returns><c>true</c> if the dependency was found, <c>false</c> otherwise.</returns>
        private bool TryParseDependency(string text, out Stem stem, ref ImportResult result)
        {
            stem = null;

            // Indexes: 0 - Form, 1 - Category (optional iff sense is present), 2 - Sense (optional)
            var data = text
                .Split(new string[] { Delimiter.Item }, StringSplitOptions.None)
                .Select(r => { return r.Trim(); })
                .ToArray();

            if(data.Length < 2)
            {
                result.Status = ImportStatus.Warning;
                result.AddMessage($"{MessageType.TokenWarning} Insufficient data ('{text}').");

                return false;
            }
            else if(data.Length == 2)
            {
                // Search by text and category: "text,category"
                stem = _referenceStems.FirstOrDefault(s => s.Form == data[0] & s.Category == data[1].ParseTag());
            }
            else if(data.Length >= 3)
            {
                if(data.Length > 3)
                {
                    result.Status = ImportStatus.Warning;

                    // There was a comma after sense, viz., "text,category,sense,extra stuff"
                    result.AddMessage($"{MessageType.TokenWarning} Extra text found ('{text}').");
                }

                if(string.IsNullOrWhiteSpace(data[1]))
                {
                    // Search by text and sense: "text,,sense"
                    stem = _referenceStems.FirstOrDefault(s => s.Form == data[0] & s.Sense == data[2].ToLowerInvariant());
                }
                else
                {
                    // Search by text, category and sens: "text,category,sense"
                    stem = _referenceStems.FirstOrDefault(s => s.Form == data[0] & s.Category == data[1].ParseTag() & s.Sense == data[2].ToLowerInvariant());
                }
            }

            if(stem is null)
            {
                result.Status = ImportStatus.Warning;
                result.AddMessage($"{MessageType.TokenWarning} Dependency was not found ('{text}').");

                return false;
            }

            return true;
        }

        #endregion Methods

        #region Classes

        /// <summary>
        /// Delimiters in the plain-text file used to list the <see cref="Stems"/>.
        /// </summary>
        public static class Delimiter
        {
            #region Fields

            public const string Ampersand = "&";
            public const string Comment = "#";
            public const string Equal = "=";
            public const string Item = ",";
            public const string KeyValue = ":";
            public const string Morpheme = ".";
            public const string Pipe = "|";
            public const string SemiColon = Token;
            public const string Space = " ";
            public const string Token = ";";

            public static char[] NeedToEscape
            {
                get
                {
                    var chars = new List<char> {
                        Token[0],
                        Comment[0],
                        KeyValue[0]
                    };

                    chars.AddRange(Environment.NewLine.ToCharArray());

                    return chars.ToArray();
                }
            }

            #endregion Fields
        }

        /// <summary>
        /// Names or "tags" of the <see cref="Stem"/> properties.
        /// </summary>
        public static class Key
        {
            #region Fields

            public const string Affixes = "af";
            public const string AffixesAlt = "fl";
            public const string Base = "bq";
            public const string BaseId = "bi";
            public const string Category = "ct";
            public const string Comments = "nt";
            public const string Compounding = "cx";
            public const string Id = "id";
            public const string Requisite = "op";
            public const string Root = "rq";
            public const string RootId = "ri";
            public const string Sense = "mn";
            public const string Suggestion = "sg";
            public const string Text = "tt";
            public const string TextAlt = "en";

            #endregion Fields

            /// <summary>
            /// The optional keys
            /// </summary>
            public static string[] OptionalKeys = new string[]
            {
                Base,
                BaseId,
                Category,
                Comments,
                Compounding,
                Requisite,
                Root,
                RootId,
                Sense,
                Suggestion
            };
        }

        public static class MessageType
        {
            #region Fields

            public const string ItemError = "[Item]";
            public const string ItemWarning = "[Item]";
            public const string TokenError = "[Token]";
            public const string TokenWarning = "[Token]";

            #endregion Fields
        }

        /// <summary>
        /// Represents the value of a <see cref="Stem"/> property.
        /// </summary>
        public class Token
        {
            #region Properties

            /// <summary>
            /// Gets or sets the string representation of the name of the <see cref="Stem"/> property.
            /// </summary>
            /// <value>The key.</value>
            public string Key { get; set; }

            /// <summary>
            /// Gets or sets the string represention of the value of the <see cref="Stem"/> property.
            /// </summary>
            /// <value>The value.</value>
            public string Value { get; set; }

            #endregion Properties

            #region Methods

            /// <summary>
            /// Converts the string representation of Token into a <see cref="Token"/> instance.
            /// </summary>
            /// <param name="text">The text.</param>
            /// <param name="value">The value.</param>
            /// <param name="message">The message (any information about the text)</param>
            /// <returns>
            /// <c>true</c> if an instance of <see cref="Token"/> was created, <c>false</c>
            /// otherwise. (Any information about a failure will be found in the <c>out</c> message parameter.)
            /// </returns>
            public static bool TryParse(string text, out Token value, out string message)
            {
                message = string.Empty;

                // Split the "key:value" and remove any external whitespace.
                var token = text.Split(new string[] { Delimiter.KeyValue }, StringSplitOptions.None)
                    .Select(t => t.Trim())
                    .ToArray();

                if(token.Length < 2)
                {
                    value = null;
                    message = $"{MessageType.TokenError} Data missing ('{text}').";

                    return false;
                }
                else if(token.Length == 2)
                {
                    bool error = false;

                    // Key
                    if(string.IsNullOrWhiteSpace(token[0]))
                    {
                        error = true;
                        message += $"{MessageType.TokenError} Key missing ('{text}').";
                    }

                    // Value
                    if(string.IsNullOrWhiteSpace(token[1]))
                    {
                        error = true;
                        message += $"{MessageType.TokenError} Value missing ('{text}').";
                    }

                    if(error)
                    {
                        value = null;

                        return false;
                    }
                }
                else if(token.Length > 2)
                {
                    message = $"{MessageType.TokenWarning} Text contains extra data ('{text}').";
                }

                value = new Token { Key = token[0], Value = token[1] };
                message = string.Empty;

                return true;
            }

            /// <summary>
            /// Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
            public override string ToString()
            {
                return $"{Key}{Delimiter.KeyValue}{Value}";
            }

            #endregion Methods
        }

        #endregion Classes
    }
}