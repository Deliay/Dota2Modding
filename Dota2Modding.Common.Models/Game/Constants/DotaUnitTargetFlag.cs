using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Models.Game.Constants
{
    public enum DotaUnitTargetFlag
    {
        DOTA_UNIT_TARGET_FLAG_NONE,
        DOTA_UNIT_TARGET_FLAG_DEAD,
        DOTA_UNIT_TARGET_FLAG_MELEE_ONLY,
        DOTA_UNIT_TARGET_FLAG_RANGED_ONLY,
        /// <summary>
        /// Units with mana, without "StatusMana" "0" in the npc_units file.
        /// </summary>
        DOTA_UNIT_TARGET_FLAG_MANA_ONLY,
        /// <summary>
        /// Units with Disable Help on.
        /// Not sure how to make a DataDriven ability use it?
        /// </summary>
        DOTA_UNIT_TARGET_FLAG_CHECK_DISABLE_HELP,
        /// <summary>
        /// Ignores invisible units (with MODIFIER_STATE_INVISIBLE.)
        /// </summary>
        DOTA_UNIT_TARGET_FLAG_NO_INVIS,
        /// <summary>
        /// Targets ENEMY units with MODIFIER_STATE_MAGIC_IMMUNE.
        /// Examples: Ensnare, Culling Blade, Primal Roar...
        /// </summary>
        DOTA_UNIT_TARGET_FLAG_MAGIC_IMMUNE_ENEMIES,
        /// <summary>
        /// Ignores FRIENDLY units with MODIFIER_STATE_MAGIC_IMMUNE.
        /// Example: Bane's Nightmare.
        /// </summary>
        DOTA_UNIT_TARGET_FLAG_NOT_MAGIC_IMMUNE_ALLIES,
        /// <summary>
        /// Ignores units with MODIFIER_STATE_ATTACK_IMMUNE.
        /// </summary>
        DOTA_UNIT_TARGET_FLAG_NOT_ATTACK_IMMUNE,
        /// <summary>
        /// Breaks when the unit goes into the fog of war.
        /// Examples: Mana Drain, Life Drain.
        /// </summary>
        DOTA_UNIT_TARGET_FLAG_FOW_VISIBLE,
        /// <summary>
        /// 	Units with MODIFIER_STATE_INVULNERABLE.
        ///     Examples: Assassinate, Recall, Boulder Smash...
        /// </summary>
        DOTA_UNIT_TARGET_FLAG_INVULNERABLE,
        /// <summary>
        /// Ignores units with "IsAncient" "1" defined.
        /// Example: Hand of Midas.
        /// </summary>
        DOTA_UNIT_TARGET_FLAG_NOT_ANCIENTS,
        /// <summary>
        /// Ignores units with "ConsideredHero" "1" defined.
        /// Examples: Astral Imprisonment, Disruption, Sunder.
        /// </summary>
        DOTA_UNIT_TARGET_FLAG_NOT_CREEP_HERO,
        /// <summary>
        /// Ignores units with MODIFIER_STATE_DOMINATED.
        /// </summary>
        DOTA_UNIT_TARGET_FLAG_NOT_DOMINATED,
        /// <summary>
        /// Ignores untis with MODIFIER_PROPERTY_IS_ILLUSION.
        /// </summary>
        DOTA_UNIT_TARGET_FLAG_NOT_ILLUSIONS,
        /// <summary>
        /// Ignores units with MODIFIER_STATE_NIGHTMARED.
        /// </summary>
        DOTA_UNIT_TARGET_FLAG_NOT_NIGHTMARED,
        /// <summary>
        /// Ignores units created through the SpawnUnit action.
        /// </summary>
        DOTA_UNIT_TARGET_FLAG_NOT_SUMMONED,
        /// <summary>
        /// Units with MODIFIER_STATE_OUT_OF_GAME.
        /// </summary>
        DOTA_UNIT_TARGET_FLAG_OUT_OF_WORLD,
        /// <summary>
        /// Units controllable by a player, accesible with Lua's IsControllableByAnyPlayer().
        /// </summary>
        DOTA_UNIT_TARGET_FLAG_PLAYER_CONTROLLED,
        /// <summary>
        /// 	Prioritizes units over trees when both are selectable.
        /// </summary>
        DOTA_UNIT_TARGET_FLAG_PREFER_ENEMIES,
    }
}
