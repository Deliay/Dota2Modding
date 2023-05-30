using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Models.Converters
{
    public class BasicObjectTypeCustomTypeDescriptor : CustomTypeDescriptor
    {
        private readonly Type _baseType;

        public BasicObjectTypeCustomTypeDescriptor(ICustomTypeDescriptor parent, Type baseType)
            : base(parent)
        {
            _baseType = baseType;
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            var props = base.GetProperties().Cast<PropertyDescriptor>().Where(p => (!_baseType.IsAssignableTo(p.ComponentType)) && p.ComponentType != _baseType);
            return new PropertyDescriptorCollection(props.ToArray());
        }
    }
}
