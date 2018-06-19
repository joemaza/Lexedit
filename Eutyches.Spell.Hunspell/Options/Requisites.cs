//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eutyches.Spell.Hunspell
{
    public static class Requisites
    {
        #region Enums

        [Flags]
        public enum Value
        {
            None = 0b0000,
            NeedAffix = 0b0001,
            KeepCase = 0b0010,
            Substandard = 0b0100,
            ForbiddenWord = 0b1000, // Stem only
            Circumfix = 0b0001_0000 // Affix only
        }

        #endregion Enums

        #region Classes

        public static class Flag
        {
            #region Fields

            public const string Circumfix = "><";
            public const string ForbiddenWord = "!!";
            public const string KeepCase = "^^";
            public const string NeedAffix = "++";
            public const string Substandard = "$$";

            #endregion Fields
        }

        public static class Token
        {
            #region Fields

            public const char Circumfix = 'X';
            public const char ForbiddenWord = 'F';
            public const char KeepCase = 'K';
            public const char NeedAffix = 'A';
            public const char Substandard = 'S';

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
            var op = lexicon.GeneralOptions;

            var flags = new List<string>();

            foreach(var enumValue in Enum.GetValues(typeof(Value)))
            {
                var ev = (Value) enumValue;

                if((value & ev) == ev)
                {
                    switch(ev)
                    {
                        case Value.Circumfix:
                            flags.Add(string.IsNullOrWhiteSpace(op?.Circumfix?.Value)
                                ? Flag.Circumfix : op.Circumfix.Value);
                            break;

                        case Value.ForbiddenWord:
                            flags.Add(string.IsNullOrWhiteSpace(op?.ForbiddenWord?.Value)
                                ? Flag.ForbiddenWord : op.ForbiddenWord.Value);
                            break;

                        case Value.KeepCase:
                            flags.Add(string.IsNullOrWhiteSpace(op?.KeepCase?.Value)
                                ? Flag.KeepCase : op.KeepCase.Value);
                            break;

                        case Value.NeedAffix:
                            flags.Add(string.IsNullOrWhiteSpace(op?.NeedAffix?.Value)
                                ? Flag.KeepCase : op.NeedAffix.Value);
                            break;

                        case Value.Substandard:
                            flags.Add(string.IsNullOrWhiteSpace(op?.Substandard?.Value)
                                ? Flag.Substandard : op.Substandard.Value);
                            break;

                        default:

                            // Do nothing
                            break;
                    }
                }
            }

            return flags;
        }

        public static string ToTokens(this Value value)
        {
            var s = new StringBuilder();

            if(value.IsSet(Value.Circumfix)) s.Append(Token.Circumfix);
            if(value.IsSet(Value.ForbiddenWord)) s.Append(Token.ForbiddenWord);
            if(value.IsSet(Value.KeepCase)) s.Append(Token.KeepCase);
            if(value.IsSet(Value.NeedAffix)) s.Append(Token.NeedAffix);
            if(value.IsSet(Value.Substandard)) s.Append(Token.Substandard);

            return s.ToString();
        }

        public static Value ToValue(char token)
        {
            var value = Value.None;

            switch(token)
            {
                case Token.Circumfix:
                    value |= Value.Circumfix;
                    break;

                case Token.ForbiddenWord:
                    value |= Value.ForbiddenWord;
                    break;

                case Token.KeepCase:
                    value |= Value.KeepCase;
                    break;

                case Token.NeedAffix:
                    value |= Value.NeedAffix;
                    break;

                case Token.Substandard:
                    value |= Value.Substandard;
                    break;

                default:
                    DebugHelper.Warning($"Token unknown: {token}");
                    break;
            }

            return value;
        }

        /// <summary>
        /// Converts a string of tokens to a <see cref="Value"/>.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <returns>Value.</returns>
        public static Value ToValue(string tokens)
        {
            var value = Value.None;

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

        #endregion Methods
    }
}