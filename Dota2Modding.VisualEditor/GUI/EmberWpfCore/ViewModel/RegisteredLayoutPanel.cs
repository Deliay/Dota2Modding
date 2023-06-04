using AvalonDock;
using AvalonDock.Layout;
using Dota2Modding.VisualEditor.GUI;
using Dota2Modding.VisualEditor.GUI.EmberWpfCore.ViewModel;
using EmberKernel;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using HandyControl.Tools.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EmberWpfCore.ViewModel
{
    public class RegisteredLayoutPanel : INotifyCollectionChanged, INotifyPropertyChanged, IKernelService
    {
        public class LayoutPanels : ObservableCollection<LayoutAnchorable>
        {
            public new event PropertyChangedEventHandler? PropertyChanged;
            public LayoutPanels()
            {
                base.PropertyChanged += LayoutDocuments_PropertyChanged;
            }

            private void LayoutDocuments_PropertyChanged(object? sender, PropertyChangedEventArgs e)
            {
                this.PropertyChanged?.Invoke(sender, e);
            }
        }

        public LayoutPanels Items { get; } = new();
        private readonly IWindowManager windowManager;

        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public RegisteredLayoutPanel(IWindowManager windowManager)
        {
            Items.CollectionChanged += Items_CollectionChanged;
            Items.PropertyChanged += Items_PropertyChanged;
            this.windowManager = windowManager;
        }

        private void Items_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }

        private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(sender, e);
        }

        public async ValueTask<LayoutAnchorable> AddOrOpen<T>(string Id, string Title, T control) where T : ILayoutedPanel
        {
            return await windowManager.BeginUIThreadScope(() =>
            {
                var exist = Items.FirstOrDefault(doc => doc.ContentId == Id);
                if (exist is not null)
                {
                    exist.IsActive = true;
                    return exist;
                }
                else
                {
                    var item = new LayoutAnchorable()
                    {
                        ContentId = Id,
                        Title = Title,
                        Content = control,
                        CanClose = control.Closeable,
                        CanHide = control.Closeable,
                    };
                    Items.Add(item);
                    return item;
                }
            });
        }

        public async ValueTask Close(string Id)
        {
            await windowManager.BeginUIThreadScope(() =>
            {
                var exist = Items.FirstOrDefault(doc => doc.ContentId == Id);
                exist?.Close();
            });
        }

    }
}
