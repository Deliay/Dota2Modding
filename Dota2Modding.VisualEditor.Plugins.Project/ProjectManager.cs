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
using Dota2Modding.Common.Models.Parser;

namespace Dota2Modding.VisualEditor.Plugins.Project
{
    public class ProjectManager : IComponent, IEventHandler<ProjectSelectedEvent>
    {
        public DotaProject DotaProject { get; private set; }

        private readonly IEventBus eventBus;
        private readonly ILogger<ProjectManager> logger;
        private readonly Dota2Locator dota2Locator;
        private readonly ILogger<DotaProject> dotaProjLogger;

        public ProjectManager(IEventBus eventBus, ILogger<ProjectManager> logger, Dota2Locator dota2Locator, ILogger<DotaProject> dotaProjLogger)
        {
            this.eventBus = eventBus;
            this.logger = logger;
            this.dota2Locator = dota2Locator;
            this.dotaProjLogger = dotaProjLogger;
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

                DotaProject = new(dotaProjLogger, file, dota2Locator);
                logger.LogInformation($"Addon {DotaProject.WorkingDirectory} loaded");

                DotaProject.InitBasePackages(file);

                await eventBus.Publish(new ProjectLoadedEvent()
                {
                    WorkingDirectory = DotaProject.WorkingDirectory,
                    AddonInfoFile = file,
                }, default);
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
