using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.GUI.Components.Consoles
{
    public class ConsoleObservableSubscriber : ObservableCollection<string>, IComponent
    {
        private readonly IWindowManager windowManager;
        private readonly CancellationTokenSource cts = new();

        public ConsoleObservableSubscriber(IWindowManager windowManager)
        {
            this.windowManager = windowManager;
            _ = Task.Run(async () =>
            {
                await foreach (var item in LoggerSink.Instance.allEvents())
                {
                    await this.windowManager.BeginUIThreadScope(() => this.Add(item));
                }
            }, cts.Token);
        }

        public void Dispose()
        {
            using var _cts = cts;
            GC.SuppressFinalize(this);
        }
    }
}
