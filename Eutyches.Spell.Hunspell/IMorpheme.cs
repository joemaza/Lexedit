using System.Collections.Generic;

namespace Eutyches.Spell.Hunspell
{
    public interface IMorpheme
    {
        List<string> Affixes { get; set; }
        string Comments { get; set; }
        Compounding.Value CompoundingValues { get; set; }
        string Form { get; set; }
        Requisites.Value RequisiteValues { get; set; }
        Suggestion.Value SuggestionValues { get; set; }
    }
}