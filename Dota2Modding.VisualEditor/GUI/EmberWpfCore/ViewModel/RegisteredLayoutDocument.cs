using AvalonDock.Layout;
using Dota2Modding.VisualEditor.GUI.EmberWpfCore.ViewModel;
using EmberKernel;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
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
        private class LayoutDocuments : ObservableCollection<LayoutDocument>
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

        private readonly LayoutDocuments items = new();

        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public RegisteredLayoutDocument()
        {
            items.CollectionChanged += Items_CollectionChanged;
            items.PropertyChanged += Items_PropertyChanged;
        }

        private void Items_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }

        private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(sender, e);
        }

        public void AddOrOpen<T>(string Id, string Title, T control) where T : ILayoutedObject
        {
            var exist = items.FirstOrDefault(doc => doc.ContentId == Id);
            if (exist is not null)
            {
                exist.IsActive = true;
            }
            else
            {
                items.Add(new LayoutDocument()
                {
                    ContentId = Id,
                    Title = Title,
                    Content = control,
                });
            }
        }

        public void Close(string Id)
        {
            var exist = items.FirstOrDefault(doc => doc.ContentId == Id);
            exist?.Close();
        }


        public void Dispose()
        {
        }
    }
}
