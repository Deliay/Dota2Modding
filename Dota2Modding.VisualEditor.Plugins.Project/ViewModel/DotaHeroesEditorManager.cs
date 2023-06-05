using Dota2Modding.Common.Models.GameStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.Plugins.Project.ViewModel
{
    public class DotaHeroesEditorManager
    {
        private readonly Packages packages;

        public DotaHeroesEditorManager(Packages packages)
        {
            this.packages = packages;
        }
    }
}
