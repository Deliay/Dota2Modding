using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.Game
{
    public abstract class KvSingleTypeCollecion<T> : BasicObject where T : BasicObject
    {
        protected KvSingleTypeCollecion(string name) : base(name)
        {
        }

        protected KvSingleTypeCollecion(string name, KVValue value) : base(name, value)
        {
        }

        protected abstract T Convert(string key, KVValue value);

        public new T this[string key]
        {
            get
            {

                if (base[key] is KVValue value)
                {
                    if (value.ValueType != KVValueType.Collection) return default!;
                    return Convert(key, value);
                }
                return default!;
            }
            set
            {
                base[key] = value.Value;
            }
        }
    }
}
