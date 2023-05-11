using EmberKernel.Services.UI.Mvvm.ViewComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor.GUI.Abstraction.Document
{
    public interface IDocumentPage : IViewComponent
    {
        public string Id { get; }
        public string Title { get; }
    }
}
