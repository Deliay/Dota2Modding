using Dota2Modding.Common.Models.Game.Constants;
using Dota2Modding.Common.Models.Game.Heroes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.Game.Entities
{
    public class DotaEntity : BasicObject
    {
        private readonly EntityAbilityCollection _abilities;
        public DotaEntity(string name) : base(name)
        {
            _abilities = new(this);
        }

        public DotaEntity(string name, KVValue value) : base(name, value)
        {
            _abilities = new(this);
        }

        public EntityAbilityCollection Abilities => _abilities;

        public string BaseClass
        {
            get => base["BaseClass"]?.ToString(CultureInfo.CurrentCulture);
            set => base["BaseClass"] = value;
        }

        public string HeroName
        {
            get => base["Name"]?.ToString(CultureInfo.CurrentCulture);
            set => base["Name"] = value;
        }

        public string Model
        {
            get => base["Model"]?.ToString(CultureInfo.CurrentCulture);
            set => base["Model"] = value;
        }

        public string SoundSet
        {
            get => base["SoundSet"]?.ToString(CultureInfo.CurrentCulture);
            set => base["SoundSet"] = value;
        }

        public bool IsCastableWhileHidden
        {
            get => base["IsCustom"].ToBoolean(CultureInfo.CurrentCulture);
            set => base["IsCustom"] = value ? 1 : 0;
        }


        public double ArmorPhysical
        {
            get => base["ArmorPhysical"].ToDouble(CultureInfo.CurrentCulture);
            set => base["ArmorPhysical"] = value.ToString();
        }

        public DotaUnitAttackCapability AttackCapabilities
        {
            get => ToFlag<DotaUnitAttackCapability>("AttackCapabilities");
            set => base["AttackCapabilities"] = FromFlag(value);
        }
        public double AttackDamageMin
        {
            get => base["AttackDamageMin"].ToDouble(CultureInfo.CurrentCulture);
            set => base["AttackDamageMin"] = value.ToString();
        }
        public double AttackDamageMax
        {
            get => base["AttackDamageMax"].ToDouble(CultureInfo.CurrentCulture);
            set => base["AttackDamageMax"] = value.ToString();
        }
        public double AttackRate
        {
            get => base["AttackRate"].ToDouble(CultureInfo.CurrentCulture);
            set => base["AttackRate"] = value.ToString();
        }
        public double BaseAttackSpeed
        {
            get => base["BaseAttackSpeed"].ToDouble(CultureInfo.CurrentCulture);
            set => base["BaseAttackSpeed"] = value.ToString();
        }
        public double AttackAnimationPoint
        {
            get => base["AttackAnimationPoint"].ToDouble(CultureInfo.CurrentCulture);
            set => base["AttackAnimationPoint"] = value.ToString();
        }
        public double AttackAcquisitionRange
        {
            get => base["AttackAcquisitionRange"].ToDouble(CultureInfo.CurrentCulture);
            set => base["AttackAcquisitionRange"] = value.ToString();
        }
        public double AttackRange
        {
            get => base["AttackRange"].ToDouble(CultureInfo.CurrentCulture);
            set => base["AttackRange"] = value.ToString();
        }
        public string ProjectileModel
        {
            get => base["ProjectileModel"]?.ToString(CultureInfo.CurrentCulture);
            set => base["ProjectileModel"] = value;
        }
        public double ProjectileSpeed
        {
            get => base["ProjectileSpeed"].ToDouble(CultureInfo.CurrentCulture);
            set => base["ProjectileSpeed"] = value.ToString();
        }
        public DotaAttribute AttributePrimary
        {
            get => ToFlag<DotaAttribute>("AttributePrimary");
            set => base["AttributePrimary"] = FromFlag(value);
        }
        public double AttributeBaseStrength
        {
            get => base["AttributeBaseStrength"].ToDouble(CultureInfo.CurrentCulture);
            set => base["AttributeBaseStrength"] = value.ToString();
        }
        public double AttributeStrengthGain
        {
            get => base["AttributeStrengthGain"].ToDouble(CultureInfo.CurrentCulture);
            set => base["AttributeStrengthGain"] = value.ToString();
        }
        public double AttributeBaseIntelligence
        {
            get => base["AttributeBaseIntelligence"].ToDouble(CultureInfo.CurrentCulture);
            set => base["AttributeBaseIntelligence"] = value.ToString();
        }
        public double AttributeIntelligenceGain
        {
            get => base["AttributeIntelligenceGain"].ToDouble(CultureInfo.CurrentCulture);
            set => base["AttributeIntelligenceGain"] = value.ToString();
        }
        public double AttributeBaseAgility
        {
            get => base["AttributeBaseAgility"].ToDouble(CultureInfo.CurrentCulture);
            set => base["AttributeBaseAgility"] = value.ToString();
        }
        public double AttributeAgilityGain
        {
            get => base["AttributeAgilityGain"].ToDouble(CultureInfo.CurrentCulture);
            set => base["AttributeAgilityGain"] = value.ToString();
        }
        public double StatusMana
        {
            get => base["StatusMana"].ToDouble(CultureInfo.CurrentCulture);
            set => base["StatusMana"] = value.ToString();
        }
        public double StatusManaRegen
        {
            get => base["StatusManaRegen"].ToDouble(CultureInfo.CurrentCulture);
            set => base["StatusManaRegen"] = value.ToString();
        }
        public double StatusHealth
        {
            get => base["StatusHealth"].ToDouble(CultureInfo.CurrentCulture);
            set => base["StatusHealth"] = value.ToString();
        }
        public double StatusHealthRegen
        {
            get => base["StatusHealthRegen"].ToDouble(CultureInfo.CurrentCulture);
            set => base["StatusHealthRegen"] = value.ToString();
        }
        public DotaUnitMoveCapability MovementCapabilities
        {
            get => ToFlag<DotaUnitMoveCapability>("MovementCapabilities");
            set => base["MovementCapabilities"] = FromFlag(value);
        }
        public double MovementSpeed
        {
            get => base["MovementSpeed"].ToDouble(CultureInfo.CurrentCulture);
            set => base["MovementSpeed"] = value.ToString();
        }
        public double MovementTurnRate
        {
            get => base["MovementTurnRate"].ToDouble(CultureInfo.CurrentCulture);
            set => base["MovementTurnRate"] = value.ToString();
        }

        public bool HasAggressiveStance
        {
            get => base["HasAggressiveStance"].ToBoolean(CultureInfo.CurrentCulture);
            set => base["HasAggressiveStance"] = value ? 1 : 0;
        }

        public string particle_folder
        {
            get => base["particle_folder"]?.ToString(CultureInfo.CurrentCulture);
            set => base["particle_folder"] = value;
        }

        public string GameSoundsFile
        {
            get => base["GameSoundsFile"]?.ToString(CultureInfo.CurrentCulture);
            set => base["GameSoundsFile"] = value;
        }

        public string VoiceFile
        {
            get => base["VoiceFile"]?.ToString(CultureInfo.CurrentCulture);
            set => base["VoiceFile"] = value;
        }

        public string IdleSoundLoop
        {
            get => base["IdleSoundLoop"]?.ToString(CultureInfo.CurrentCulture);
            set => base["IdleSoundLoop"] = value;
        }

        public bool HasInventory
        {
            get => base["HasInventory"].ToBoolean(CultureInfo.CurrentCulture);
            set => base["HasInventory"] = value ? 1 : 0;
        }
        public double VisionDaytimeRange
        {
            get => base["VisionDaytimeRange"].ToDouble(CultureInfo.CurrentCulture);
            set => base["VisionDaytimeRange"] = value.ToString();
        }
        public double VisionNighttimeRange
        {
            get => base["VisionNighttimeRange"].ToDouble(CultureInfo.CurrentCulture);
            set => base["VisionNighttimeRange"] = value.ToString();
        }
        public double MagicalResistance
        {
            get => base["MagicalResistance"].ToDouble(CultureInfo.CurrentCulture);
            set => base["MagicalResistance"] = value.ToString();
        }

        public string TeamName
        {
            get => base["TeamName"]?.ToString(CultureInfo.CurrentCulture);
            set => base["TeamName"] = value;
        }

        public bool DisableDamageDisplay
        {
            get => base["DisableDamageDisplay"].ToBoolean(CultureInfo.CurrentCulture);
            set => base["DisableDamageDisplay"] = value ? 1 : 0;
        }
    }
}
