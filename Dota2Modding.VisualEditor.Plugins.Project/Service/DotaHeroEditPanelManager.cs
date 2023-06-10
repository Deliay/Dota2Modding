using Dota2Modding.Common.Models.Project;
using Dota2Modding.VisualEditor.Plugins.Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.Plugins.Project.Service
{
    public class DotaHeroEditPanelManager : ObservableCollection<DotaHeroViewModel>
    {
        private readonly DotaProject project;

        public DotaHeroEditPanelManager(DotaProject project)
        {
            this.project = project;
        }
    }
}
