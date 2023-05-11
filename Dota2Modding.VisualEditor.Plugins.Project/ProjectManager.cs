using Dota2Modding.Common.Models.Addon;
using Dota2Modding.Common.Models;
using Dota2Modding.VisualEditor.Plugins.Project.Abstraction;
using Dota2Modding.VisualEditor.Plugins.Project.Abstraction.Events;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus;
using EmberKernel.Services.EventBus.Handlers;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.Plugins.Project
{
    public class ProjectManager : IComponent, IEventHandler<ProjectSelectedEvent>
    {
        public DotaProject DotaProject { get; private set; }

        private readonly IEventBus eventBus;

        public ProjectManager(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async ValueTask Handle(ProjectSelectedEvent @event)
        {
            var file = @event.SelectedAddonInfoFile;
            try
            {
                var obj = KvLoader.Parse(file);
                if (AddonInfo.TryParse(obj, out var info))
                {
                    DotaProject = new(file, info);
                    await eventBus.Publish(new ProjectLoadedEvent()
                    {
                        AddonInfo = DotaProject.AddonInfo,
                        WorkingDirectory = DotaProject.WorkingDirectory,
                    }, default);
                }
            }
            catch (Exception e)
            {
                var errorDialog = new TaskDialog
                {
                    StartupLocation = TaskDialogStartupLocation.CenterOwner,
                    Text = e.Message,
                    StandardButtons = TaskDialogStandardButtons.Ok,
                    Icon = TaskDialogStandardIcon.Error,
                };
                errorDialog.Show();
            }
        }
    }
}
