using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace Dota2Modding.Common.Models.Converters
{
    public class KvObjectDescriptionProvider : TypeDescriptionProvider
    {
        private readonly KVObject kvObject;

        public KvObjectDescriptionProvider(KVObject kvObject)
            : base(TypeDescriptor.GetProvider(typeof(KVObject)))
        {
            this.kvObject = kvObject;
        }

        public override ICustomTypeDescriptor? GetTypeDescriptor(Type objectType, object? instance)
        {
            return new KvObjectTypeDescriptor(kvObject, base.GetTypeDescriptor(objectType, instance));
        }
    }
}
