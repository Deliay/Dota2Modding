using Dota2Modding.Common.Models.Addon;
using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.Plugins.Project.Abstraction.Events
{
    public class ProjectLoadedEvent : Event<ProjectLoadedEvent>
    {
        public string WorkingDirectory { get; set; }

        public AddonInfo AddonInfo { get; set; }

        public string AddonInfoFile { get; set; }
    }
}
