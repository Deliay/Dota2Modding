﻿using Autofac;
using AvalonDock.Layout;
using Dota2Modding.Common.Models.GameStructure;
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
    /// Interaction logic for ProjectExplorer.xaml
    /// </summary>
    public partial class ProjectExplorer : UserControl,
        IViewComponent, ILayoutedPanel, IDefaultLayoutStrategy,
        IEventHandler<ProjectLoadedEvent>, IEventHandler<ProjectUnloadEvent>
    {
        private ILifetimeScope scope;
        private IWindowManager windowManager;

        public ProjectExplorer()
        {
            InitializeComponent();
        }

        public int InitialHeight => 400;

        public string Id => "[Guid(\"4BA71D6F-0EA9-4F77-BEC1-F5270F741CDD\")]";

        public string Title => "VPK Explorer";

        public AnchorSide DefaultStrategy => AnchorSide.Right;

        public bool Closeable => false;

        public bool InitialDock => true;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async ValueTask Handle(ProjectLoadedEvent @event)
        {
            var pm = scope.Resolve<ProjectManager>();
            await windowManager.BeginUIThreadScope(() =>
            {
                tree.ItemsSource = pm.DotaProject.Packages.RootFolder;
            });
        }

        public async ValueTask Handle(ProjectUnloadEvent @event)
        {
            await windowManager.BeginUIThreadScope(() =>
            {
                this.tree.ItemsSource = null;
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
