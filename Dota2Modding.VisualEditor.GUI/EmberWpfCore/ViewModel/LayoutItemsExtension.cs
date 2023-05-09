using Autofac;
using AvalonDock;
using AvalonDock.Layout;
using EmberKernel.Services.UI.Mvvm.Extension;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
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
        public static async ValueTask RegisterPanel<T>(this ILifetimeScope scope, DockingManager dockingManager, T obj) where T : ILayoutedObject
        {
            await obj.Initialize(scope);
            if (obj is ILayoutedDocument)
            {
                var manager = scope.Resolve<RegisteredLayoutDocument>();

                manager.AddOrOpen(obj.Id, obj.Title, obj);
            }
            else if (obj is ILayoutedPanel panel)
            {
                var manager = scope.Resolve<RegisteredLayoutPanel>();

                var layout = panel is IDefaultLayoutStrategy defaultLayoutItem ? defaultLayoutItem.DefaultStrategy : AnchorableShowStrategy.Left;

                await manager.AddOrOpen(dockingManager, panel.Id, panel.Title, panel, layout);
            }

        }
    }
}
