﻿using AvalonDock;
using AvalonDock.Layout;
using Dota2Modding.VisualEditor.GUI;
using Dota2Modding.VisualEditor.GUI.EmberWpfCore.ViewModel;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
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
    public class RegisteredLayoutPanel : INotifyCollectionChanged, INotifyPropertyChanged, EmberKernel.Plugins.Components.IComponent
    {
        private class LayoutPanels : ObservableCollection<LayoutAnchorable>
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

        private readonly LayoutPanels items = new();
        private readonly IWindowManager windowManager;

        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public RegisteredLayoutPanel(IWindowManager windowManager)
        {
            items.CollectionChanged += Items_CollectionChanged;
            items.PropertyChanged += Items_PropertyChanged;
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

        public async ValueTask AddOrOpen<T>(DockingManager dockingManager, string Id, string Title, T control, AnchorSide pos) where T : ILayoutedPanel
        {
            await windowManager.BeginUIThreadScope(() =>
            {
                var exist = items.FirstOrDefault(doc => doc.ContentId == Id);
                if (exist is not null)
                {
                    exist.IsActive = true;
                }
                else
                {
                    var item = new LayoutAnchorable()
                    {
                        ContentId = Id,
                        Title = Title,
                        Content = control,
                    };
                    items.Add(item);
                    item.AddToLayout(dockingManager, AnchorableShowStrategy.Most);
                    var pane = pos switch
                    {
                        AnchorSide.Left => dockingManager.Layout.LeftSide,
                        AnchorSide.Right => dockingManager.Layout.RightSide,
                        AnchorSide.Top => dockingManager.Layout.TopSide,
                        _ => dockingManager.Layout.BottomSide,
                    };
                    var group = pane.Descendents().OfType<LayoutAnchorGroup>().FirstOrDefault()!;

                    group.InsertChildAt(0, item);
                }
            });
        }

        public async ValueTask Close(string Id)
        {
            await windowManager.BeginUIThreadScope(() =>
            {
                var exist = items.FirstOrDefault(doc => doc.ContentId == Id);
                exist?.Close();
            });
        }


        public void Dispose()
        {
        }
    }
}
