using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Components;
using System.Threading.Tasks;
using GameFinder.StoreHandlers;
using GameFinder.StoreHandlers.Steam;
using EmberKernel.Services.EventBus.Handlers;
using Dota2Modding.VisualEditor.Plugins.Project.Abstraction.Events;
using Dota2Modding.VisualEditor.Plugins.Project.Menu;
using EmberKernel;
using EmberKernel.Services.EventBus;
using Dota2Modding.VisualEditor.Plugins.Project.Abstraction;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using Dota2Modding.Common.Models.Addon;
using Dota2Modding.Common.Models;
using System;
using Dota2Modding.VisualEditor.GUI.Abstraction.EditorMenu;
using EmberKernel.Plugins.Attributes;
using System.Xml.Linq;
using EmberKernel.Services.UI.Mvvm.Extension;
using Dota2Modding.VisualEditor.Plugins.Project.Components;
using Dota2Modding.VisualEditor.GUI.EmberWpfCore.ViewModel;
using EmberCore.KernelServices.UI.View;
using System.Windows;

namespace Dota2Modding.VisualEditor.Plugins.Project
{
    [EmberPlugin(Author = "ZeroAsh", Name = "Dota 2 Modding - Project Manager", Version = "0.0.1")]
    public class ProjectManagerPlugin : Plugin, ICoreWpfPlugin
    {
        public void BuildApplication(Application application)
        {
            application.Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Dota2Modding.VisualEditor.Plugins.Project;Component/resources.xaml", UriKind.Absolute)
            });
        }

        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureComponent<ProjectManager>().AsSelf().SingleInstance();
            builder.ConfigureComponent<OpenProjectMenu>().AsSelf().SingleInstance();
            builder.ConfigureComponent<CloseProjectMenu>().AsSelf().SingleInstance();
            builder.ConfigureComponent<DotaProjectMenu>().AsSelf().SingleInstance();
            //builder.ConfigureComponent<DotaHeroesPanelMenu>().AsSelf().SingleInstance();
            builder.ConfigureComponent<AddonInfoPanel>().AsSelf().As<ILayoutedObject>().SingleInstance();
            builder.ConfigureComponent<ProjectExplorer>().AsSelf().As<ILayoutedObject>().SingleInstance();
            builder.ConfigureComponent<DotaHeroesPanel>().AsSelf().As<ILayoutedObject>().SingleInstance();
            builder.ConfigureComponent<DotaAbilitiesPanel>().AsSelf().As<ILayoutedObject>().SingleInstance();
        }

        public override async ValueTask Initialize(ILifetimeScope scope)
        {
            await scope.RegisterOrOpenPanel<AddonInfoPanel>();
            await scope.RegisterOrOpenPanel<ProjectExplorer>();
            await scope.RegisterOrOpenPanel<DotaHeroesPanel>();
            await scope.RegisterOrOpenPanel<DotaAbilitiesPanel>();

            scope.Subscription<ProjectSelectedEvent, ProjectManager>();

            scope.Subscription<ProjectLoadedEvent, OpenProjectMenu>();
            scope.Subscription<ProjectUnloadEvent, OpenProjectMenu>();

            scope.Subscription<ProjectLoadedEvent, CloseProjectMenu>();
            scope.Subscription<ProjectUnloadEvent, CloseProjectMenu>();

            scope.Subscription<ProjectLoadedEvent, AddonInfoPanel>();
            scope.Subscription<ProjectUnloadEvent, AddonInfoPanel>();

            scope.Subscription<ProjectLoadedEvent, ProjectExplorer>();
            scope.Subscription<ProjectUnloadEvent, ProjectExplorer>();

            scope.Subscription<ProjectLoadedEvent, DotaProjectMenu>();
            scope.Subscription<ProjectUnloadEvent, DotaProjectMenu>();

            scope.Subscription<ProjectLoadedEvent, DotaHeroesPanel>();
            scope.Subscription<ProjectUnloadEvent, DotaHeroesPanel>();

            scope.Subscription<ProjectLoadedEvent, DotaAbilitiesPanel>();
            scope.Subscription<ProjectUnloadEvent, DotaAbilitiesPanel>();

            await scope.InitializeMenuItem<OpenProjectMenu>();
            await scope.InitializeMenuItem<CloseProjectMenu>();
            //await scope.InitializeMenuItem<DotaHeroesPanelMenu>();
        }

        public override async ValueTask Uninitialize(ILifetimeScope scope)
        {
            await scope.UnInitializeMenuItem<OpenProjectMenu>();
            await scope.UnInitializeMenuItem<CloseProjectMenu>();
            //await scope.UnInitializeMenuItem<DotaHeroesPanelMenu>();
            scope.Unsubscription<ProjectSelectedEvent, ProjectManager>();

            scope.Unsubscription<ProjectUnloadEvent, OpenProjectMenu>();
            scope.Unsubscription<ProjectLoadedEvent, OpenProjectMenu>();
            scope.Unsubscription<ProjectUnloadEvent, CloseProjectMenu>();
            scope.Unsubscription<ProjectLoadedEvent, CloseProjectMenu>();

            scope.Unsubscription<ProjectLoadedEvent, AddonInfoPanel>();
            scope.Unsubscription<ProjectUnloadEvent, AddonInfoPanel>();

            scope.Unsubscription<ProjectLoadedEvent, ProjectExplorer>();
            scope.Unsubscription<ProjectUnloadEvent, ProjectExplorer>();

            scope.Unsubscription<ProjectLoadedEvent, DotaProjectMenu>();
            scope.Unsubscription<ProjectUnloadEvent, DotaProjectMenu>();

            scope.Unsubscription<ProjectLoadedEvent, DotaHeroesPanel>();
            scope.Unsubscription<ProjectUnloadEvent, DotaHeroesPanel>();

            scope.Unsubscription<ProjectLoadedEvent, DotaAbilitiesPanel>();
            scope.Unsubscription<ProjectUnloadEvent, DotaAbilitiesPanel>();
        }
    }
}