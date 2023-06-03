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
            get => GetString("BaseClass");
            set => base["BaseClass"] = value;
        }

        public string Model
        {
            get => GetString("Model");
            set => base["Model"] = value;
        }

        public string SoundSet
        {
            get => GetString("SoundSet");
            set => base["SoundSet"] = value;
        }

        public bool IsCastableWhileHidden
        {
            get => GetBoolean("IsCustom") ?? false;
            set => base["IsCustom"] = value ? 1 : 0;
        }


        public float ArmorPhysical
        {
            get => GetSingle("ArmorPhysical") ?? 0;
            set => base["ArmorPhysical"] = value.ToString();
        }

        public DotaUnitAttackCapability? AttackCapabilities
        {
            get => ToFlag<DotaUnitAttackCapability>("AttackCapabilities");
            set => base["AttackCapabilities"] = FromFlag(value.Value);
        }
        public float AttackDamageMin
        {
            get => GetSingle("AttackDamageMin") ?? 0f;
            set => base["AttackDamageMin"] = value.ToString();
        }
        public float AttackDamageMax
        {
            get => GetSingle("AttackDamageMax") ?? 0f;
            set => base["AttackDamageMax"] = value.ToString();
        }
        public float AttackRate
        {
            get => GetSingle("AttackRate") ?? 1f;
            set => base["AttackRate"] = value.ToString();
        }
        public float BaseAttackSpeed
        {
            get => GetSingle("BaseAttackSpeed") ?? 100f;
            set => base["BaseAttackSpeed"] = value.ToString();
        }
        public float AttackAnimationPoint
        {
            get => GetSingle("AttackAnimationPoint") ?? 0;
            set => base["AttackAnimationPoint"] = value.ToString();
        }
        public float AttackAcquisitionRange
        {
            get => GetSingle("AttackAcquisitionRange") ?? 0;
            set => base["AttackAcquisitionRange"] = value.ToString();
        }
        public float AttackRange
        {
            get => GetSingle("AttackRange") ?? 0;
            set => base["AttackRange"] = value.ToString();
        }
        public string ProjectileModel
        {
            get => GetString("ProjectileModel");
            set => base["ProjectileModel"] = value;
        }
        public float ProjectileSpeed
        {
            get => GetSingle("ProjectileSpeed") ?? 0;
            set => base["ProjectileSpeed"] = value.ToString();
        }
        public DotaAttribute? AttributePrimary
        {
            get => ToFlag<DotaAttribute>("AttributePrimary");
            set => base["AttributePrimary"] = FromFlag(value.Value);
        }
        public float AttributeBaseStrength
        {
            get => GetSingle("AttributeBaseStrength") ?? 0;
            set => base["AttributeBaseStrength"] = value.ToString();
        }
        public float AttributeStrengthGain
        {
            get => GetSingle("AttributeStrengthGain") ?? 0;
            set => base["AttributeStrengthGain"] = value.ToString();
        }
        public float AttributeBaseIntelligence
        {
            get => GetSingle("AttributeBaseIntelligence") ?? 0;
            set => base["AttributeBaseIntelligence"] = value.ToString();
        }
        public float AttributeIntelligenceGain
        {
            get => GetSingle("AttributeIntelligenceGain") ?? 0;
            set => base["AttributeIntelligenceGain"] = value.ToString();
        }
        public float AttributeBaseAgility
        {
            get => GetSingle("AttributeBaseAgility") ?? 0;
            set => base["AttributeBaseAgility"] = value.ToString();
        }
        public float AttributeAgilityGain
        {
            get => GetSingle("AttributeAgilityGain") ?? 0;
            set => base["AttributeAgilityGain"] = value.ToString();
        }
        public float StatusMana
        {
            get => GetSingle("StatusMana") ?? 0;
            set => base["StatusMana"] = value.ToString();
        }
        public float StatusManaRegen
        {
            get => GetSingle("StatusManaRegen") ?? 0;
            set => base["StatusManaRegen"] = value.ToString();
        }
        public float StatusHealth
        {
            get => GetSingle("StatusHealth") ?? 0;
            set => base["StatusHealth"] = value.ToString();
        }
        public float StatusHealthRegen
        {
            get => GetSingle("StatusHealthRegen") ?? 0;
            set => base["StatusHealthRegen"] = value.ToString();
        }
        public DotaUnitMoveCapability? MovementCapabilities
        {
            get => ToFlag<DotaUnitMoveCapability>("MovementCapabilities");
            set => base["MovementCapabilities"] = FromFlag(value.Value);
        }
        public float MovementSpeed
        {
            get => GetSingle("MovementSpeed") ?? 0;
            set => base["MovementSpeed"] = value.ToString();
        }
        public float MovementTurnRate
        {
            get => GetSingle("MovementTurnRate") ?? 0;
            set => base["MovementTurnRate"] = value.ToString();
        }

        public bool HasAggressiveStance
        {
            get => GetBoolean("HasAggressiveStance") ?? false;
            set => base["HasAggressiveStance"] = value ? 1 : 0;
        }

        public string particle_folder
        {
            get => GetString("particle_folder");
            set => base["particle_folder"] = value;
        }

        public string GameSoundsFile
        {
            get => GetString("GameSoundsFile");
            set => base["GameSoundsFile"] = value;
        }

        public string VoiceFile
        {
            get => GetString("VoiceFile");
            set => base["VoiceFile"] = value;
        }

        public string IdleSoundLoop
        {
            get => GetString("IdleSoundLoop");
            set => base["IdleSoundLoop"] = value;
        }

        public bool HasInventory
        {
            get => GetBoolean("HasInventory") ?? false;
            set => base["HasInventory"] = value ? 1 : 0;
        }
        public float VisionDaytimeRange
        {
            get => GetSingle("VisionDaytimeRange") ?? 0;
            set => base["VisionDaytimeRange"] = value.ToString();
        }
        public float VisionNighttimeRange
        {
            get => GetSingle("VisionNighttimeRange") ?? 0;
            set => base["VisionNighttimeRange"] = value.ToString();
        }
        public float MagicalResistance
        {
            get => GetSingle("MagicalResistance") ?? 0;
            set => base["MagicalResistance"] = value.ToString();
        }

        public string TeamName
        {
            get => GetString("TeamName");
            set => base["TeamName"] = value;
        }

        public bool DisableDamageDisplay
        {
            get => GetBoolean("DisableDamageDisplay") ?? false;
            set => base["DisableDamageDisplay"] = value ? 1 : 0;
        }
    }
}
