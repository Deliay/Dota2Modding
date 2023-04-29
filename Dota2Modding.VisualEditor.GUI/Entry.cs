using Autofac;
using EmberCore.KernelServices.UI.View;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.Extension;
using EmberWpfCore.Components.Configuration.View;
using EmberWpfCore.Components.PluginsManager.View;
using EmberWpfCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dota2Modding.VisualEditor.GUI
{
    [EmberPlugin(Author = "ZeroAsh", Name = "Dota 2 Modding Visual Editor main entry", Version = "0.0.1")]
    public class Entry : Plugin, ICoreWpfPlugin
    {
        public void BuildApplication(Application application)
        {
            application.Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Dota2Modding.VisualEditor.GUI;component/shared.xaml", UriKind.Absolute)
            });
        }

        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureComponent<RegisteredTabs>();
            builder.ConfigureWpfWindow<MainWindow>();
        }

        public override async ValueTask Initialize(ILifetimeScope scope)
        {
            await scope.InitializeWpfWindow<MainWindow>();
            await scope.InitializeUIComponent<PluginsTab>();
            await scope.InitializeUIComponent<ConfigurationTab>();
        }

        public override async ValueTask Uninitialize(ILifetimeScope scope)
        {
            await scope.UninitializeWpfWindow<MainWindow>();
            await scope.UninitializeUIComponent<PluginsTab>();
            await scope.UninitializeUIComponent<ConfigurationTab>();
        }
    }
}
