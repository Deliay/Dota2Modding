using Dota2Modding.VisualEditor.GUI.EditorMenu;
using EmberKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.GUI.Components.Menu
{
    public class MenuItemExit : AbstractMenuItem
    {
        private readonly Kernel kernel;

        public MenuItemExit(Kernel kernel)
        {
            this.kernel = kernel;
        }

        public override string Id => "F5D47F75-46CF-41E4-B1B1-9F77F4C1120C";

        public override string ParentId => FileMenu.GUID;

        public override string Name => "Exit";

        public override string Description => "Exit";

        public override event EventHandler? CanExecuteChanged;

        public override bool CanExecute(object? parameter)
        {
            return true;
        }

        public override void Dispose()
        {

        }

        public override void Execute(object? parameter)
        {
            kernel.Exit();
        }
    }
}
