using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.Converters
{
    public class KvPropertyDescriptor : PropertyDescriptor
    {
        private readonly KVObject kvObject;
        private readonly string key;

        public KvPropertyDescriptor(KVObject kvObject, string key) : base(key, null)
        {
            this.kvObject = kvObject;
            this.key = key;
        }

        public override string DisplayName => $"KV.{Name}";

        public override Type ComponentType => null;

        public override bool IsReadOnly => true;

        public override Type PropertyType => typeof(KVValue);

        public override string Category => "KV";

        public override bool CanResetValue(object component) => false;

        public override object? GetValue(object? component)
        {
            return kvObject[key];
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object? component, object? value)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }
    }
}
