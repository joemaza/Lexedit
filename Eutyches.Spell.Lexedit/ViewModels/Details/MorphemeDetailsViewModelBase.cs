//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonServiceLocator;
using Eutyches.Spell.Hunspell;
using Eutyches.Spell.Lexedit.Services.Interfaces;
using Prism.Events;

/// <summary>
/// The Details namespace.
/// </summary>
namespace Eutyches.Spell.Lexedit.ViewModels.Details
{
    /// <summary>
    /// ViewModels based on this class have additional properties for <see
    /// cref="Eutyches.Spell.Hunspell.Affix"/> es, and values for <see
    /// cref="Eutyches.Spell.Hunspell.Requisites.Value"/>, <see
    /// cref="Eutyches.Spell.Hunspell.Suggestion.Value"/> and <see cref="Eutyches.Spell.Hunspell.Compounding.Value"/>
    /// </summary>
    /// <typeparam name="TMorpheme">The type of the t morpheme.</typeparam>
    /// <seealso cref="Eutyches.Spell.Lexedit.ViewModels.Details.DetailsViewModelBase{TMorpheme}"/>
    public abstract class MorphemeViewModelBase<TMorpheme> : DetailsViewModelBase<TMorpheme> where TMorpheme : Morpheme, new()
    {
        #region Fields

        protected static readonly List<string> AffixClipboard = new List<string>();

        protected MorphemeViewModelBase(IEventAggregator eventAggregator, IFileService fileService, IDialogService dialogService, IToolService toolService)
            : base(eventAggregator, fileService, dialogService, toolService)
        {
        }

        #endregion Fields

        #region Constructors

        ///// <summary>
        ///// Initializes a new instance of the <see cref="MorphemeViewModelBase{TMorpheme}"/> class.
        ///// </summary>
        //protected MorphemeViewModelBase() : base()
        //{
        //}

        ///// <summary>
        ///// Initializes a new instance of the <see cref="MorphemeViewModelBase{TMorpheme}"/> class.
        ///// </summary>
        ///// <param name="data">The data.</param>
        ///// <param name="eventAggregator">The event aggregator.</param>
        ///// <param name="fileService">The file service.</param>
        ///// <param name="dialogService">The dialog service.</param>
        //protected MorphemeViewModelBase(
        //    TMorpheme data,
        //    IEventAggregator eventAggregator,
        //    IFileService fileService,
        //    IDialogService dialogService, IToolService toolService)
        //    : base(data, eventAggregator, fileService, dialogService, toolService)
        //{
        //}

        /// <summary>
        /// Gets or sets the affixes.
        /// </summary>
        /// <value>The affixes.</value>
        public List<string> Affixes
        {
            get => Data.Affixes;

            set
            {
                if(Data.Affixes == value)
                    return;

                BeginEdit();
                Data.Affixes = value;
                RaisePropertyChanged(nameof(Affixes));
            }
        }

        public string CircumfixFlag => GeneralOptions.Circumfix?.Value;

        public IEnumerable<string> Clipboard
        {
            get => AffixClipboard.ToList();

            set
            {
                AffixClipboard.Clear();
                AffixClipboard.AddRange(value);

                RaisePropertyChanged(nameof(Clipboard));
            }
        }

        public string CompoundBeginFlag => CompoundingOptions.CompoundBegin?.Value;

        public string CompoundFlagFlag => CompoundingOptions.CompoundFlag?.Value;

        public string CompoundForbidFlagFlag => CompoundingOptions.CompoundForbidFlag?.Value;

        protected CompoundingOptions CompoundingOptions => _fileService.Lexicon.CompoundingOptions;

        /// <summary>
        /// Gets or sets the compounding.
        /// </summary>
        /// <value>The compounding.</value>
        public Compounding.Value CompoundingValues
        {
            get => Data.CompoundingValues;

            set
            {
                if(Data.CompoundingValues == value)
                    return;

                BeginEdit();
                Data.CompoundingValues = value;
                RaisePropertyChanged(nameof(CompoundingValues));
            }
        }

        public string CompoundLastFlag => CompoundingOptions.CompoundLast?.Value;

