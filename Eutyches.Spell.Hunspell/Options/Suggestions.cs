//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eutyches.Spell.Hunspell
{
    public static class Suggestion
    {
        #region Enums

        [Flags]
        public enum Value
        {
            Default = 0b00,
            NoSuggest = 0b01,
            Warn = 0b10,
        }

        #endregion Enums

        #region Classes

        public static class Flag
        {
            #region Fields

            public const string NoSuggest = "~~";
            public const string Warn = "%%";

            #endregion Fields
        }

        public static class Token
        {
            #region Fields

            public const char NoSuggest = 'N';
            public const char Warn = 'W';

            #endregion Fields
        }

        #endregion Classes

        #region Methods

        public static void Clear(this Value flags, Value value)
        {
            flags &= ~(value);
        }

        public static bool IsSet(this Value flags, Value value)
        {
            return (flags & value) == value;
        }

        public static void Set(this Value flags, Value value)
        {
            flags |= value;
        }

        public static IEnumerable<string> ToFlags(this Value value, Lexicon lexicon)
        {
            var op = lexicon.SuggestionOptions;

            var flags = new List<string>();

            foreach(var enumValue in Enum.GetValues(typeof(Value)))
            {
                var ev = (Value) enumValue;

                if((value & ev) == ev)
                {
                    switch(ev)
                    {
                        case Value.NoSuggest:
                            flags.Add(string.IsNullOrWhiteSpace(op?.NoSuggest?.Value)
                                ? Flag.NoSuggest : op?.NoSuggest?.Value);
                            break;

                        case Value.Warn:
                            flags.Add(string.IsNullOrWhiteSpace(op?.Warn?.Value)
                                ? Flag.Warn : op?.Warn?.Value);
                            break;

                        default:
                            break;
                    }
                }
            }

            return flags;
        }

        public static string ToTokens(this Value value)
        {
            StringBuilder s = new StringBuilder();

            if(value.IsSet(Value.NoSuggest)) s.Append(Token.NoSuggest);
            if(value.IsSet(Value.Warn)) s.Append(Token.Warn);

            return s.ToString();
        }

        public static Value ToValue(string tokens)
        {
            var value = Value.Default;

            if(string.IsNullOrWhiteSpace(tokens))
            {
                return value;
            }

            foreach(var t in tokens.ToUpperInvariant().Distinct())
            {
                value |= ToValue(t);
            }

            return value;
        }

        public static Value ToValue(char token)
        {
            var value = Value.Default;

            switch(token)
            {
                case Token.NoSuggest:
                    value |= Value.NoSuggest;
                    break;

                case Token.Warn:
                    value |= Value.Warn;
                    break;

                default:
                    DebugHelper.Warning($"Token unknown: {token}");
                    break;
            }

            return value;
        }

        #endregion Methods
    }
}