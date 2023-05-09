using Autofac;
using Dota2Modding.VisualEditor.GUI.EditorMenu;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.GUI.Service
{
    public class EditorMenuManager : ObservableCollection<IEditorMenuItem>, IMenuItemManager
    {
        private readonly Dictionary<string, List<IEditorMenuItem>> _cache = new();
        private readonly Dictionary<IEditorMenuItem, IEditorMenuItem> parents = new();

        public void Dispose()
        {
            this.Clear();
            _cache.Clear();
            parents.Clear();
            GC.SuppressFinalize(this);
        }

        public async ValueTask<IEditorMenuItem> ResolveParent(string id, TimeSpan timeout, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<IEditorMenuItem>();
            NotifyCollectionChangedEventHandler handler = (object? sender, NotifyCollectionChangedEventArgs e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var item in e.NewItems)
                    {
                        if (item is IEditorMenuItem menuItem)
                        {
                            if (menuItem.Id == id)
                            {
                                tcs.SetResult(menuItem);
                            }
                        }
                    }
                }
            };
            this.CollectionChanged += handler;
            
            return await tcs.Task.ContinueWith((r) =>
            {
                this.CollectionChanged -= handler;
                return r;
            }).Unwrap().WaitAsync(timeout);
        }

        public ValueTask InitializeMenuItem<T>(ILifetimeScope scope) where T : IEditorMenuItem
        {
            var menuItem = scope.Resolve<T>();

            if (menuItem.ParentId == null)
            {
                this.Add(menuItem);
                parents.Add(menuItem, null!);
            }
            else
            {
                var parent = this.FirstOrDefault(m => m.Id == menuItem.ParentId);
                if (parent == null)
                {
                    if (!_cache.ContainsKey(menuItem.ParentId))
                    {
                        _cache.Add(menuItem.ParentId, new());
                    }

                    _cache[menuItem.ParentId].Add(menuItem);
                }
                else
                {
                    parent.Add(menuItem);
                    this.parents.Add(menuItem, parent);
                }
            }

            if (_cache.TryGetValue(menuItem.Id, out var value))
            {
                foreach (var item in value)
                {
                    menuItem.Add(item);
                    parents.Add(item, menuItem);
                }
                _cache.Remove(menuItem.Id);
            }

            return ValueTask.CompletedTask;
        }

        public ValueTask UnInitializeMenuItem<T>(ILifetimeScope scope) where T : IEditorMenuItem
        {
            var menuItem = scope.Resolve<T>();

            if (parents.TryGetValue(menuItem, out var parent))
            {
                if (parent is null)
                {
                    this.Remove(menuItem);
                }
                else
                {
                    parent.Remove(menuItem);
                }
                parents.Remove(menuItem);
            }

            return ValueTask.CompletedTask;
        }
    }
}
