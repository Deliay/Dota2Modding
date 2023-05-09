using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.GUI.EditorMenu
{
    public interface IMenuItemManager : INotifyCollectionChanged, INotifyPropertyChanged, ICollection<IEditorMenuItem>, EmberKernel.Plugins.Components.IComponent
    {
        ValueTask InitializeMenuItem<T>(ILifetimeScope scope) where T : IEditorMenuItem;
        ValueTask UnInitializeMenuItem<T>(ILifetimeScope scope) where T : IEditorMenuItem;
    }
}
