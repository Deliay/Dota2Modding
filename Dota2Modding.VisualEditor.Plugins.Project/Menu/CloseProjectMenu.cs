using Dota2Modding.VisualEditor.GUI.Abstraction.EditorMenu;
using Dota2Modding.VisualEditor.GUI.Abstraction.Menu;
using Dota2Modding.VisualEditor.Plugins.Project.Abstraction.Events;
using EmberKernel.Services.EventBus.Handlers;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.Plugins.Project.Menu
{
    public class CloseProjectMenu : AbstractMenuItem,
        IEventHandler<ProjectLoadedEvent>,
        IEventHandler<ProjectUnloadEvent>
    {
        private readonly IWindowManager windowManager;
        private readonly ProjectManager projectManager;

        public override string Id => "[Guid(\"5D8427F9-9993-4507-95EF-35BC910C54D8\")]";

        public override string ParentId => FileMenu.GUID;

        public override string Name => "Close Project";

        public override string Description => "Close opend project";

        public override event EventHandler? CanExecuteChanged;

        public bool IsProjectOpend { get; private set; } = false;

        public override bool CanExecute(object? parameter)
        {
            return IsProjectOpend;
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override async void Execute(object? parameter)
        {
            await Task.Run(() => windowManager.BeginUIThreadScope(projectManager.CloseProject));
        }

        public CloseProjectMenu(IWindowManager windowManager, ProjectManager projectManager)
        {
            this.windowManager = windowManager;
            this.projectManager = projectManager;
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
    }
}
