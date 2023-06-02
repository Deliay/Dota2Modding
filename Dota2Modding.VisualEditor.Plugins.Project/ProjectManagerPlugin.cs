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

namespace Dota2Modding.VisualEditor.Plugins.Project
{
    [EmberPlugin(Author = "ZeroAsh", Name = "Dota 2 Modding - Project Manager", Version = "0.0.1")]
    public class ProjectManagerPlugin : Plugin
    {

        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureComponent<ProjectManager>().AsSelf().SingleInstance();
            builder.ConfigureComponent<OpenProjectMenu>().AsSelf().SingleInstance();
            builder.ConfigureComponent<AddonInfoPanel>().AsSelf().As<ILayoutedObject>().SingleInstance();
            builder.ConfigureComponent<ProjectExplorer>().AsSelf().As<ILayoutedObject>().SingleInstance();
        }

        public override async ValueTask Initialize(ILifetimeScope scope)
        {
            await scope.RegisterPanel<AddonInfoPanel>();
            await scope.RegisterPanel<ProjectExplorer>();
            scope.Subscription<ProjectSelectedEvent, ProjectManager>();
            scope.Subscription<ProjectLoadedEvent, OpenProjectMenu>();
            scope.Subscription<ProjectUnloadEvent, OpenProjectMenu>();
            scope.Subscription<ProjectLoadedEvent, AddonInfoPanel>();
            scope.Subscription<ProjectUnloadEvent, AddonInfoPanel>();
            scope.Subscription<ProjectLoadedEvent, ProjectExplorer>();
            scope.Subscription<ProjectUnloadEvent, ProjectExplorer>();
            await scope.InitializeMenuItem<OpenProjectMenu>();
        }

        public override async ValueTask Uninitialize(ILifetimeScope scope)
        {
            await scope.UnInitializeMenuItem<OpenProjectMenu>();
            scope.Unsubscription<ProjectSelectedEvent, ProjectManager>();
            scope.Unsubscription<ProjectUnloadEvent, OpenProjectMenu>();
            scope.Unsubscription<ProjectLoadedEvent, OpenProjectMenu>();
            scope.Unsubscription<ProjectLoadedEvent, AddonInfoPanel>();
            scope.Unsubscription<ProjectUnloadEvent, AddonInfoPanel>();
            scope.Unsubscription<ProjectLoadedEvent, ProjectExplorer>();
            scope.Unsubscription<ProjectUnloadEvent, ProjectExplorer>();
        }
    }
}