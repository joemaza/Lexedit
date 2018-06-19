//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eutyches.Spell.Hunspell
{
    public static class Compounding
    {
        #region Enums

        [Flags]
        public enum Value
        {
            Default = 0b0,
            CompoundFlag = 0b0000_01,

            ForceUCase = 0b0001_00,
            CompoundRoot = 0b0010_00,
            OnlyInCompound = 0b0100_00,
            SyllableNum = 0b1000_00,

            CompoundBegin = 0b0001_0000_00,
            CompoundMiddle = 0b0010_0000_00,
            CompoundLast = 0b0100_0000_00,

            CompoundPermitFlag = 0b0001_0000_0000_00,
            CompoundForbidFlag = 0b0010_0000_0000_00,
        }

        #endregion Enums

        #region Classes

        public static class Flag
        {
            #region Fields

            public const string CompoundBegin = "@<";
            public const string CompoundFlag = "@@";
            public const string CompoundForbidFlag = "@-";
            public const string CompoundLast = "@>";
            public const string CompoundMiddle = "@|";
            public const string CompoundPermitFlag = "@+";
            public const string CompoundRoot = "@%";
            public const string ForceUCase = "@^";
            public const string OnlyInCompound = "@!";
            public const string SyllableNum = "@$";

            #endregion Fields
        }

        public static class Token
        {
            #region Fields

            public const char CompoundBegin = 'B';
            public const char CompoundFlag = 'C';
            public const char CompoundForbidFlag = '-';
            public const char CompoundLast = 'L';
            public const char CompoundMiddle = 'M';
            public const char CompoundPermitFlag = '+';
            public const char CompoundRoot = 'R';
            public const char ForceUCase = 'U';
            public const char OnlyInCompound = '!';
            public const char SyllableNum = '#';

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
            var op = lexicon.CompoundingOptions;

            var flags = new List<string>();

            foreach(var enumValue in Enum.GetValues(typeof(Value)))
            {
                var ev = (Value) enumValue;

                if((value & ev) == ev)
                {
                    switch(ev)
                    {
                        case Value.CompoundBegin:
                            flags.Add(string.IsNullOrWhiteSpace(op?.CompoundBegin?.Value)
                                ? Flag.CompoundBegin : op?.CompoundBegin?.Value);
                            break;

                        case Value.CompoundFlag:
                            flags.Add(string.IsNullOrWhiteSpace(op?.CompoundFlag?.Value)
                                ? Flag.CompoundFlag : op?.CompoundFlag?.Value);
                            break;

                        case Value.CompoundForbidFlag:
                            flags.Add(string.IsNullOrWhiteSpace(op?.CompoundForbidFlag?.Value)
                                ? Flag.CompoundForbidFlag : op?.CompoundForbidFlag?.Value);
                            break;

                        case Value.CompoundLast:
                            flags.Add(string.IsNullOrWhiteSpace(op?.CompoundLast?.Value)
                                ? Flag.CompoundLast : op?.CompoundLast?.Value);
                            break;

                        case Value.CompoundMiddle:
                            flags.Add(string.IsNullOrWhiteSpace(op?.CompoundMiddle?.Value)
                                ? Flag.CompoundMiddle : op?.CompoundMiddle?.Value);
                            break;

                        case Value.CompoundPermitFlag:
                            flags.Add(string.IsNullOrWhiteSpace(op?.CompoundPermitFlag?.Value)
                                ? Flag.CompoundPermitFlag : op?.CompoundPermitFlag?.Value);
                            break;

                        case Value.CompoundRoot:
                            flags.Add(string.IsNullOrWhiteSpace(op?.CompoundRoot?.Value)
                                ? Flag.CompoundRoot : op?.CompoundRoot?.Value);
                            break;

                        case Value.ForceUCase:
                            flags.Add(string.IsNullOrWhiteSpace(op?.ForceUCase?.Value)
                                ? Flag.ForceUCase : op?.ForceUCase?.Value);
                            break;

                        case Value.OnlyInCompound:
                            flags.Add(string.IsNullOrWhiteSpace(op?.OnlyInCompound?.Value)
                                ? Flag.OnlyInCompound : op?.OnlyInCompound?.Value);
                            break;

                        case Value.SyllableNum:
                            flags.Add(string.IsNullOrWhiteSpace(op?.SyllableNum?.Value)
                                ? Flag.SyllableNum : op?.SyllableNum?.Value);
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
            var s = new StringBuilder();

            if(value.IsSet(Value.CompoundBegin)) s.Append(Token.CompoundBegin);
            if(value.IsSet(Value.CompoundFlag)) s.Append(Token.CompoundFlag);
            if(value.IsSet(Value.CompoundForbidFlag)) s.Append(Token.CompoundForbidFlag);
            if(value.IsSet(Value.CompoundLast)) s.Append(Token.CompoundLast);
            if(value.IsSet(Value.CompoundMiddle)) s.Append(Token.CompoundMiddle);
            if(value.IsSet(Value.CompoundPermitFlag)) s.Append(Token.CompoundPermitFlag);
            if(value.IsSet(Value.CompoundRoot)) s.Append(Token.CompoundRoot);
            if(value.IsSet(Value.ForceUCase)) s.Append(Token.ForceUCase);
            if(value.IsSet(Value.OnlyInCompound)) s.Append(Token.OnlyInCompound);
            if(value.IsSet(Value.SyllableNum)) s.Append(Token.SyllableNum);

            return s.ToString();
        }

        public static Value ToValue(char token)
        {
            var value = Value.Default;

            switch(token)
            {
                case Token.CompoundBegin:
                    value |= Value.CompoundBegin;
                    break;

                case Token.CompoundFlag:
                    value |= Value.CompoundFlag;
                    break;

                case Token.CompoundForbidFlag:
                    value |= Value.CompoundForbidFlag;
                    break;

                case Token.CompoundLast:
                    value |= Value.CompoundLast;
                    break;

                case Token.CompoundMiddle:
                    value |= Value.CompoundMiddle;
                    break;

                case Token.CompoundPermitFlag:
                    value |= Value.CompoundPermitFlag;
                    break;

                case Token.CompoundRoot:
                    value |= Value.CompoundRoot;
                    break;

                case Token.ForceUCase:
                    value |= Value.ForceUCase;
                    break;

                case Token.OnlyInCompound:
                    value |= Value.OnlyInCompound;
                    break;

                case Token.SyllableNum:
                    value |= Value.SyllableNum;
                    break;

                default:

                    DebugHelper.Warning($"Token unknown: {token}");
                    break;
            }

            return value;
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

        #endregion Methods
    }
}