using Dota2Modding.VisualEditor.GUI.Abstraction.EditorMenu;
using EmberKernel.Plugins.Models;
using EmberKernel.Services.UI.Mvvm.ViewModel.Plugins;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.GUI.Abstraction.Menu.Plugins
{
    public class InstalledPluginsMenu : AbstractMenu
    {
        public InstalledPluginsMenu(IPluginManagerViewModel pluginDescriptors)
        {
            this.pluginDescriptors = pluginDescriptors;
            pluginDescriptors.CollectionChanged += PluginDescriptors_CollectionChanged;
            ProcessItems(pluginDescriptors);
        }

        private string IdOf(PluginDescriptor descriptor) => $"{GUID}_{descriptor.Name}_{descriptor.Author}_{descriptor.Version}";

        private void ProcessItems(IEnumerable enumerator)
        {
            foreach (var item in enumerator)
            {
                if (item is PluginDescriptor descriptor)
                {
                    Add(new InstalledPluginMenuItem(IdOf(descriptor), descriptor.Name, descriptor.Version));
                }
            }
        }

        private void PluginDescriptors_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                ProcessItems(e.NewItems);
            }
            else
            {
                foreach (var item in e.OldItems)
                {
                    if (item is PluginDescriptor descriptor)
                    {
                        var targetId = IdOf(descriptor);
                        var findedItem = this.FirstOrDefault(i => i.Id == targetId);
                        if (findedItem is not null)
                        {
                            Remove(findedItem);
                        }
                    }
                }
            }
        }

        public const string GUID = "F68A4DFC-0706-45A4-9DD5-8C70FC9C45DA";
        private readonly IPluginManagerViewModel pluginDescriptors;

        public override string Id => GUID;

        public override string ParentId => PluginMenu.GUID;

        public override string Name => "Installed";

        public override string Description => "Installed plugins";

        public override event EventHandler? CanExecuteChanged;

        public override void Dispose()
        {
            pluginDescriptors.CollectionChanged -= PluginDescriptors_CollectionChanged;
            GC.SuppressFinalize(this);
        }

    }
}
