using AvalonDock.Layout;
using Dota2Modding.VisualEditor.GUI.EmberWpfCore.ViewModel;
using EmberKernel;
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
using System.Windows.Controls;

namespace EmberWpfCore.ViewModel
{
    public class RegisteredLayoutDocument :  INotifyCollectionChanged, INotifyPropertyChanged, IKernelService
    {
        private readonly IWindowManager windowManager;

        public class LayoutDocuments : ObservableCollection<LayoutDocument>
        {
            public new event PropertyChangedEventHandler? PropertyChanged;
            public LayoutDocuments()
            {
                base.PropertyChanged += LayoutDocuments_PropertyChanged;
            }

            private void LayoutDocuments_PropertyChanged(object? sender, PropertyChangedEventArgs e)
            {
                this.PropertyChanged?.Invoke(sender, e);
            }
        }

        public LayoutDocuments Items { get; } = new();

        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public RegisteredLayoutDocument(IWindowManager windowManager)
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

        public void AddOrOpen<T>(T control) where T : ILayoutedDocument
        {
            windowManager.BeginUIThreadScope(() =>
            {

                var exist = Items.FirstOrDefault(doc => doc.ContentId == control.Id);
                if (exist is not null)
                {
                    exist.IsActive = true;
                }
                else
                {
                    Items.Add(new LayoutDocument()
                    {
                        ContentId = control.Id,
                        Title = control.Title,
                        Content = control,
                        CanClose = control.Closeable,
                    });
                }
            });
        }

        public void Close(string Id)
        {
            var exist = Items.FirstOrDefault(doc => doc.ContentId == Id);
            exist?.Close();
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
