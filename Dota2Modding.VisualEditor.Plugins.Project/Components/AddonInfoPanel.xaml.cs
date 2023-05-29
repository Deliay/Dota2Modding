using Autofac;
using AvalonDock.Layout;
using Dota2Modding.VisualEditor.GUI.EmberWpfCore.ViewModel;
using Dota2Modding.VisualEditor.Plugins.Project.Abstraction.Events;
using EmberKernel.Services.EventBus.Handlers;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dota2Modding.VisualEditor.Plugins.Project.Components
{
    /// <summary>
    /// Interaction logic for AddonInfoPanel.xaml
    /// </summary>
    public partial class AddonInfoPanel : UserControl,
        IViewComponent, ILayoutedPanel, IDefaultLayoutStrategy,
        IEventHandler<ProjectLoadedEvent>, IEventHandler<ProjectUnloadEvent>
    {
        private ILifetimeScope scope;
        private IWindowManager windowManager;

        public AddonInfoPanel()
        {
            InitializeComponent();
        }

        public AnchorSide DefaultStrategy => AnchorSide.Left;

        public int InitialHeight => 400;

        public string Id => "5D9B1280-FAB5-4FB9-92D8-4CC0FA84D778";

        public string Title => "Addon Information";

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async ValueTask Handle(ProjectLoadedEvent @event)
        {
            await windowManager.BeginUIThreadScope(() =>
            {
                var proj = scope.Resolve<ProjectManager>();
                this.grid.SelectedObject = proj.DotaProject.AddonInfo;
            });
        }

        public async ValueTask Handle(ProjectUnloadEvent @event)
        {
            await windowManager.BeginUIThreadScope(() =>
            {
                this.grid.SelectedObject = null!;
            });
        }

        public ValueTask Initialize(ILifetimeScope scope)
        {
            this.scope = scope;
            this.windowManager = scope.Resolve<IWindowManager>();

            return default;
        }

        public ValueTask Uninitialize(ILifetimeScope scope)
        {
            return default;
        }
    }
}
