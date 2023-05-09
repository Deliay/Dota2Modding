using Dota2Modding.VisualEditor.GUI.EditorMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.GUI.Components.Menu
{
    public class FileMenu : AbstractMenu
    {
        public const string GUID = "0643707C-30F2-457F-9C4B-5C66D687704F";
        public override string Id => GUID;

        public override string ParentId => null!;

        public override string Name => "File";

        public override string Description => "File";

        public override event EventHandler? CanExecuteChanged;

        public override void Dispose()
        {
        }

    }
}
