//=============================================================================
// 
//=============================================================================

namespace Eutyches.Spell.Hunspell
{
    public enum AffixType
    {
        Unknown,
        Prefix,
        Suffix,
        Infix,
    }

    public static class AffixTypeExtension
    {
        #region Methods

        public static string ToFlag(this AffixType type)
        {
            string flag;

            switch(type)
            {
                case AffixType.Infix:
                case AffixType.Prefix:
                    flag = "PFX";
                    break;

                case AffixType.Suffix:
                    flag = "SFX";
                    break;

                default:
                    flag = "!*!";
                    break;
            }

            return flag;
        }

        #endregion Methods
    }
}