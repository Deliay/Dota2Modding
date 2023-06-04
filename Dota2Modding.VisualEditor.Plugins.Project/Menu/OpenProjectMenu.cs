using Dota2Modding.Common.Models.Project;
using Dota2Modding.VisualEditor.GUI;
using Dota2Modding.VisualEditor.GUI.Abstraction.EditorMenu;
using Dota2Modding.VisualEditor.GUI.Abstraction.Menu;
using Dota2Modding.VisualEditor.Plugins.Project.Abstraction.Events;
using EmberKernel.Services.EventBus;
using EmberKernel.Services.EventBus.Handlers;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.Plugins.Project.Menu
{
    public class OpenProjectMenu : AbstractMenuItem,
        IEventHandler<ProjectLoadedEvent>,
        IEventHandler<ProjectUnloadEvent>
    {
        private readonly Dota2Locator dota2Locator;
        private readonly IEventBus eventBus;
        private readonly IWindowManager windowManager;

        public override string Id => "B273E1D7-E0B5-462E-8991-619F372ABBD1";

        public override string ParentId => FileMenu.GUID;

        public override string Name => "Open Addon";

        public override string Description => "Open a Dota2 Project game folder";

        public override event EventHandler? CanExecuteChanged;

        public bool IsProjectOpend { get; private set; } = false;

        public OpenProjectMenu(IEventBus eventBus, IWindowManager windowManager)
        {
            this.eventBus = eventBus;
            this.windowManager = windowManager;
            dota2Locator = new Dota2Locator();
        }

        public override void Execute(object? parameter)
        {
            var dota2 = dota2Locator.Locate();
            var dialog = new CommonOpenFileDialog
            {
                Multiselect = false,
                Title = "Select Dota2 addon...",
                EnsureFileExists = true,
                EnsurePathExists = true,
                DefaultDirectory = dota2 is not null ? Path.Combine(dota2.Path, "game", "dota_addons") : null,
            };
            var ext = new CommonFileDialogFilter
            {
                DisplayName = "AddonInfo Files"
            };
            ext.Extensions.Add("txt_c;addoninfo.txt");
            dialog.Filters.Add(ext);

            var result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                // wrap in new task
                Task.Run(() => eventBus.Publish(new ProjectSelectedEvent() { SelectedAddonInfoFile = dialog.FileName! }));
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async ValueTask Handle(ProjectLoadedEvent @event)
        {
            IsProjectOpend = true;
            await windowManager.BeginUIThreadScope(() => CanExecuteChanged?.Invoke(this, EventArgs.Empty));

        }

        public async ValueTask Handle(ProjectUnloadEvent @event)
        {
            IsProjectOpend = false;
            await windowManager.BeginUIThreadScope(() => CanExecuteChanged?.Invoke(this, EventArgs.Empty));
        }

        public override bool CanExecute(object? parameter)
        {
            return !IsProjectOpend;
        }
    }
}
