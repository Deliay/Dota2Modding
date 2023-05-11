using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.Plugins.Project.Abstraction.Events
{
    public class ProjectUnloadEvent : Event<ProjectUnloadEvent>
    {
        public string WorkingDirectory { get; set; }
    }
}
