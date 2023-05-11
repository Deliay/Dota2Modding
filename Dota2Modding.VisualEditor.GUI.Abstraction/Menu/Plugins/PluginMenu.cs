using Autofac;
using Dota2Modding.VisualEditor.GUI.Abstraction.EditorMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.GUI.Abstraction.Menu.Plugins
{
    public class PluginMenu : AbstractMenu
    {
        public PluginMenu(ILifetimeScope scope)
        {
            this.scope = scope;
        }

        public const string GUID = "91BF3066-EF1F-4D8B-899E-C83241BA5975";
        private readonly ILifetimeScope scope;

        public override string Id => GUID;

        public override string ParentId => null!;

        public override string Name => "Plugins";

        public override string Description => "Plugins";

        public override event EventHandler? CanExecuteChanged;

        public override void Dispose()
        {
        }

    }
}
