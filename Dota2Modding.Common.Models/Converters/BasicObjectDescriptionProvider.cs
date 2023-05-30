using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Models.Converters
{
    public class BasicObjectDescriptionProvider : TypeDescriptionProvider
    {
        private readonly Type _baseType;

        public BasicObjectDescriptionProvider(Type objectType, Type baseType)
            : base(TypeDescriptor.GetProvider(objectType))
        {
            _baseType = baseType;
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            ICustomTypeDescriptor descriptor = base.GetTypeDescriptor(objectType, instance);
            if (_baseType != null && _baseType.IsAssignableFrom(objectType))
            {
                descriptor = new BasicObjectTypeCustomTypeDescriptor(descriptor, _baseType);
            }
            return descriptor;
        }
    }
}
