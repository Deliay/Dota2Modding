using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models
{
    public class IgnoreCaseBasicObject : BasicObject
    {
        private readonly Dictionary<string, string> Mapping = new();
        public IgnoreCaseBasicObject(KVObject kv) : base(kv.Name, kv.Value)
        {
            if (Value.ValueType == KVValueType.Collection)
            {
                var lowerKv = new KVObject(kv.Name, Enumerable.Empty<KVObject>());
                foreach (var obj in kv)
                {
                    Mapping.Add(obj.Name.ToLower(), obj.Name);
                }
            }
        }

        private string FallbackKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return key;

            if (Mapping.TryGetValue(key.ToLower(), out var value))
            {
                return value;
            }
            return key;
        }

        public override KVValue this[string key]
        {
            get => base[FallbackKey(key)];
            set => base[FallbackKey(key)] = value;
        }
    }
}
