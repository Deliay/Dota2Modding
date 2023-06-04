using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.GUI.EmberWpfCore.ViewModel
{
    public interface ILayoutedPanel : ILayoutedObject
    {
        public int InitialHeight { get; }

        public bool InitialDock { get; }
    }
}