        public string CompoundMiddleFlag => CompoundingOptions.CompoundMiddle?.Value;

        public string CompoundPermitFlagFlag => CompoundingOptions.CompoundPermitFlag?.Value;

        public string CompoundRootFlag => CompoundingOptions.CompoundRoot?.Value;

        public string ForbiddenWordFlag => GeneralOptions.ForbiddenWord?.Value;

        public string ForceUCaseFlag => CompoundingOptions.ForceUCase?.Value;

        /// <summary>
        /// Gets or sets the form.
        /// </summary>
        /// <value>The form.</value>
        public string Form
        {
            get => Data.Form;

            set
            {
                if(Data.Form == value)
                    return;

                ValidateProperty(nameof(Form), value);

                BeginEdit();
                Data.Form = value.Trim();

                RaisePropertyChanged(nameof(Form));
            }
        }

        protected GeneralOptions GeneralOptions => _fileService.Lexicon.GeneralOptions;

        /// <summary>
        /// Gets a value indicating whether this instance is circumfix defined.
        /// </summary>
        /// <value><c>true</c> if this instance is circumfix defined; otherwise, <c>false</c>.</value>
        public bool IsCircumfixDefined => !string.IsNullOrWhiteSpace(CircumfixFlag);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is circumfix. (Rules only)
        /// </summary>
        /// <value><c>true</c> if this instance is circumfix; otherwise, <c>false</c>.</value>
        public bool IsCircumfixSet
        {
            get => Data.RequisiteValues.IsSet(Requisites.Value.Circumfix);
            set => SetRequisiteValues(nameof(IsCircumfixSet), Requisites.Value.Circumfix, value);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is compound begin defined.
        /// </summary>
        /// <value><c>true</c> if this instance is compound begin defined; otherwise, <c>false</c>.</value>
        public bool IsCompoundBeginDefined => !string.IsNullOrWhiteSpace(CompoundBeginFlag);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is compound begin set.
        /// </summary>
        /// <value><c>true</c> if this instance is compound begin set; otherwise, <c>false</c>.</value>
        public bool IsCompoundBeginSet
        {
            get => Data.CompoundingValues.IsSet(Compounding.Value.CompoundBegin);
            set => SetCompoundingValues(nameof(IsCompoundBeginSet), Compounding.Value.CompoundBegin, value);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is compound flag defined.
        /// </summary>
        /// <value><c>true</c> if this instance is compound flag defined; otherwise, <c>false</c>.</value>
        public bool IsCompoundFlagDefined => !string.IsNullOrWhiteSpace(CompoundFlagFlag);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is compound flag set.
        /// </summary>
        /// <value><c>true</c> if this instance is compound flag set; otherwise, <c>false</c>.</value>
        public bool IsCompoundFlagSet
        {
            get => Data.CompoundingValues.IsSet(Compounding.Value.CompoundFlag);
            set => SetCompoundingValues(nameof(IsCompoundFlagSet), Compounding.Value.CompoundFlag, value);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is compound forbid flag defined.
        /// </summary>
        /// <value><c>true</c> if this instance is compound forbid flag defined; otherwise, <c>false</c>.</value>
        public bool IsCompoundForbidFlagDefined => !string.IsNullOrWhiteSpace(CompoundForbidFlagFlag);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is compound forbid flag set.
        /// </summary>
        /// <value><c>true</c> if this instance is compound forbid flag set; otherwise, <c>false</c>.</value>
        public bool IsCompoundForbidFlagSet
        {
            get => Data.CompoundingValues.IsSet(Compounding.Value.CompoundForbidFlag);
            set => SetCompoundingValues(nameof(IsCompoundForbidFlagSet), Compounding.Value.CompoundForbidFlag, value);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is compound last defined.
        /// </summary>
        /// <value><c>true</c> if this instance is compound last defined; otherwise, <c>false</c>.</value>
        public bool IsCompoundLastDefined => !string.IsNullOrWhiteSpace(CompoundLastFlag);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is compound last set.
        /// </summary>
        /// <value><c>true</c> if this instance is compound last set; otherwise, <c>false</c>.</value>
        public bool IsCompoundLastSet
        {
            get => Data.CompoundingValues.IsSet(Compounding.Value.CompoundLast);
            set => SetCompoundingValues(nameof(IsCompoundLastSet), Compounding.Value.CompoundLast, value);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is compound middle defined.
        /// </summary>
        /// <value><c>true</c> if this instance is compound middle defined; otherwise, <c>false</c>.</value>
        public bool IsCompoundMiddleDefined => !string.IsNullOrWhiteSpace(CompoundMiddleFlag);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is compound middle set.
        /// </summary>
        /// <value><c>true</c> if this instance is compound middle set; otherwise, <c>false</c>.</value>
        public bool IsCompoundMiddleSet
        {
            get => Data.CompoundingValues.IsSet(Compounding.Value.CompoundMiddle);
            set => SetCompoundingValues(nameof(IsCompoundMiddleSet), Compounding.Value.CompoundMiddle, value);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is compound permit flag defined.
        /// </summary>
        /// <value><c>true</c> if this instance is compound permit flag defined; otherwise, <c>false</c>.</value>
        public bool IsCompoundPermitFlagDefined => !string.IsNullOrWhiteSpace(CompoundPermitFlagFlag);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is compound permit flag set.
        /// </summary>
        /// <value><c>true</c> if this instance is compound permit flag set; otherwise, <c>false</c>.</value>
        public bool IsCompoundPermitFlagSet
        {
            get => Data.CompoundingValues.IsSet(Compounding.Value.CompoundPermitFlag);
            set => SetCompoundingValues(nameof(IsCompoundPermitFlagSet), Compounding.Value.CompoundPermitFlag, value);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is compound root defined.
        /// </summary>
        /// <value><c>true</c> if this instance is compound root defined; otherwise, <c>false</c>.</value>
        public bool IsCompoundRootDefined => !string.IsNullOrWhiteSpace(CompoundRootFlag);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is compound root set.
        /// </summary>
        /// <value><c>true</c> if this instance is compound root set; otherwise, <c>false</c>.</value>
        public bool IsCompoundRootSet
        {
            get => Data.CompoundingValues.IsSet(Compounding.Value.CompoundRoot);
            set => SetCompoundingValues(nameof(IsCompoundRootSet), Compounding.Value.CompoundRoot, value);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is forbidden word defined.
        /// </summary>
        /// <value><c>true</c> if this instance is forbidden word defined; otherwise, <c>false</c>.</value>
        public bool IsForbiddenWordDefined => !string.IsNullOrWhiteSpace(ForbiddenWordFlag);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is forbidden.
        /// </summary>
        /// <value><c>true</c> if this instance is forbidden; otherwise, <c>false</c>.</value>
        public bool IsForbiddenWordSet
        {
            get => Data.RequisiteValues.IsSet(Requisites.Value.ForbiddenWord);
            set => SetRequisiteValues(nameof(IsForbiddenWordSet), Requisites.Value.ForbiddenWord, value);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is force u case defined.
        /// </summary>
        /// <value><c>true</c> if this instance is force u case defined; otherwise, <c>false</c>.</value>
        public bool IsForceUCaseDefined => !string.IsNullOrWhiteSpace(ForceUCaseFlag);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is force u case set.
        /// </summary>
        /// <value><c>true</c> if this instance is force u case set; otherwise, <c>false</c>.</value>
        public bool IsForceUCaseSet
        {
            get => Data.CompoundingValues.IsSet(Compounding.Value.ForceUCase);
            set => SetCompoundingValues(nameof(IsForceUCaseSet), Compounding.Value.ForceUCase, value);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is keep case defined.
        /// </summary>
        /// <value><c>true</c> if this instance is keep case defined; otherwise, <c>false</c>.</value>
        public bool IsKeepCaseDefined => !string.IsNullOrWhiteSpace(KeepCaseFlag);

        /// <summary>
        /// Gets or sets a value indicating whether case should be kept.
        /// </summary>
        /// <value><c>true</c> if case need to be kept; otherwise, <c>false</c>.</value>
        public bool IsKeepCaseSet
        {
            get => Data.RequisiteValues.IsSet(Requisites.Value.KeepCase);
            set => SetRequisiteValues(nameof(IsKeepCaseSet), Requisites.Value.KeepCase, value);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is need affix defined.
        /// </summary>
        /// <value><c>true</c> if this instance is need affix defined; otherwise, <c>false</c>.</value>
        public bool IsNeedAffixDefined => !string.IsNullOrWhiteSpace(NeedAffixFlag);

        /// <summary>
        /// Gets or sets a value indicating whether another affix is needed.
        /// </summary>
        /// <value><c>true</c> if further affixation is needed; otherwise, <c>false</c>.</value>
        public bool IsNeedAffixSet
        {
            get => Data.RequisiteValues.IsSet(Requisites.Value.NeedAffix);
            set => SetRequisiteValues(nameof(IsNeedAffixSet), Requisites.Value.NeedAffix, value);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is no suggest defined.
        /// </summary>
        /// <value><c>true</c> if this instance is no suggest defined; otherwise, <c>false</c>.</value>
        public bool IsNoSuggestDefined => !string.IsNullOrWhiteSpace(NoSuggestFlag);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is not suggestable.
        /// </summary>
        /// <value><c>true</c> if this instance is not suggestable; otherwise, <c>false</c>.</value>
        public bool IsNoSuggestSet
        {
            get => Data.SuggestionValues.IsSet(Suggestion.Value.NoSuggest);
            set => SetSuggestionValues(nameof(IsNoSuggestSet), Suggestion.Value.NoSuggest, value);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is only in compound defined.
        /// </summary>
        /// <value><c>true</c> if this instance is only in compound defined; otherwise, <c>false</c>.</value>
        public bool IsOnlyInCompoundDefined => !string.IsNullOrWhiteSpace(OnlyInCompoundFlag);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is only in compound set.
        /// </summary>
        /// <value><c>true</c> if this instance is only in compound set; otherwise, <c>false</c>.</value>
        public bool IsOnlyInCompoundSet
        {
            get => Data.CompoundingValues.IsSet(Compounding.Value.OnlyInCompound);
            set => SetCompoundingValues(nameof(IsOnlyInCompoundSet), Compounding.Value.OnlyInCompound, value);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is substandard defined.
        /// </summary>
        /// <value><c>true</c> if this instance is substandard defined; otherwise, <c>false</c>.</value>
        public bool IsSubstandardDefined => !string.IsNullOrWhiteSpace(SubstandardFlag);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is substandard.
        /// </summary>
        /// <value><c>true</c> if this instance is substandard; otherwise, <c>false</c>.</value>
        public bool IsSubstandardSet
        {
            get => Data.RequisiteValues.IsSet(Requisites.Value.Substandard);
            set => SetRequisiteValues(nameof(IsSubstandardSet), Requisites.Value.Substandard, value);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is syllable number defined.
        /// </summary>
        /// <value><c>true</c> if this instance is syllable number defined; otherwise, <c>false</c>.</value>
        public bool IsSyllableNumDefined => !string.IsNullOrWhiteSpace(SyllableNumFlag);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is syllable number set.
        /// </summary>
        /// <value><c>true</c> if this instance is syllable number set; otherwise, <c>false</c>.</value>
        public bool IsSyllableNumSet
        {
            get => Data.CompoundingValues.IsSet(Compounding.Value.SyllableNum);
            set => SetCompoundingValues(nameof(IsSyllableNumSet), Compounding.Value.SyllableNum, value);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is warn defined.
        /// </summary>
        /// <value><c>true</c> if this instance is warn defined; otherwise, <c>false</c>.</value>
        public bool IsWarnDefined => !string.IsNullOrWhiteSpace(WarnFlag);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is marked warning.
        /// </summary>
        /// <value><c>true</c> if this instance is marked warning; otherwise, <c>false</c>.</value>
        public bool IsWarnSet
        {
            get => Data.SuggestionValues.IsSet(Suggestion.Value.Warn);
            set => SetSuggestionValues(nameof(IsWarnSet), Suggestion.Value.Warn, value);
        }

        public string KeepCaseFlag => GeneralOptions.KeepCase?.Value;

        public string NeedAffixFlag => GeneralOptions.NeedAffix?.Value;

        public string NoSuggestFlag => SuggestionOptions.NoSuggest?.Value;

        public string OnlyInCompoundFlag => CompoundingOptions.OnlyInCompound?.Value;

        /// <summary>
        /// Gets or sets the requisites.
        /// </summary>
        /// <value>The requisites.</value>
        public Requisites.Value RequisiteValues
        {
            get => Data.RequisiteValues;

            set
            {
                if(Data.RequisiteValues == value)
                    return;

                BeginEdit();
                Data.RequisiteValues = value;
                RaisePropertyChanged(nameof(RequisiteValues));
            }
        }

        public string SubstandardFlag => GeneralOptions.Substandard?.Value;

        protected SuggestionOptions SuggestionOptions => _fileService.Lexicon.SuggestionOptions;

        /// <summary>
        /// Gets or sets the suggestion.
        /// </summary>
        /// <value>The suggestion.</value>
        public Suggestion.Value SuggestionValues
        {
            get => Data.SuggestionValues;

            set
            {
                if(Data.SuggestionValues == value)
                    return;

                BeginEdit();
                Data.SuggestionValues = value;
                RaisePropertyChanged(nameof(SuggestionValues));
            }
        }

        public string SyllableNumFlag => CompoundingOptions.SyllableNum?.Value;

        public string WarnFlag => SuggestionOptions.Warn?.Value;

        /// <summary>
        /// Performs a check on the flags in type <typeparamref name="T"/> if <paramref name="bit"/>
        /// were set or cleared. Returns a value specifying if the value would change. If it would,
        /// the new value is return in <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bit">The bit to target.</param>
        /// <param name="value">The value.</param>
        /// <param name="set">if set to <c>true</c> set the <paramref name="bit"/>.</param>
        /// <param name="result">
        /// The result if the flag were to change. If changed, this value must be cast using the
        /// appropriate enum to be used; otherwise, it would have the same integer value of <paramref name="value"/>
        /// </param>
        /// <returns><c>true</c> if the flag value would change, <c>false</c> otherwise.</returns>
        protected bool ChangeFlag<T>(T bit, T value, bool set, out int result) where T : struct, IConvertible
        {
            // Convert the (enum) values to Int32's and copy the value to result
            int current = value.ToInt32(CultureInfo.InvariantCulture);
            result = current;

            // Set the bit if true; clear, the bit if false
            if(set)
            {
                result |= bit.ToInt32(CultureInfo.InvariantCulture);
            }
            else
            {
                result &= ~(bit.ToInt32(CultureInfo.InvariantCulture));
            }

            // Compare the integer values;
            return result != current;
        }

        /// <summary>
        /// Sets the compounding values.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="set">if set to <c>true</c> [set].</param>
        protected void SetCompoundingValues(string property, Compounding.Value value, bool set)
        {
            if(ChangeFlag(value, Data.CompoundingValues, set, out int newValue))
            {
                CompoundingValues = (Compounding.Value) newValue;
                RaisePropertyChanged(property);
            }
        }

        /// <summary>
        /// Sets the requisite values.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="set">if set to <c>true</c> [set].</param>
        protected void SetRequisiteValues(string property, Requisites.Value value, bool set)
        {
            if(ChangeFlag(value, Data.RequisiteValues, set, out int newValue))
            {
                RequisiteValues = (Requisites.Value) newValue;
                RaisePropertyChanged(property);
            }
        }

        /// <summary>
        /// Sets the suggestion values.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="set">if set to <c>true</c> [set].</param>
        protected void SetSuggestionValues(string property, Suggestion.Value value, bool set)
        {
            if(ChangeFlag(value, Data.SuggestionValues, set, out int newValue))
            {
                SuggestionValues = (Suggestion.Value) newValue;
                RaisePropertyChanged(property);
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        public string Comments
        {
            get => Data.Comments;

            set
            {
                if(Data.Comments == value) return;

                BeginEdit();
                Data.Comments = value;

                RaisePropertyChanged(nameof(Comments));
            }
        }

        #endregion Properties
    }
}