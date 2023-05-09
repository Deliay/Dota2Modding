using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.GUI.EditorMenu
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class MenuParentAttribute : Attribute
    {
        readonly string parentGuid;

        public MenuParentAttribute(string parentGuid)
        {
            this.parentGuid = parentGuid;
        }

        public string PositionalString
        {
            get { return parentGuid; }
        }
    }
}
