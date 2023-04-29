using Autofac;
using EmberKernel;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using EmberWpfCore.ViewModel;
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
        public ValueTask Initialize(ILifetimeScope scope)
        {
            var tabs = scope.Resolve<RegisteredTabs>();
            Kernel = scope.Resolve<Kernel>();
            //(FindName("Tabs") as TabControl).ItemsSource = tabs;
            Show();
            return default;
        }

        public ValueTask Uninitialize(ILifetimeScope scope)
        {
            Hide();
            Close();
            return default;
        }
    }
}
