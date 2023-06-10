using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.GUI.Abstraction
{
    public class MainWindowConfiguration
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public string LastProject { get; set; } = string.Empty;

        public bool EditorConfirmnation { get; set; } = false;
    }
}
