using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Models.Game.Constants
{
    public enum DotaUnitTarget
    {
        /// <summary>
        /// Default
        /// </summary>
        DOTA_UNIT_TARGET_NONE,
        /// <summary>
        /// 	Everything, including hidden entities.
        /// </summary>
        DOTA_UNIT_TARGET_ALL,
        /// <summary>
        /// 	npc_dota_hero Heroes.
        ///     DOTA_NPC_UNIT_RELATIONSHIP_TYPE_HERO
        /// </summary>
        DOTA_UNIT_TARGET_HERO,
        /// <summary>
        /// 	Basic units, including summons.
        /// </summary>
        DOTA_UNIT_TARGET_BASIC,
        /// <summary>
        /// npc_dota_creep_siege
        ///  DOTA_NPC_UNIT_RELATIONSHIP_TYPE_SIEGE
        /// </summary>
        DOTA_UNIT_TARGET_MECHANICAL,
        DOTA_UNIT_TARGET_BUILDING,
        DOTA_UNIT_TARGET_TREE,
        /// <summary>
        /// npc_dota_creature, npc_dota_creep
        /// Same as BASIC, but might not include things like some summons.
        /// Examples: Death Pact, Devour.
        /// </summary>
        DOTA_UNIT_TARGET_CREEP,
        /// <summary>
        /// npc_dota_courier, npc_dota_flying_courier  DOTA_NPC_UNIT_RELATIONSHIP_TYPE_COURIER
        /// </summary>
        DOTA_UNIT_TARGET_COURIER,
        /// <summary>
        /// Everything not included in the previous types.
        /// </summary>
        DOTA_UNIT_TARGET_OTHER,
        /// <summary>
        /// Not exposed? Examples: Replicate, Sunder, Demonic Conversion, Tether, Infest...
        /// </summary>
        DOTA_UNIT_TARGET_CUSTOM,
    }
}
