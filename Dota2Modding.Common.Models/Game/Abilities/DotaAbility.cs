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
            get => base["BaseClass"].ToString(CultureInfo.CurrentCulture);
            set => base["BaseClass"] = value;
        }

        public string ScriptFile
        {
            get => base["ScriptFile"].ToString(CultureInfo.CurrentCulture);
            set => base["ScriptFile"] = value;
        }

        public string AbilityTextureName
        {
            get => base["AbilityTextureName"].ToString(CultureInfo.CurrentCulture);
            set => base["AbilityTextureName"] = value;
        }

        public int MaxLevel
        {
            get => base["MaxLevel"].ToInt32(CultureInfo.CurrentCulture);
            set => base["MaxLevel"] = value;
        }

        public DotaGameActivity? AbilityCastAnimation
        {
            get => ToFlag<DotaGameActivity>("AbilityCastAnimation");
            set => base["AbilityCastAnimation"] = FromFlag(value.Value);
        }

        public float AbilityCastRange
        {
            get => base["MaxLevel"].ToSingle(CultureInfo.CurrentCulture);
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
            get => base["AbilityCastPoint"].ToInt32(CultureInfo.CurrentCulture);
            set => base["AbilityCastPoint"] = value;
        }

        public string AbilityManaCost
        {
            get => base["AbilityManaCost"].ToString(CultureInfo.CurrentCulture);
            set => base["AbilityManaCost"] = value;
        }

        public int FightRecapLevel
        {
            get => base["FightRecapLevel"].ToInt32(CultureInfo.CurrentCulture);
            set => base["FightRecapLevel"] = value;
        }

        public string AbilityCooldown
        {
            get => base["AbilityCooldown"].ToString(CultureInfo.CurrentCulture);
            set => base["AbilityCooldown"] = value;
        }

        public DotaAbilityType? AbilityType
        {
            get => ToFlag<DotaAbilityType>("AbilityType");
            set => base["AbilityType"] = FromFlag(value.Value);
        }
        public int RequiredLevel
        {
            get => base["RequiredLevel"].ToInt32(CultureInfo.CurrentCulture);
            set => base["RequiredLevel"] = value;
        }
        public int LevelsBetweenUpgrades
        {
            get => base["LevelsBetweenUpgrades"].ToInt32(CultureInfo.CurrentCulture);
            set => base["LevelsBetweenUpgrades"] = value;
        }
        /// <summary>
        /// Cast While Hidden
        /// </summary>
        public bool IsCastableWhileHidden
        {
            get => base["IsCastableWhileHidden"].ToBoolean(CultureInfo.CurrentCulture);
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
