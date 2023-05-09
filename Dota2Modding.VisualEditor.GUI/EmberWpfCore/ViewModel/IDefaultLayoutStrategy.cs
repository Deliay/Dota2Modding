using AvalonDock.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.GUI.EmberWpfCore.ViewModel
{
    public interface IDefaultLayoutStrategy
    {
        public AnchorableShowStrategy DefaultStrategy { get; }
    }
}
