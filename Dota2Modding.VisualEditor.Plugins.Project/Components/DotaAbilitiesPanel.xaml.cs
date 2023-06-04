using Autofac;
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
using static System.Formats.Asn1.AsnWriter;

namespace Dota2Modding.VisualEditor.Plugins.Project.Components
{
    /// <summary>
    /// Interaction logic for DotaAbilitiesPanel.xaml
    /// </summary>
    public partial class DotaAbilitiesPanel : UserControl,
        IViewComponent, ILayoutedDocument,
        IEventHandler<ProjectLoadedEvent>,
        IEventHandler<ProjectUnloadEvent>
    {
        private ILifetimeScope scope;
        private IWindowManager windowManager;
        private ProjectManager projectManager;

        public DotaAbilitiesPanel()
        {
            InitializeComponent();
        }

        public bool Closeable => false;

        public string Id => "[Guid(\"F7C41D92-B278-4463-B027-F28BCC823FC2\")]";

        public string Title => "Abilities";

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async ValueTask Handle(ProjectLoadedEvent @event)
        {
            this.projectManager = scope.Resolve<ProjectManager>();
            await windowManager.BeginUIThreadScope(() =>
            {
                this.DataContext = this.projectManager.AbilitiesViewModel;
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
