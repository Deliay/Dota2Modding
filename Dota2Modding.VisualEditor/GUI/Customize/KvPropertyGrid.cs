using Dota2Modding.Common.Models;
using Dota2Modding.Common.Models.Converters;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ValveKeyValue;

namespace Dota2Modding.VisualEditor.GUI.Customize
{

    [TemplatePart(Name = ElementItemsControl, Type = typeof(ItemsControl))]
    [TemplatePart(Name = ElementSearchBar, Type = typeof(SearchBar))]
    public class KvPropertyGrid : Control
    {
        private const string ElementItemsControl = "PART_ItemsControl";

        private const string ElementSearchBar = "PART_SearchBar";

        private ItemsControl _itemsControl;

        private ICollectionView _dataView;

        private SearchBar _searchBar;

        private string _searchKey;

        public KvPropertyGrid()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.SortByCategory, SortByCategory, (s, e) => e.CanExecute = ShowSortButton));
            CommandBindings.Add(new CommandBinding(ControlCommands.SortByName, SortByName, (s, e) => e.CanExecute = ShowSortButton));
        }

        public virtual KvPropertyResolver PropertyResolver { get; } = new();

        public static readonly RoutedEvent SelectedObjectChangedEvent =
            EventManager.RegisterRoutedEvent("SelectedObjectChanged", RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<object>), typeof(KvPropertyGrid));

        public event RoutedPropertyChangedEventHandler<object> SelectedObjectChanged
        {
            add => AddHandler(SelectedObjectChangedEvent, value);
            remove => RemoveHandler(SelectedObjectChangedEvent, value);
        }

        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register(
            nameof(SelectedObject), typeof(object), typeof(KvPropertyGrid), new PropertyMetadata(default, OnSelectedObjectChanged));

        private static void OnSelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (KvPropertyGrid)d;
            ctl.OnSelectedObjectChanged(e.OldValue, e.NewValue);
        }

        public object SelectedObject
        {
            get => GetValue(SelectedObjectProperty);
            set => SetValue(SelectedObjectProperty, value);
        }

        protected virtual void OnSelectedObjectChanged(object oldValue, object newValue)
        {
            UpdateItems(newValue);
            RaiseEvent(new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, SelectedObjectChangedEvent));
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description), typeof(string), typeof(KvPropertyGrid), new PropertyMetadata(default(string)));

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public static readonly DependencyProperty MaxTitleWidthProperty = DependencyProperty.Register(
            nameof(MaxTitleWidth), typeof(double), typeof(KvPropertyGrid), new PropertyMetadata(.0));

        public double MaxTitleWidth
        {
            get => (double)GetValue(MaxTitleWidthProperty);
            set => SetValue(MaxTitleWidthProperty, value);
        }

        public static readonly DependencyProperty MinTitleWidthProperty = DependencyProperty.Register(
            nameof(MinTitleWidth), typeof(double), typeof(KvPropertyGrid), new PropertyMetadata(.0));

        public double MinTitleWidth
        {
            get => (double)GetValue(MinTitleWidthProperty);
            set => SetValue(MinTitleWidthProperty, value);
        }

        public static readonly DependencyProperty ShowSortButtonProperty = DependencyProperty.Register(
            nameof(ShowSortButton), typeof(bool), typeof(KvPropertyGrid), new PropertyMetadata(true));

        public bool ShowSortButton
        {
            get => (bool)GetValue(ShowSortButtonProperty);
            set => SetValue(ShowSortButtonProperty, value);
        }

        public override void OnApplyTemplate()
        {
            if (_searchBar != null)
            {
                _searchBar.SearchStarted -= SearchBar_SearchStarted;
            }

            base.OnApplyTemplate();

            _itemsControl = GetTemplateChild(ElementItemsControl) as ItemsControl;
            _searchBar = GetTemplateChild(ElementSearchBar) as SearchBar;

            if (_searchBar != null)
            {
                _searchBar.SearchStarted += SearchBar_SearchStarted;
            }

            UpdateItems(SelectedObject);
        }

        private IEnumerable<PropertyDescriptor> GetPropertyDescriptors(object obj)
        {
            if (obj is KVObject kvObject)
            {
                var kv = new KvObjectDescriptionProvider(kvObject);
                var kvDescriptors = kv.GetTypeDescriptor(typeof(KVObject), obj);
                if (kvDescriptors is not null)
                {
                    foreach (var kvProp in kvDescriptors.GetProperties(null).Cast<KvPropertyDescriptor>())
                    {
                        yield return kvProp;
                    }
                }
            }
            foreach (var item in TypeDescriptor.GetProperties(obj).OfType<PropertyDescriptor>())
            {
                yield return item;
            }
        }

        private void UpdateItems(object obj)
        {
            if (obj == null || _itemsControl == null) return;

            _dataView = CollectionViewSource.GetDefaultView(GetPropertyDescriptors(obj)
                .Where(PropertyResolver.ResolveIsBrowsable).Select(CreatePropertyItem)
                .Do(item => item.InitElement()));

            SortByCategory(null, null);
            _itemsControl.ItemsSource = _dataView;
        }

        private void SortByCategory(object sender, ExecutedRoutedEventArgs e)
        {
            if (_dataView == null) return;

            using (_dataView.DeferRefresh())
            {
                _dataView.GroupDescriptions.Clear();
                _dataView.SortDescriptions.Clear();
                _dataView.SortDescriptions.Add(new SortDescription(PropertyItem.CategoryProperty.Name, ListSortDirection.Ascending));
                _dataView.SortDescriptions.Add(new SortDescription(PropertyItem.DisplayNameProperty.Name, ListSortDirection.Ascending));
                _dataView.GroupDescriptions.Add(new PropertyGroupDescription(PropertyItem.CategoryProperty.Name));
            }
        }

        private void SortByName(object sender, ExecutedRoutedEventArgs e)
        {
            if (_dataView == null) return;

            using (_dataView.DeferRefresh())
            {
                _dataView.GroupDescriptions.Clear();
                _dataView.SortDescriptions.Clear();
                _dataView.SortDescriptions.Add(new SortDescription(PropertyItem.PropertyNameProperty.Name, ListSortDirection.Ascending));
            }
        }

        private void SearchBar_SearchStarted(object sender, FunctionEventArgs<string> e)
        {
            if (_dataView == null) return;

            _searchKey = e.Info;
            if (string.IsNullOrEmpty(_searchKey))
            {
                foreach (UIElement item in _dataView)
                {
                    item.Show();
                }
            }
            else
            {
                foreach (PropertyItem item in _dataView)
                {
                    item.Show(item.PropertyName.ToLower().Contains(_searchKey) || item.DisplayName.ToLower().Contains(_searchKey));
                }
            }
        }

        protected virtual PropertyItem CreatePropertyItem(PropertyDescriptor propertyDescriptor) => new()
        {
            Category = PropertyResolver.ResolveCategory(propertyDescriptor),
            DisplayName = PropertyResolver.ResolveDisplayName(propertyDescriptor),
            Description = PropertyResolver.ResolveDescription(propertyDescriptor),
            IsReadOnly = PropertyResolver.ResolveIsReadOnly(propertyDescriptor),
            DefaultValue = PropertyResolver.ResolveDefaultValue(propertyDescriptor),
            Editor = PropertyResolver.ResolveEditor(propertyDescriptor),
            Value = SelectedObject,
            PropertyName = propertyDescriptor.Name,
            PropertyType = propertyDescriptor.PropertyType,
            PropertyTypeName = $"{propertyDescriptor.PropertyType.Namespace}.{propertyDescriptor.PropertyType.Name}"
        };

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            TitleElement.SetTitleWidth(this, new GridLength(Math.Max(MinTitleWidth, Math.Min(MaxTitleWidth, ActualWidth / 3))));
        }
    }
}
