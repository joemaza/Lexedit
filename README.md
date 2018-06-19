# Lexedit
A Windows application for maintaining Hunspell spell-check dictionaries

# Features
The principal idea is behind Lexedit is the lexicon.

*Lexicon*, in its linguistic sense, is... 
> *"The complete set of meaningful units in a language."* 
> - [Oxford English Dictionaries](https://en.oxforddictionaries.com/definition/lexicon)

The current state of the code reflects my focus on properly associating stems (*bound and unbound morphemes*) with affixes (*bound morphemes*), which is related to one of my other projects, the [spellcheck](https://github.com/joemaza/spellcheck) project for Iloko, Tagalog and Cebuano. The foundations for other features are therein and will appear in future commits.

# Background
Years ago I was inspired to create a Hunspell dictionary for Iloko. As I started creating the dictionary, maintaining it became cumbersome. I looked for solutions and after trying some, I decided to create my own, not only for my use but to also make it more *accesible* for others who would like to assume the role of creating a spell-check for their language.

The version of the code is the second iteration. I've decided to release this version to GitHub with the hopes of visibility and garnering interest.

# Planned
* Parse *.aff and *.dic files to create a lexicon
* Editing of the various sections of the *.aff file, i.e.:
  * General Information
  * Suggestions
  * Compounding
  * Conversions
