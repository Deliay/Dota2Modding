using Autofac;
using AvalonDock.Layout;
using AvalonDock;
using AvalonDock.Layout.Serialization;
using Dota2Modding.VisualEditor.GUI.Abstraction.EditorMenu;
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
using System.Windows.Forms;
using System.Collections.Specialized;
using EmberKernel.Services.EventBus.Handlers;
using Dota2Modding.VisualEditor.Events;
using EmberKernel.Plugins.Components;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using Microsoft.Extensions.Options;
using Dota2Modding.VisualEditor.GUI.Abstraction;
using EmberKernel.Services.Configuration;
using Dota2Modding.VisualEditor.Plugins.Project.Abstraction.Events;
using EmberKernel.Services.EventBus;
using HandyControl.Tools;
using HandyControl.Controls;
using Dota2Modding.VisualEditor.GUI.Abstraction.Document;

namespace Dota2Modding.VisualEditor.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : GlowWindow, IHostedWindow, IComponent,
        IEventHandler<AllPluginResolvedEvent>,
        IEventHandler<ProjectLoadedEvent>,
        IEventHandler<ProjectUnloadEvent>
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Kernel Kernel { get; set; }
        private IWindowManager WindowManager { get; set; }
        private IMenuItemManager MenuItemManager { get; set; }
        private ILogger<MainWindow> Logger { get; set; }
        private IPluginOptions<Entry, MainWindowConfiguration> Config { get; set; }
        private IEventBus EventBus { get; set; }

        public async ValueTask Initialize(ILifetimeScope scope)
        {
            Kernel = scope.Resolve<Kernel>();
            WindowManager = scope.Resolve<IWindowManager>();
            MenuItemManager = scope.Resolve<IMenuItemManager>();
            Logger = scope.Resolve<ILogger<MainWindow>>();
            Config = scope.Resolve<IPluginOptions<Entry, MainWindowConfiguration>>();
            EventBus = scope.Resolve<IEventBus>();
            menu.ItemsSource = MenuItemManager;
            Show();
            var panelManager = scope.Resolve<RegisteredLayoutPanel>();
            foreach (var anchorable in panelManager.Items)
            {
                InsertToLayout(anchorable);
            }
            var documentManager = scope.Resolve<RegisteredLayoutDocument>();
            foreach (var document in documentManager.Items)
            {
                LayoutDocumentPane.Children.Add(document);
            }
            documentManager.CollectionChanged += DocumentManager_CollectionChanged;
            panelManager.CollectionChanged += PanelManager_CollectionChanged;
            ConfigHelper.Instance.SetWindowDefaultStyle();
            ConfigHelper.Instance.SetNavigationWindowDefaultStyle();
        }

        private void DocumentManager_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems.OfType<LayoutDocument>())
                {
                    LayoutDocumentPane.Children.Add(item);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems.OfType<LayoutDocument>())
                {
                    LayoutDocumentPane.Children.Remove(item);
                }
            }
        }

        public void InsertToLayout(LayoutAnchorable anchorable)
        {

            var pos = anchorable.Content is IDefaultLayoutStrategy defaultLayoutItem ? defaultLayoutItem.DefaultStrategy : AnchorSide.Left;
            //anchorable.AddToLayout(dockManager, AnchorableShowStrategy.Most);
            var pane = pos switch
            {
                AnchorSide.Left => left,
                AnchorSide.Right => right,
                _ => bottom,
            };

            anchorable.AddToLayout(dockManager, AnchorableShowStrategy.Most);
            pane.Children.Add(anchorable);

            if (anchorable.Content is ILayoutedPanel panel)
            {
                if (panel.InitialDock)
                {
                    anchorable.IsVisible = true;
                    anchorable.IsSelected = true;
                }
            }

        }

        public void RemoveFromLayout(LayoutAnchorable anchorable)
        {
            anchorable.Parent.RemoveChild(anchorable);
        }

        public async ValueTask Uninitialize(ILifetimeScope scope)
        {
            var panelManager = scope.Resolve<RegisteredLayoutPanel>();
            var documentManager = scope.Resolve<DocumentManager>();
            panelManager.CollectionChanged -= PanelManager_CollectionChanged;
            documentManager.CollectionChanged -= DocumentManager_CollectionChanged;

            await WindowManager.BeginUIThreadScope(() => this.LayoutDocumentPane.Children.Clear());

            var serializer = new XmlLayoutSerializer(dockManager);
            using var writer = new StreamWriter("layout.xml");
            serializer.Serialize(writer);
            Hide();
            var cfg = Config.Create();
            cfg.Width = this.Width;
            cfg.Height = this.Height;
            await Config.SaveAsync(cfg);
        }

        private void PanelManager_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems.OfType<LayoutAnchorable>())
                {
                    InsertToLayout(item);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems.OfType<LayoutAnchorable>())
                {
                    RemoveFromLayout(item);
                }
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WindowManager.BeginUIThreadScope(() => Kernel.Exit());
            e.Cancel = true;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async ValueTask Handle(AllPluginResolvedEvent @event)
        {
            await WindowManager.BeginUIThreadScope(() =>
            {
                var config = Config.Create();
                if (config.Width > 0 && config.Height > 0)
                {
                    this.Width = config.Width; this.Height = config.Height;
                }
                if (config.LastProject.Length > 0)
                {
                    Task.Run(() => EventBus.Publish(new ProjectSelectedEvent() { SelectedAddonInfoFile = config.LastProject }));
                }
                //if (File.Exists("layout.xml"))
                //{
                //    Logger.LogInformation("Restoring layout");
                //    var serializer = new XmlLayoutSerializer(dockManager);
                    
                //    serializer.Deserialize("layout.xml");
                //    Logger.LogInformation("Layout restored");

                //}
            });
        }

        public async ValueTask Handle(ProjectLoadedEvent @event)
        {
            var cfg = Config.Create();
            cfg.LastProject = @event.AddonInfoFile;
            await Config.SaveAsync(cfg);
        }

        public async ValueTask Handle(ProjectUnloadEvent @event)
        {
            var cfg = Config.Create();
            cfg.LastProject = string.Empty;
            await Config.SaveAsync(cfg);
        }

        private void dockManager_DocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            this.LayoutDocumentPane.Children.Remove(e.Document);
        }
    }
}
