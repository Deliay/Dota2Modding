using Dota2Modding.VisualEditor.GUI.Abstraction.EditorMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.GUI.Abstraction.Menu.Plugins
{
    public class InstalledPluginMenuItem : AbstractMenuItem
    {
        public InstalledPluginMenuItem(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
        public override string Id { get; }

        public override string ParentId => InstalledPluginsMenu.GUID;

        public override string Name { get; }

        public override string Description { get; }

        public override event EventHandler? CanExecuteChanged;

        public override bool CanExecute(object? parameter)
        {
            return false;
        }

        public override void Dispose()
        {
        }

        public override void Execute(object? parameter)
        {
        }
    }
}
