using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.Game.Abilities
{
    public class DotaAbilityValue : BasicObject
    {
        public DotaAbilityValue(string name, KVValue value) : base(name, value)
        {
        }
        public DotaAbilityValue(string name) : base(name)
        {
        }

        public new string Value
        {
            get => GetString("value");
            set => base["value"] = value;
        }

        public static DotaAbilityValue FromSingleValue(string key, string value)
        {
            return new DotaAbilityValue(key)
            {
                Value = value
            };
        }
    }
}
