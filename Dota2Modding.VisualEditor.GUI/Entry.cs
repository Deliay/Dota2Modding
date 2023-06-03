using Autofac;
using Dota2Modding.VisualEditor.Events;
using Dota2Modding.VisualEditor.GUI.Abstraction;
using Dota2Modding.VisualEditor.GUI.Abstraction.EditorMenu;
using Dota2Modding.VisualEditor.GUI.Abstraction.Menu;
using Dota2Modding.VisualEditor.GUI.Abstraction.Menu.Plugins;
using Dota2Modding.VisualEditor.GUI.Components.Consoles;
using Dota2Modding.VisualEditor.GUI.EmberWpfCore.ViewModel;
using Dota2Modding.VisualEditor.Plugins.Project.Abstraction.Events;
using EmberCore.KernelServices.UI.View;
using EmberKernel;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.Extension;
using EmberKernel.Services.UI.Mvvm.ViewModel.Configuration.Extension;
using EmberWpfCore.Components.Configuration.View;
using EmberWpfCore.Components.PluginsManager.View;
using EmberWpfCore.ViewModel;
using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dota2Modding.VisualEditor.GUI
{
    [EmberPlugin(Author = "ZeroAsh", Name = "Dota 2 Modding - Core", Version = "0.0.1")]
    public class Entry : Plugin, ICoreWpfPlugin
    {
        public void BuildApplication(Application application)
        {
            application.Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Dota2Modding.VisualEditor.GUI;component/shared.xaml", UriKind.Absolute)
            });
            application.Resources.MergedDictionaries.Add(ResourceHelper.GetTheme());
        }

        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.UsePluginOptionsModel<Entry, MainWindowConfiguration>();
            builder.ConfigureComponent<ConsoleObservableSubscriber>().AsSelf().SingleInstance();
            builder.ConfigureWpfWindow<MainWindow>();
            builder.ConfigureUIComponent<PluginsTab>();
            builder.ConfigureUIComponent<ConfigurationTab>();
            builder.ConfigureComponent<FileMenu>().AsSelf().SingleInstance();
            builder.ConfigureComponent<PluginMenu>().AsSelf().SingleInstance();
            builder.ConfigureComponent<MenuItemExit>().AsSelf().SingleInstance();
            builder.ConfigureComponent<InstalledPluginsMenu>().AsSelf().SingleInstance();
            builder.ConfigureComponent<ConsolePanel>().AsSelf().As<ILayoutedObject>().SingleInstance();
        }

        public override async ValueTask Initialize(ILifetimeScope scope)
        {
            await scope.InitializeWpfWindow<MainWindow>();
            await scope.InitializeUIComponent<PluginsTab>();
            await scope.InitializeUIComponent<ConfigurationTab>();
            await scope.InitializeMenuItem<FileMenu>();
            await scope.InitializeMenuItem<PluginMenu>();
            await scope.InitializeMenuItem<MenuItemExit>();
            await scope.InitializeMenuItem<InstalledPluginsMenu>();
            await scope.RegisterOrOpenPanel<ConsolePanel>();
            scope.Subscription<AllPluginResolvedEvent, MainWindow>();
            scope.Subscription<ProjectLoadedEvent, MainWindow>();
            scope.Subscription<ProjectUnloadEvent, MainWindow>();
        }

        public override async ValueTask Uninitialize(ILifetimeScope scope)
        {
            await scope.UninitializeWpfWindow<MainWindow>();
            await scope.UninitializeUIComponent<PluginsTab>();
            await scope.UninitializeUIComponent<ConfigurationTab>();
            await scope.UnInitializeMenuItem<FileMenu>();
            await scope.UnInitializeMenuItem<PluginMenu>();
            await scope.UnInitializeMenuItem<PluginMenu>();
            await scope.UnInitializeMenuItem<InstalledPluginsMenu>();
            scope.Unsubscription<AllPluginResolvedEvent, MainWindow>();
            scope.Unsubscription<ProjectLoadedEvent, MainWindow>();
            scope.Unsubscription<ProjectUnloadEvent, MainWindow>();
        }
    }
}
