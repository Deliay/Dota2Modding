using Autofac;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.GUI.Abstraction.EditorMenu
{
    public static class EditorMenuManagerExtensions
    {
        public static ValueTask InitializeMenuItem<T>(this ILifetimeScope scope) where T : IEditorMenuItem
        {
            var itemManager = scope.Resolve<IMenuItemManager>();
            var windowManager = scope.Resolve<IWindowManager>();

            return windowManager.BeginUIThreadScope(() => itemManager.InitializeMenuItem<T>(scope));
        }

        public static ValueTask UnInitializeMenuItem<T>(this ILifetimeScope scope) where T : IEditorMenuItem
        {
            var itemManager = scope.Resolve<IMenuItemManager>();
            var windowManager = scope.Resolve<IWindowManager>();
            return windowManager.BeginUIThreadScope(() => itemManager.UnInitializeMenuItem<T>(scope));
        }
    }
}
