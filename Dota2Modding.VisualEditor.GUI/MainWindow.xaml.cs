using Autofac;
using AvalonDock.Layout.Serialization;
using Dota2Modding.VisualEditor.GUI.EditorMenu;
using Dota2Modding.VisualEditor.GUI.EmberWpfCore.ViewModel;
using EmberKernel;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using EmberWpfCore.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace Dota2Modding.VisualEditor.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IHostedWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Kernel Kernel { get; set; }
        private IWindowManager WindowManager { get; set; }
        private IMenuItemManager MenuItemManager { get; set; } 
        public async ValueTask Initialize(ILifetimeScope scope)
        {
            Kernel = scope.Resolve<Kernel>();
            WindowManager = scope.Resolve<IWindowManager>();
            MenuItemManager = scope.Resolve<IMenuItemManager>();
            menu.ItemsSource = MenuItemManager;
            Show();
            var layoutObjects = scope.Resolve<IEnumerable<ILayoutedObject>>();
            foreach (var layoutObject in layoutObjects)
            {
                await scope.RegisterPanel(this.dockManager, layoutObject);
            }
            var serializer = new XmlLayoutSerializer(dockManager);
            serializer.Deserialize("layout.xml");
        }

        public ValueTask Uninitialize(ILifetimeScope scope)
        {
            var serializer = new XmlLayoutSerializer(dockManager);
            using var writer = new StreamWriter("layout.xml");
            serializer.Serialize(writer);
            Hide();
            return default;
        }

        private void OpenTookit(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WindowManager.BeginUIThreadScope(() => Kernel.Exit());
            e.Cancel = true;
        }
    }
}
