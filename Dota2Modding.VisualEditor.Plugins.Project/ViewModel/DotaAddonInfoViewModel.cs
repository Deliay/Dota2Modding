using Dota2Modding.Common.Models.Addon;
using Dota2Modding.Common.Models.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.Plugins.Project.ViewModel
{
    public class DotaAddonInfoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly DotaProject project;

        public AddonInfo Addon { get; }

        public DotaAddonInfoViewModel(DotaProject project)
        {
            this.project = project;
            this.Addon = project.AddonInfo;
        }
    }
}
