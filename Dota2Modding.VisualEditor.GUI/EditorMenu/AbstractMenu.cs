using Autofac;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.GUI.EditorMenu
{
    public abstract class AbstractMenu : AbstractMenuItem
    {
        public override bool CanExecute(object? parameter) => true;

        public override void Execute(object? parameter) { }
    }
}
