using Autofac;
using Dota2Modding.VisualEditor.GUI.Abstraction.EditorMenu;
using Dota2Modding.VisualEditor.GUI.EmberWpfCore.ViewModel;
using Dota2Modding.VisualEditor.Plugins.Project.Components;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using EmberWpfCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.Plugins.Project.Menu
{
    public class DotaHeroesPanelMenu : AbstractMenuItem
    {
        private readonly DotaHeroesPanel panel;
        private readonly RegisteredLayoutDocument documentManager;
        private readonly IWindowManager windowManager;

        public override string Id => "[Guid(\"2AA424C1-0CFE-47E8-8CFA-62CBDC07B5CD\")]";

        public override string ParentId => DotaProjectMenu.Guid;

        public override string Name => "Heroes";

        public override string Description => "Show heroes pannel";

        public override event EventHandler? CanExecuteChanged;

        public override bool CanExecute(object? parameter) => true;

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public DotaHeroesPanelMenu(DotaHeroesPanel panel, RegisteredLayoutDocument documentManager, IWindowManager windowManager)
        {
            this.panel = panel;
            this.documentManager = documentManager;
            this.windowManager = windowManager;
        }

        public override void Execute(object? parameter)
        {
            documentManager.AddOrOpen(panel);
        }
    }
}
