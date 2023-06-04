using Autofac;
using Dota2Modding.Common.Models.Project;
using Dota2Modding.VisualEditor.GUI.Abstraction.Document;
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
    /// Interaction logic for DotaHeroesPanel.xaml
    /// </summary>
    public partial class DotaHeroesPanel : UserControl,
        IViewComponent, ILayoutedDocument,
        IEventHandler<ProjectLoadedEvent>,
        IEventHandler<ProjectUnloadEvent>
    {
        private ILifetimeScope scope;
        private IWindowManager windowManager;
        private ProjectManager projectManager;

        public DotaHeroesPanel()
        {
            InitializeComponent();
        }

        public string Id => "[Guid(\"5B9608F8-879E-4950-8569-F3F9955EF16B\")]";

        public string Title => "Heroes";

        public bool Closeable => false;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async ValueTask Handle(ProjectLoadedEvent @event)
        {
            this.projectManager = scope.Resolve<ProjectManager>();
            await windowManager.BeginUIThreadScope(() =>
            {
                this.DataContext = projectManager.HeroViewModel;
            });
        }

        public async ValueTask Handle(ProjectUnloadEvent @event)
        {
            await windowManager.BeginUIThreadScope(() =>
            {
                this.DataContext = null;
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
