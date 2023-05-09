using Autofac;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.GUI.EditorMenu
{
    public abstract class AbstractMenuItem : ObservableCollection<IEditorMenuItem>, IEditorMenuItem
    {
        public abstract string Id { get; }
        public abstract string ParentId { get; }

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract event EventHandler? CanExecuteChanged;

        public abstract bool CanExecute(object? parameter);

        public abstract void Dispose();

        public abstract void Execute(object? parameter);

        public ValueTask InitializeMenuItem<T>(ILifetimeScope scope) where T : IEditorMenuItem
        {
            this.Add(scope.Resolve<T>());

            return ValueTask.CompletedTask;
        }

        public ValueTask UnInitializeMenuItem<T>(ILifetimeScope scope) where T : IEditorMenuItem
        {
            this.Remove(scope.Resolve<T>());

            return ValueTask.CompletedTask;
        }
    }
}
