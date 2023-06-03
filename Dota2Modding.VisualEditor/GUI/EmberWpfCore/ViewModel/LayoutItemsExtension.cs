using Autofac;
using AvalonDock;
using AvalonDock.Layout;
using EmberKernel.Services.UI.Mvvm.Extension;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using EmberWpfCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Dota2Modding.VisualEditor.GUI.EmberWpfCore.ViewModel
{
    public static class LayoutItemsExtension
    {
        public static async ValueTask RegisterOrOpenPanel<T>(this ILifetimeScope scope) where T : ILayoutedObject
        {
            var wm = scope.Resolve<IWindowManager>();
            await wm.BeginUIThreadScope(async () =>
            {
                var obj = scope.Resolve<T>();
                await obj.Initialize(scope);
                if (obj is ILayoutedDocument doc)
                {
                    var manager = scope.Resolve<RegisteredLayoutDocument>();

                    manager.AddOrOpen(doc);
                }
                else if (obj is ILayoutedPanel panel)
                {
                    var manager = scope.Resolve<RegisteredLayoutPanel>();

                    var pos = panel is IDefaultLayoutStrategy defaultLayoutItem ? defaultLayoutItem.DefaultStrategy : AnchorSide.Left;

                    var anchorable = await manager.AddOrOpen(panel.Id, panel.Title, panel);

                }
            });
        }

        public static async ValueTask RegisterOrOpenPanel<T>(this ILifetimeScope scope, T obj) where T : ILayoutedObject
        {
            await obj.Initialize(scope);
            var wm = scope.Resolve<IWindowManager>();
            await wm.BeginUIThreadScope(async () =>
            {
                if (obj is ILayoutedDocument doc)
                {
                    var manager = scope.Resolve<RegisteredLayoutDocument>();

                    manager.AddOrOpen(doc);
                }
                else if (obj is ILayoutedPanel panel)
                {
                    var manager = scope.Resolve<RegisteredLayoutPanel>();

                    var pos = panel is IDefaultLayoutStrategy defaultLayoutItem ? defaultLayoutItem.DefaultStrategy : AnchorSide.Left;

                    var anchorable = await manager.AddOrOpen(panel.Id, panel.Title, panel);

                }
            });
        }
    }
}
