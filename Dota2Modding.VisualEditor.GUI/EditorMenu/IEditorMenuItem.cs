using EmberKernel.Plugins.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dota2Modding.VisualEditor.GUI.EditorMenu
{
    public interface IEditorMenuItem : ICommand, IComponent, IMenuItemManager
    {
        string Id { get; }
        string ParentId { get; }
        string Name { get; }
        string Description { get; }
    }
}
