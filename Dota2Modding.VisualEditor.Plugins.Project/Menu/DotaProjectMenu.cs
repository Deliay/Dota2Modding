using Autofac;
using Dota2Modding.VisualEditor.GUI.Abstraction.EditorMenu;
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
    public class DotaProjectMenu : AbstractMenu,
        IEventHandler<ProjectLoadedEvent>,
        IEventHandler<ProjectUnloadEvent>
    {
        private readonly ILifetimeScope scope;
        private readonly IWindowManager windowManager;

        public DotaProjectMenu(ILifetimeScope scope, IWindowManager windowManager)
        {
            this.scope = scope;
            this.windowManager = windowManager;
        }

        public ValueTask Handle(ProjectLoadedEvent @event)
        {
            return windowManager.BeginUIThreadScope(scope.InitializeMenuItem<DotaProjectMenu>);
        }

        public ValueTask Handle(ProjectUnloadEvent @event)
        {
            return windowManager.BeginUIThreadScope(scope.UnInitializeMenuItem<DotaProjectMenu>);
        }

        public const string Guid = "[Guid(\"DD1881B5-36D3-46B1-9FBA-BD97DDA9B562\")]";

        public override string Id => Guid;

        public override string ParentId => null!;

        public override string Name => "Project";

        public override string Description => "Dota Project related menu";

        public override event EventHandler? CanExecuteChanged;

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
