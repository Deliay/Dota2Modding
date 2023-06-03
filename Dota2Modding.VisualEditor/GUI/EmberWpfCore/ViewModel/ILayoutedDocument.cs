using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.GUI.EmberWpfCore.ViewModel
{
    public interface ILayoutedDocument : ILayoutedObject
    {
        public bool Closeable { get; }
    }
}
