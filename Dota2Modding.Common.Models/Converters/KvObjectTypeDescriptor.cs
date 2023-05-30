using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.Converters
{
    internal class KvObjectTypeDescriptor : CustomTypeDescriptor
    {
        private readonly KVObject instance;

        public KvObjectTypeDescriptor(KVObject instance, ICustomTypeDescriptor? parent) : base(parent)
        {
            this.instance = instance;
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[]? attributes)
        {
            var properties = base.GetProperties(attributes).Cast<PropertyDescriptor>();
            var dictProperty = properties
                .Where(p => p.ComponentType == typeof(KVObject))
                .FirstOrDefault(p => p.Name == "Value");

            var descriptors = instance.Select(x => new KvPropertyDescriptor(x, x.Name)).ToArray();
            var result = new PropertyDescriptorCollection(descriptors.ToArray());
            return result;
        }
    }
}
