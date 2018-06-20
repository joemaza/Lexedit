# Lexedit
Lexedit is an application to assist creators and maintainers of  Hunspell dictionaries.

# Features

## Current or in Development
* Create/Export Hunspell files (*.aff and *.dic), together or separately
 
  The main function. Produce Hunspell dictionary files for use in Web browser extensions or plug-ins or as-is. Importing these files to create a lexicon is planned.

* Import/Export stems and affixes in various formats: JSON, compressed file, (tagged) plain-text file

  Import or export stems and/or affixes for back up or sharing with a colleague.
  
* Morpheme Meta Data

  Both stems and affixes have additional information, such as meaning or lexical categories (parts of speech), to make associations easier to understand for the user. This information can be included in the Hunspell files.

* Stem Relations

  Make relations between stems, for example, specify roots, irregular formations, derived formations and other formations that are not easily determined or specified by affixation.

* *Ad Hoc* Validation

   Test stems and their associated affixes by entering a list of words, both valid and invalid.

* Editing of other linguistic information found in the *.aff file (affix file), i.e.:
  * Dictionary Information
  * General Options
  * Suggestion Options
  * Compounding Options
  * Conversion Options
  
## Planned
* Parse *.aff and *.dic files to create a lexicon
* Creation of Web browser or productivity suite extensions, such as Firefox spelling dictionaries or LibreOffice extensions

# Background
Years ago I was inspired to create a Hunspell dictionary for Iloko. As I started creating the dictionary, maintaining it became cumbersome. I looked for solutions. After finding some and trying them, I decided to create a tool that better suited my needs and my style of work. The goal was not only for my own use but to also make it more *accessible* for others who would want to creat a spell-check dictionary for their language.

The version of the code is the second iteration. I've decided to release this version to GitHub with the hopes of visibility and garnering interest as I reimplement my vision based on what I had learned from the previous iteration.

The principal idea is behind Lexedit is the lexicon.

*Lexicon*, in its linguistic sense, is... 
> *"The complete set of meaningful units in a language."* 
> - [Oxford English Dictionaries](https://en.oxforddictionaries.com/definition/lexicon)

The main focus, then, is the two types of morphemes, units of meaning, in language. Stems are units that can stand alone as a *word* (i.e., a *root*) or is the common part of a word to which inflections are applied. Affixes, on the other hand, are those untis of meaning that cannot stand alone, but must be used with stems to create a valid word. Lexedit facilitates creating stems and associating them with the proper affixes.
