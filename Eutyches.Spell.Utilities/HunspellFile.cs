//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using Eutyches.Spell.Hunspell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eutyches.Spell.Utilities
{
    public class HunspellFile
    {
        #region Fields

        /// <summary>
        /// The affix file extension
        /// </summary>
        public const string AffixFileExtension = ".aff";

        /// <summary>
        /// The dictionary file extension
        /// </summary>
        public const string DictionaryFileExtension = ".dic";

        /// <summary>
        /// The name width
        /// </summary>
        private const int NameWidth = -16;

        /// <summary>
        /// The option width
        /// </summary>
        private const int OptionWidth = -24;

        /// <summary>
        /// The separator width
        /// </summary>
        private const int SeparatorWidth = 120;

        #endregion Fields

        #region Methods

        public HunspellFile(Lexicon lexicon)
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

        public string AffixListingText()
        {
            var primaries = new List<string>(Lexicon.Affixes
               .Where(aff => aff.IsPrimary == true)
               .Select(aff => $"*[{aff.Flag}] [{aff.Type.ToFlag()}] [{aff.Rules.Count(),5}]: {aff.Label}"));

            primaries.Sort((a1, a2) => string.CompareOrdinal(a1, a2));

            var dependents = new List<string>(Lexicon.Affixes
                .Where(aff => aff.IsPrimary == false)
                .Select(aff => $" [{aff.Flag}] [{aff.Type.ToFlag()}] [{aff.Rules.Count(),5}]: {aff.Label}"));

            dependents.Sort((a1, a2) => string.CompareOrdinal(a1, a2));

            var sb = new StringBuilder();

            sb.AddCommentBlock($"AFFIX LISTING - COUNT: {Lexicon.Affixes.Count} [Primary: {primaries.Count}; Dependents: {dependents.Count}]");
            sb.AddComments(primaries);
            sb.AddComments(dependents);

            return sb.ToString();
        }

        /// <summary>
        /// Gets the affix file text.
        /// </summary>
        /// <param name="lexicon">The lexicon.</param>
        /// <param name="addComments">if set to <c>true</c> [add comments].</param>
        /// <returns>System.String.</returns>
        public string GetAffixFileText(bool addComments)
        {
            var sb = new StringBuilder();

            // FileMetaInfo
            sb.AppendLine(FileMetaInfoText(addComments));

            // GeneralOptions
            sb.AppendLine(GeneralOptionsText(addComments));

            // SuggestionOptions
            sb.AppendLine(SuggestionOptionsText(addComments));

            // ConversionOptions
            sb.AppendLine(ConversionOptionsText(addComments));

            // CompoundingOptions
            sb.AppendLine(CompoundingOptionsText(addComments));

            // Affixes
            sb.AppendLine(AffixesText(addComments));

            return sb.ToString();
        }

        /// <summary>
        /// Gets the dictionary file text.
        /// </summary>
        /// <param name="lexicon">The lexicon.</param>
        /// <param name="addComments">if set to <c>true</c> [add comments].</param>
        /// <returns>System.String.</returns>
        public string GetDictionaryFileText(bool addComments)
        {
            Lexicon.Stems.Sort();

            var sb = new StringBuilder();

            sb.AppendLine(Lexicon.Stems.Count.ToString());

            foreach(var stem in Lexicon.Stems)
            {
                sb.AppendLine(stem.ToText(Lexicon, addComments));
            }

            return sb.ToString();
        }

        public void WriteAffixFile(string filePath, bool addComments = false, Encoding encoding = null)
        {
            if(encoding is null)
            {
                encoding = Encoding.UTF8;
            }

            File.WriteAllText(filePath, GetAffixFileText(addComments), Encoding.UTF8);
        }

        /// <summary>
        /// write affix file as an asynchronous operation.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="lexicon">The lexicon.</param>
        /// <param name="addComments">if set to <c>true</c> [add comments].</param>
        /// <returns>Task.</returns>
        public async Task WriteAffixFileAsync(string filePath, bool addComments = false, Encoding encoding = null)
        {
            if(encoding is null)
            {
                encoding = Encoding.UTF8;
            }

            using(var stream = new StreamWriter(filePath, false, encoding))
            {
                await stream.WriteAsync(GetAffixFileText(addComments));
            }
        }

        /// <summary>
        /// Writes the affix listing.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void WriteAffixListing(string filePath)
        {
            File.WriteAllText(filePath, AffixListingText());
        }

        /// <summary>
        /// Writes the dictionary file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="lexicon">The lexicon.</param>
        /// <param name="addComments">if set to <c>true</c> [add comments].</param>
        public void WriteDictionaryFile(string filePath, bool addComments = false, Encoding encoding = null)
        {
            if(encoding is null)
            {
                encoding = Encoding.UTF8;
            }

            File.WriteAllText(filePath, GetDictionaryFileText(addComments), encoding);
        }

        /// <summary>
        /// write dictionary file as an asynchronous operation.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="lexicon">The lexicon.</param>
        /// <param name="addComments">if set to <c>true</c> [add comments].</param>
        /// <returns>Task.</returns>
        public async Task WriteDictionaryFileAsync(string filePath, bool addComments = false, Encoding encoding = null)
        {
            if(encoding is null)
            {
                encoding = Encoding.UTF8;
            }
            using(var stream = new StreamWriter(filePath, false, encoding))
            {
                await stream.WriteAsync(GetDictionaryFileText(addComments));
            }
        }

        private string AffixesText(bool addComments)
        {
            Lexicon.Affixes.Sort();

            var sb = new StringBuilder();
            sb.AddCommentBlock(new string[] { "AFFIX CLASSES", $"COUNT: {Lexicon.Affixes.Count}" });

            foreach(var affix in Lexicon.Affixes)
            {
                // Add header
                if(addComments)
                {
                    sb.AddCommentBar();
                    sb.AddComments($"[{affix.Flag}] {affix.Label.ToUpperInvariant()}");

                    var comments = affix.Comments?.SplitToLines()?.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());

                    if(comments.Count() > 0)
                    {
                        sb.AddSectionBar();

                        sb.AddComments(comments);
                    }

                    sb.AddCommentBar();
                }

                // Affix Intro
                sb.AppendLine($"{affix.Type.ToFlag()} {affix.Flag} {(affix.CanCombine ? 'Y' : 'N')} {affix.Rules.Count}");

                // Sort the rules
                affix.Rules.Sort();

                // Rules
                sb.AppendLineCollection(affix.Rules.Select(r => r.ToText(affix, Lexicon, addComments)));

                if(addComments)
                {
                    sb.AppendLine(string.Empty);
                }
            }

            return sb.ToString();
        }

        private string CompoundingOptionsText(bool addComments)
        {
            var sb = new StringBuilder();

            if(addComments)
            {
                sb.AddCommentBlock("COMPOUNDING OPTIONS");
            }

            sb.AppendLine(OptionsText(Lexicon.CompoundingOptions, addComments));

            // BREAKS
            if(Lexicon.CompoundingOptions?.Breaks.Count > 0)
            {
                if(addComments)
                {
                    sb.AddSectionBlock("COMPOUNDING BREAKS");
                }

                sb.AppendLine($"BREAK {Lexicon.CompoundingOptions.Breaks.Count}");

                foreach(var @break in Lexicon.CompoundingOptions.Breaks)
                {
                    var line = $"BREAK {@break.Value}";

                    if(addComments)
                    {
                        sb.AppendLine($"{line,NameWidth} # {@break.Comments}");
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                }
            }

            // COMPOUND RULES
            if(Lexicon.CompoundingOptions?.CompoundRules.Count > 0)
            {
                if(addComments)
                {
                    sb.AddSectionBlock("COMPOUNDING RULES");
                }

                sb.AppendLine($"COMPOUNDRULE {Lexicon.CompoundingOptions.CompoundRules.Count}");

                foreach(var rule in Lexicon.CompoundingOptions.CompoundRules)
                {
                    var line = $"COMPOUNDRULE {rule.Value}";

                    if(addComments)
                    {
                        sb.AppendLine($"{line,NameWidth} # {rule.Comments}");
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                }
            }

            // COMPOUND PATTERNS
            if(Lexicon.CompoundingOptions?.CheckCompoundPatterns.Count > 0)
            {
                if(addComments)
                {
                    sb.AddSectionBlock("COMPOUNDING PATTERNS");
                }

                sb.AppendLine($"CHECKCOMPOUNDPATTERN {Lexicon.CompoundingOptions.CompoundRules.Count}");

                foreach(var pattern in Lexicon.CompoundingOptions.CompoundRules)
                {
                    var line = $"CHECKCOMPOUNDPATTERN {pattern.Value}";

                    if(addComments)
                    {
                        sb.AppendLine($"{line,NameWidth} # {pattern.Comments}");
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                }
            }

            return sb.ToString();
        }

        private string ConversionOptionsText(bool addComments)
        {
            var sb = new StringBuilder();

            if(addComments)
            {
                sb.AddCommentBlock("CONVERSION OPTIONS");
            }

            sb.AppendLine(OptionsText(Lexicon.CompoundingOptions, addComments));

            // INPUT CONVERSIONS
            if(Lexicon.ConversionOptions.InputConversions?.Count > 0)
            {
                if(addComments)
                {
                    sb.AddSectionBlock("INPUT CONVERSIONS");
                }

                sb.AppendLine($"ICONV {Lexicon.ConversionOptions.InputConversions.Count}");

                foreach(var con in Lexicon.ConversionOptions.InputConversions)
                {
                    var line = $"ICONV {con.From} {con.To}";

                    if(addComments & !(string.IsNullOrWhiteSpace(con.Comments)))
                    {
                        sb.AppendLine($"{line,NameWidth} # {con.Comments}");
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                }
            }

            // OUTPUT CONVERSIONS
            if(Lexicon.ConversionOptions.OutputConversions?.Count > 0)
            {
                if(addComments)
                {
                    sb.AddSectionBlock("OUTPUT CONVERSIONS");
                }

                sb.AppendLine($"OCONV {Lexicon.ConversionOptions.InputConversions.Count}");

                foreach(var con in Lexicon.ConversionOptions.InputConversions)
                {
                    var line = $"OCONV {con.From} {con.To}";

                    if(addComments & !(string.IsNullOrWhiteSpace(con.Comments)))
                    {
                        sb.AppendLine($"{line,NameWidth} # {con.Comments}");
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                }
            }

            return sb.ToString();
        }

        private string FileMetaInfoText(bool addComments)
        {
            var sb = new StringBuilder();

            var info = Lexicon.MetaInfo;

            var local = info.Creation.ToLocalTime();
            var utc = info.Creation.ToUniversalTime();

            sb.AddComments(new string[] {
            $"NAME:      {info.Name}",
            $"LANGUAGE:  {info.LanguageName}",
            $"CREATION:  {local.ToString("R")} [LOC: {local.ToString("O")}; UTC: {utc.ToString("O")}]",
            $"CREATOR:   {info.Creator}",
            });

            sb.AddComments($"CONTRIBUTORS:");
            foreach(var contributor in info.Contributors)
            {
                sb.AddComments($" * {contributor}");
            }
            sb.AppendLine(string.Empty);

            sb.AddSectionBlock("SHORT DESCRIPTION");
            sb.AddComments(info.ShortDescription.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));
            sb.AppendLine(string.Empty);

            sb.AddSectionBlock("LONG DESCRIPTION");
            sb.AddComments(info.LongDescription.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));
            sb.AppendLine(string.Empty);

            sb.AddSectionBlock("README");
            sb.AddComments(info.ReadMe.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));
            sb.AppendLine(string.Empty);

            if(addComments)
            {
                sb.AppendLine(AffixListingText());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generals the options text.
        /// </summary>
        /// <param name="lexicon">The lexicon.</param>
        /// <param name="addComments">if set to <c>true</c> [add comments].</param>
        /// <returns>System.String.</returns>
        private string GeneralOptionsText(bool addComments)
        {
            var sb = new StringBuilder();

            if(addComments)
            {
                sb.AddCommentBlock("GENERAL OPTIONS");
            }

            sb.AppendLine(OptionsText(Lexicon.GeneralOptions, addComments));

            return sb.ToString();
        }

        private string OptionsText(object obj, bool addComments)
        {
            var sb = new StringBuilder();

            var properties = from p in obj.GetType().GetProperties()
                             where
                             (p.PropertyType == typeof(Toggle)
                             || p.PropertyType == typeof(FlagOption)
                             || p.PropertyType == typeof(Option<string>)
                             || p.PropertyType == typeof(Option<FlagType>)
                             || p.PropertyType == typeof(Option<int>))
                             orderby p.Name
                             select p;

            foreach(var info in properties)
            {
                var option = info.GetValue(obj);

                // If the particular property is null, then there is no need to output it.
                if(option == null)
                {
                    continue;
                }

                var comments = option.GetType().GetProperty("Comments").GetValue(option)?.ToString();

                var name = info.Name.ToUpperInvariant();

                if(info.PropertyType == typeof(Toggle))
                {
                    if(addComments & (!string.IsNullOrWhiteSpace(comments)))
                    {
                        sb.AppendLine($"{name,OptionWidth} # {comments}");
                    }
                    else
                    {
                        sb.AppendLine(name);
                    }
                }
                else if(info.PropertyType == typeof(Option<FlagType>))
                {
                    var value = option.GetType().GetProperty("Value").GetValue(option);

                    var line = $"{name,NameWidth} {((FlagType) value).ToText()}";

                    if(addComments & (!string.IsNullOrWhiteSpace(comments)))
                    {
                        sb.AppendLine($"{line,OptionWidth} # {comments}");
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                }
                else if(info.PropertyType == typeof(Option<int>)
                   || info.PropertyType == typeof(Option<string>)
                   || info.PropertyType == typeof(FlagOption))
                {
                    var value = option.GetType().GetProperty("Value").GetValue(option).ToString();

                    var line = $"{name,NameWidth} {value}";

                    if(addComments & (!string.IsNullOrWhiteSpace(comments)))
                    {
                        sb.AppendLine($"{line,OptionWidth} # {comments}");
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                }
            }

            if(addComments & (sb.Length > 0))
            {
                sb.AppendLine(string.Empty);
            }

            return sb.ToString();
        }

        private string SuggestionOptionsText(bool addComments)
        {
            var sb = new StringBuilder();

            if(addComments)
            {
                sb.AddCommentBlock("SUGGESTION OPTIONS");
            }

            sb.AppendLine(OptionsText(Lexicon.SuggestionOptions, addComments));

            // MAPPINGS
            if(Lexicon.SuggestionOptions.Mappings?.Count > 0)
            {
                if(addComments)
                {
                    sb.AddSectionBlock("MAPPINGS");
                }

                sb.AppendLine($"MAP {Lexicon.SuggestionOptions.Mappings.Count}");

                foreach(var map in Lexicon.SuggestionOptions.Mappings)
                {
                    var mapping = $"MAP {(map.From.Length > 1 ? $"({map.From})" : map.From)}{(map.To.Length > 1 ? $"({map.To})" : map.To)}";

                    if(addComments)
                    {
                        sb.AppendLine($"{mapping,OptionWidth} # {map.Comments}");
                    }
                    else
                    {
                        sb.AppendLine(mapping);
                    }
                }
            }

            // PHONETIC RULES
            if(Lexicon.SuggestionOptions.Replacements?.Count > 0)
            {
                if(addComments)
                {
                    sb.AddSectionBlock("REPLACEMENTS");
                }

                sb.AppendLine($"REP {Lexicon.SuggestionOptions.Replacements.Count}");

                foreach(var rep in Lexicon.SuggestionOptions.Replacements)
                {
                    var replacement = $"REP {rep.From} {rep.To}";

                    if(addComments)
                    {
                        sb.AppendLine($"{replacement,OptionWidth} # {rep.Comments}");
                    }
                    else
                    {
                        sb.AppendLine(replacement);
                    }
                }
            }

            if(Lexicon.SuggestionOptions.PhoneticRules?.Count > 0)
            {
                if(addComments)
                {
                    sb.AddSectionBlock("PHONETIC RULES");
                }

                sb.AppendLine($"PHONE {Lexicon.SuggestionOptions.PhoneticRules.Count}");

                foreach(var phone in Lexicon.SuggestionOptions.PhoneticRules)
                {
                    var rule = $"PHONE {phone.From} {phone.To}";

                    if(addComments)
                    {
                        sb.AppendLine($"{rule,OptionWidth} # {phone.Comments}");
                    }
                    else
                    {
                        sb.AppendLine(rule);
                    }
                }
            }
            return sb.ToString();
        }

        #endregion Methods
    }

    internal static class StringBuilderHelper
    {
        #region Methods

        public static void AppendLineCollection(this StringBuilder sb, IEnumerable<string> lines)
        {
            foreach(var line in lines)
            {
                sb.AppendLine(line);
            }
        }

        #endregion Methods
    }
}