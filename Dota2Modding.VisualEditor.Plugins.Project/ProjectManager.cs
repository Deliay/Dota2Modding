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
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace Dota2Modding.VisualEditor.Plugins.Project
{
    public class ProjectManager : IComponent, IEventHandler<ProjectSelectedEvent>
    {
        public DotaProject DotaProject { get; private set; }

        private readonly IEventBus eventBus;
        private readonly ILogger<ProjectManager> logger;

        public ProjectManager(IEventBus eventBus, ILogger<ProjectManager> logger)
        {
            this.eventBus = eventBus;
            this.logger = logger;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async ValueTask Handle(ProjectSelectedEvent @event)
        {
            var file = @event.SelectedAddonInfoFile;
            logger.LogInformation($"Opening file {file}");
            try
            {
                var obj = KvLoader.Parse(file);
                if (AddonInfo.TryParse(obj, out var info))
                {
                    DotaProject = new(file, info);
                    logger.LogInformation($"Addon {DotaProject.WorkingDirectory} loaded");
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
