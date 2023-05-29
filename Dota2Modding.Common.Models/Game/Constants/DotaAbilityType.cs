using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Models.Game.Constants
{
    public enum DotaAbilityType
    {
        /// <summary>
        /// 	Normal ability, learnable at level 1 and upgradeable every 2 levels.
        /// </summary>
        DOTA_ABILITY_TYPE_BASIC,
        /// <summary>
        /// 5 levels between upgrades, and requires level 6 to spend the first point on it.
        /// Also tags the ability as ultimate for the HUD.
        /// </summary>
        DOTA_ABILITY_TYPE_ULTIMATE,
        /// <summary>
        /// Used for attribute_bonus.
        /// </summary>
        DOTA_ABILITY_TYPE_ATTRIBUTES,
        DOTA_ABILITY_TYPE_HIDDEN,

    }
}
