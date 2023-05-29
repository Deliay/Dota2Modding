using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Models.Game.Constants
{
    public enum DotaSpellImmunityType
    {
        SPELL_IMMUNITY_NONE = 0,
        SPELL_IMMUNITY_ALLIES_YES = 1,
        SPELL_IMMUNITY_ALLIES_NO = 2,
        SPELL_IMMUNITY_ENEMIES_YES = 3,
        SPELL_IMMUNITY_ENEMIES_NO = 4,
        SPELL_IMMUNITY_ALLIES_YES_ENEMIES_NO = 5,
    }
}
