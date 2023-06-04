using Dota2Modding.Common.Models.Game.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.Game.Abilities
{
    public class DotaAbility : BasicObject
    {
        public DotaAbility(string name, KVValue value) : base(name, value)
        {
        }

        public string BaseClass
        {
            get => GetString("BaseClass");
            set => base["BaseClass"] = value;
        }

        public string ScriptFile
        {
            get => GetString("ScriptFile");
            set => base["ScriptFile"] = value;
        }

        public string AbilityTextureName
        {
            get => GetString("AbilityTextureName");
            set => base["AbilityTextureName"] = value;
        }

        public int ID
        {
            get => GetInt32("ID") ?? 4;
            set => base["ID"] = value;
        }

        public int MaxLevel
        {
            get => GetInt32("MaxLevel") ?? 4;
            set => base["MaxLevel"] = value;
        }

        public DotaGameActivity? AbilityCastAnimation
        {
            get => ToFlag<DotaGameActivity>("AbilityCastAnimation");
            set => base["AbilityCastAnimation"] = FromFlag(value.Value);
        }

        public float AbilityCastRange
        {
            get => GetSingle("MaxLevel") ?? 0;
            set => base["MaxLevel"] = value.ToString();
        }

        public DotaDamageTypes? AbilityUnitDamageType
        {
            get => ToFlag<DotaDamageTypes>("AbilityUnitDamageType");
            set => base["AbilityUnitDamageType"] = FromFlag(value.Value);
        }

        public DotaSpellImmunityType? SpellImmunityType
        {
            get => ToFlag<DotaSpellImmunityType>("SpellImmunityType");
            set => base["SpellImmunityType"] = FromFlag(value.Value);
        }

        public int AbilityCastPoint
        {
            get => GetInt32("AbilityCastPoint") ?? 0;
            set => base["AbilityCastPoint"] = value;
        }

        public string AbilityManaCost
        {
            get => GetString("AbilityManaCost");
            set => base["AbilityManaCost"] = value;
        }

        public int FightRecapLevel
        {
            get => GetInt32("FightRecapLevel") ?? 0;
            set => base["FightRecapLevel"] = value;
        }

        public string AbilityCooldown
        {
            get => GetString("AbilityCooldown");
            set => base["AbilityCooldown"] = value;
        }

        public DotaAbilityType? AbilityType
        {
            get => ToFlag<DotaAbilityType>("AbilityType");
            set => base["AbilityType"] = FromFlag(value.Value);
        }
        public int RequiredLevel
        {
            get => GetInt32("RequiredLevel") ?? 0;
            set => base["RequiredLevel"] = value;
        }
        public int LevelsBetweenUpgrades
        {
            get => GetInt32("LevelsBetweenUpgrades") ?? 0;
            set => base["LevelsBetweenUpgrades"] = value;
        }
        /// <summary>
        /// Cast While Hidden
        /// </summary>
        public bool IsCastableWhileHidden
        {
            get => GetBoolean("IsCastableWhileHidden") ?? true;
            set => base["IsCastableWhileHidden"] = value ? 1 : 0;
        }
        public DotaUnitTargetTeam? AbilityUnitTargetTeam
        {
            get => ToFlag<DotaUnitTargetTeam>("AbilityUnitTargetTeam");
            set => base["AbilityUnitTargetTeam"] = FromFlag(value.Value);
        }
        public IReadOnlySet<DotaAbilityBehavior> AbilityBehavior
        {
            get => FlagsOf<DotaAbilityBehavior>("AbilityBehavior");
            set => base["AbilityBehavior"] = ToFlag(value);
        }

        public IReadOnlySet<DotaUnitTarget> AbilityUnitTargetType
        {
            get => FlagsOf<DotaUnitTarget>("AbilityUnitTargetType");
            set => base["AbilityUnitTargetType"] = ToFlag(value);
        }

    }
}
