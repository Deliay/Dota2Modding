using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Models.Game.Constants
{
    public enum DotaDamageTypes
    {
        DAMAGE_TYPE_NONE = 0,
        DAMAGE_TYPE_PHYSICAL = 1,
        DAMAGE_TYPE_MAGICAL = 2,
        DAMAGE_TYPE_PURE = 4,
        DAMAGE_TYPE_ALL = 7,
        DAMAGE_TYPE_HP_REMOVAL = 8,
        DAMAGE_TYPE_ABILITY_DEFINED = 22,
    }
}
