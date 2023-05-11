using Dota2Modding.VisualEditor.GUI;
using Dota2Modding.VisualEditor.GUI.Abstraction.EditorMenu;
using Dota2Modding.VisualEditor.GUI.Abstraction.Menu;
using Dota2Modding.VisualEditor.Plugins.Project.Abstraction.Events;
using EmberKernel.Services.EventBus;
using EmberKernel.Services.EventBus.Handlers;
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

        public override string Id => "B273E1D7-E0B5-462E-8991-619F372ABBD1";

        public override string ParentId => FileMenu.GUID;

        public override string Name => "Open Addon";

        public override string Description => "Open a Dota2 Project game folder";

        public override event EventHandler? CanExecuteChanged;

        public bool IsProjectOpend { get; private set; } = false;

        public OpenProjectMenu(Dota2Locator dota2Locator, IEventBus eventBus)
        {
            this.dota2Locator = dota2Locator;
            this.eventBus = eventBus;
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
            var ext = new CommonFileDialogFilter();
            ext.DisplayName = "AddonInfo Files";
            ext.Extensions.Add("txt_c;addoninfo.txt");
            dialog.Filters.Add(ext);

            var result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                eventBus.Publish(new ProjectSelectedEvent() { SelectedAddonInfoFile = dialog.FileName! });
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ValueTask Handle(ProjectLoadedEvent @event)
        {
            IsProjectOpend = true;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);

            return default;
        }

        public ValueTask Handle(ProjectUnloadEvent @event)
        {
            IsProjectOpend = false;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);

            return default;
        }

        public override bool CanExecute(object? parameter)
        {
            return !IsProjectOpend;
        }
    }
}
