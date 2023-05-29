using Autofac;
using AvalonDock.Layout;
using Dota2Modding.VisualEditor.GUI.EmberWpfCore.ViewModel;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
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
    public partial class AddonInfoPanel : UserControl, IViewComponent, ILayoutedPanel, IDefaultLayoutStrategy
    {
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

        public ValueTask Initialize(ILifetimeScope scope)
        {
            var pm = scope.Resolve<ProjectManager>();

            return default;
        }

        public ValueTask Uninitialize(ILifetimeScope scope)
        {
            throw new NotImplementedException();
        }
    }
}
