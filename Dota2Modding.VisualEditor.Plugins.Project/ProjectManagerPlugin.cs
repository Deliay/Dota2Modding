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

namespace Dota2Modding.VisualEditor.Plugins.Project
{
    [EmberPlugin(Author = "ZeroAsh", Name = "Dota 2 Modding - Project Manager", Version = "0.0.1")]
    public class ProjectManagerPlugin : Plugin
    {

        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureComponent<ProjectManager>().AsSelf().SingleInstance();
            builder.ConfigureComponent<Dota2Locator>().AsSelf().SingleInstance();
            builder.ConfigureComponent<OpenProjectMenu>().AsSelf().SingleInstance();
        }

        public override async ValueTask Initialize(ILifetimeScope scope)
        {
            scope.Subscription<ProjectSelectedEvent, ProjectManager>();
            scope.Subscription<ProjectLoadedEvent, OpenProjectMenu>();
            scope.Subscription<ProjectUnloadEvent, OpenProjectMenu>();
            await scope.InitializeMenuItem<OpenProjectMenu>();
        }

        public override async ValueTask Uninitialize(ILifetimeScope scope)
        {
            await scope.UnInitializeMenuItem<OpenProjectMenu>();
            scope.Unsubscription<ProjectUnloadEvent, OpenProjectMenu>();
            scope.Unsubscription<ProjectLoadedEvent, OpenProjectMenu>();
            scope.Unsubscription<ProjectSelectedEvent, ProjectManager>();
        }
    }
}