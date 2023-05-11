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

namespace Dota2Modding.VisualEditor.GUI.Components.Consoles
{
    /// <summary>
    /// Interaction logic for ConsolePanel.xaml
    /// </summary>
    public partial class ConsolePanel : UserControl, IViewComponent, ILayoutedPanel, IDefaultLayoutStrategy
    {
        public ConsolePanel()
        {
            InitializeComponent();
        }

        public string Id => "491F3404-6E28-4898-A054-E9D6B4262CBF";
        public string Title => "Console";
        public AnchorSide DefaultStrategy => AnchorSide.Bottom;

        public int InitialHeight => 200;

        public void Dispose()
        {
        }

        public ValueTask Initialize(ILifetimeScope scope)
        {
            this.terminal.ItemsSource = scope.Resolve<ConsoleObservableSubscriber>();
            return ValueTask.CompletedTask;
        }

        public ValueTask Uninitialize(ILifetimeScope scope)
        {
            this.terminal.ItemsSource = null;
            return ValueTask.CompletedTask;
        }
    }
}
