using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.Game.Abilities
{
    public class DotaAbilityValues : BasicObject
    {
        public DotaAbilityValues(string name, KVValue value) : base(name, value)
        {
        }

        public new DotaAbilityValue this[string key]
        {
            get
            {
                var raw = base[key];
                return raw.ValueType switch
                {
                    KVValueType.Collection => new DotaAbilityValue(key, raw),
                    _ => DotaAbilityValue.FromSingleValue(key, raw.ToString(CultureInfo.CurrentCulture)),
                };
            }
            set
            {
                base[key] = value.Value;
            }
        }
    }
}
